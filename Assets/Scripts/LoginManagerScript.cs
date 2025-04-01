using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class LoginManagerScript : MonoBehaviour
{
    public static LoginManagerScript Instance;
    public bool IsLoggedIn = false;

    [Header("UI Elements")]
    public TMP_InputField username;
    public TMP_InputField password;
    public TMP_InputField username1;
    public TMP_InputField password1;
    public Button loginButton;
    public Button registerButton;
    public TMP_Text statusText;

    [Header("Systems")]
    public GameObject RegisterSystem;
    public GameObject LoginSystem;
    public GameObject TrajectSystem;
    

    [Header("Dependencies")]
    public UserApiClient userApiClient;

    public void Start()
    {
        RegisterSystem.SetActive(true);
        LoginSystem.SetActive(true);
        TrajectSystem.SetActive(false);
    }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Zorgt ervoor dat dit object blijft bestaan bij scene-wisseling
        }
        else
        {
            Destroy(gameObject); // Voorkomt meerdere instanties van LoginManagerScript
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if (username.isFocused)
            {
                password.Select();
            }
            if (password.isFocused)
            {
                username.Select();
            }
        }

        if(Input.GetKeyDown(KeyCode.Return))
        {
            Login();
        }
    }

    public async void Login()
    {
        if (string.IsNullOrEmpty(username1.text) || string.IsNullOrEmpty(password1.text))
        {
            ShowMessage("Vul alle velden in!", Color.red);
            return;
        }

        User user = new User { email = username1.text, password = password1.text };
        IWebRequestReponse response = await userApiClient.Login(user);

        switch (response)
        {
            case WebRequestData<string> dataResponse:
                IsLoggedIn = true;
                ShowMessage("Login succesvol!", Color.green);
                Debug.Log("Gebruiker is ingelogd!");

                RegisterSystem.SetActive(false);
                LoginSystem.SetActive(false);
                TrajectSystem.SetActive(true);

                break;
            case WebRequestError errorResponse:
                ShowMessage("Login fout: " + errorResponse.ErrorMessage, Color.red);
                Debug.LogError("Login fout: " + errorResponse.ErrorMessage);
                break;
            default:
                Debug.LogError("Onbekende login response ontvangen");
                break;
        }
    }

    public async void Register()
    {
        if (string.IsNullOrEmpty(username1.text) || string.IsNullOrEmpty(password1.text))
        {
            ShowMessage("Vul alle velden in!", Color.red);
            return;
        }

        User user = new User { email = username1.text, password = password1.text };
        IWebRequestReponse response = await userApiClient.Register(user);

        switch (response)
        {
            case WebRequestData<string> dataResponse:
                ShowMessage("Registratie succesvol!", Color.green);
                Debug.Log("Gebruiker geregistreerd!");
                Login();
                break;
            case WebRequestError errorResponse:
                ShowMessage("Fout bij registratie: " + errorResponse.ErrorMessage, Color.red);
                Debug.LogError("Registratiefout: " + errorResponse.ErrorMessage);
                break;
            default:
                Debug.LogError("Onbekende registratie response ontvangen");
                break;
        }
    }

    public void Logout()
    {
        IsLoggedIn = false;
        ShowMessage("Je bent uitgelogd!", Color.yellow);
        Debug.Log("Gebruiker is uitgelogd!");
    }

    private void ShowMessage(string message, Color color)
    {
        if (statusText != null)
        {
            statusText.text = message;
            statusText.color = color;
        }
    }

    public void LoadNextScene(string sceneName)
    {
        if (IsLoggedIn)
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            ShowMessage("Je moet eerst inloggen!", Color.red);
        }
    }
}
