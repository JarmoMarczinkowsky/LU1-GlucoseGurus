using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.PackageManager.Requests;
using UnityEngine.SceneManagement;
using System.Net;
using System.Xml.Linq;
using UnityEngine.Networking;
using System.Threading.Tasks;
public class LoginManagerScript : MonoBehaviour
{ 
    [Header("Ui elements")]
    public TMP_InputField username;
    public TMP_InputField password;
    public Button loginButton;
    public Button registerButton;

    public void LoadNextScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

}
