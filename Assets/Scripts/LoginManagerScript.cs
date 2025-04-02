using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System.Collections.Generic;

public class LoginManagerScript : MonoBehaviour
{
    public static LoginManagerScript Instance;
    //public bool IsLoggedIn = false;

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
    private ApiClientHolder ApiClientHolder;
    private UserApiClient userApiClient;
    private ParentGuardianApiClient parentGuardianApiClient;
    private PatientApiClient patientApiClient;

    public void Start()
    {
        ApiClientHolder = ApiClientHolder.instance;
        userApiClient = ApiClientHolder.userApiClient;
        parentGuardianApiClient = ApiClientHolder.parentGuardianApiClient;
        patientApiClient = ApiClientHolder.patientApiClient;

        RegisterSystem.SetActive(true);
        LoginSystem.SetActive(true);
        TrajectSystem.SetActive(false);
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
                ShowMessage("Login succesvol!", Color.green);
                Debug.Log("Gebruiker is ingelogd!");

                ReadPatientInfo();

                SceneManager.LoadScene("TreatmentPlanPage");

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

    private async void ReadPatientInfo()
    {
        string parentGuardianId = "";

        IWebRequestReponse response3 = await parentGuardianApiClient.ReadParentGuardians();

        switch (response3)
        {
            case WebRequestData<List<ParentGuardian>> dataResponse3:

                parentGuardianId = dataResponse3.Data[0].id;

                break;
            case WebRequestError errorResponse3:
                Debug.LogError("Fout bij ophalen: " + errorResponse3.ErrorMessage);
                break;
            default:
                Debug.LogError("Onbekende respons ontvangen");
                break;
        }

        ApiClientHolder.ParentGuardianId = parentGuardianId;

        IWebRequestReponse response2 = await patientApiClient.ReadPatientsByParentGuardian(parentGuardianId);

        switch (response2)
        {
            case WebRequestData<List<Patient>> dataResponse:
                ApiClientHolder.Patient = dataResponse.Data[0];
                break;
            case WebRequestError errorResponse:
                Debug.LogError("Fout bij opslaan: " + errorResponse.ErrorMessage);
                break;
            default:
                Debug.LogError("Onbekende respons ontvangen");
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
                LoginAfterRegister(user);
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

    private async void LoginAfterRegister(User user)
    {

        //User user = new User { email = username1.text, password = password1.text };
        IWebRequestReponse response = await userApiClient.Login(user);

        switch (response)
        {
            case WebRequestData<string> dataResponse:

                //ShowMessage("Login succesvol!", Color.green);
                //Debug.Log("Gebruiker is ingelogd!");

                RegisterSystem.SetActive(false);
                LoginSystem.SetActive(false);
                TrajectSystem.SetActive(true);

                break;
            case WebRequestError errorResponse:
                //ShowMessage("Login fout: " + errorResponse.ErrorMessage, Color.red);
                //Debug.LogError("Login fout: " + errorResponse.ErrorMessage);
                break;
            default:
                //Debug.LogError("Onbekende login response ontvangen");
                break;
        }
    }

    public void Logout()
    {
        //IsLoggedIn = false;
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
        //if (IsLoggedIn)
        //{
            SceneManager.LoadScene(sceneName);
        //}
        //else
        //{
        //    ShowMessage("Je moet eerst inloggen!", Color.red);
        //}
    }

    public void LoadWelcomeParents(string route)
    {
        ApiClientHolder.Route = route;
        SceneManager.LoadScene("WelcomeParentsPage");
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

}
