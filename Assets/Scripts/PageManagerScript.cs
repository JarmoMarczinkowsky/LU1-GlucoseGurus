using System.Collections.Generic;
//using System.Drawing;
using NUnit.Framework;
using TMPro;

//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PageManagerScript : MonoBehaviour
{
    [Header("Single Objects")]
    public GameObject DropdownMenu;

    void Start()
    {
        DropdownMenu.SetActive(false);

    }

    #region Navigation

    public void ToggleMenu()
    {

        if (DropdownMenu.activeSelf == false)
        {
            DropdownMenu.SetActive(true);
        }
        else
        {
            DropdownMenu.SetActive(false);
        }
    }

    ////public void SwitchScene(string scenename)
    //{
    //    Debug.Log("Gekozen voor: " + scenename);
    //    //SceneManager.LoadScene(scenename);
    //}

    public void SwitchScene(string sceneName)
    {
        if (LoginManagerScript.Instance != null && LoginManagerScript.Instance.IsLoggedIn)
        {
            Debug.Log("Scène wisselen naar: " + sceneName);
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning("Je moet eerst inloggen voordat je van scene kunt wisselen!");
        }
    }

    public void LoadRegister()
    {
        SceneManager.LoadScene("RegisterScreenPage");
    }

    public void LoadLogin()
    {
        SceneManager.LoadScene("LoginScreenPage");
    }

    public void LoadTreatment()
    {
        SwitchScene("TreatmentPlanPage");
    }

    public void LoadWelcomeKids()
    {
        SwitchScene("WelcomeChildrenPage");
    }

    public void LoadWelcomeParents()
    {
        SwitchScene("WelcomeParentsPage");
    }
    #endregion Navigation

}
