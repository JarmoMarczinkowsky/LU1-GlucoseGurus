using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LoginManagerScript : MonoBehaviour
{
    public static LoginManagerScript Instance;
    public bool IsLoggedIn = false;

    [Header("UI Elements")]
    public TMP_InputField username;
    public TMP_InputField password;
    public Button loginButton;
    public Button registerButton;
    public TMP_Text statusText;

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

    public void Login()
    {
        if (username.text == "" || password.text == "")
        {
            ShowMessage("Vul alle velden in!", Color.red);
            return;
        }

        // Simuleer succesvolle login (hier kun je later serverauthenticatie toevoegen)
        IsLoggedIn = true;
        ShowMessage("Login succesvol!", Color.green);
        Debug.Log("Gebruiker is ingelogd!");

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


