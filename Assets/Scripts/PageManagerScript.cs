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

    public void SwitchScene(string scenename)
    {
        Debug.Log("Gekozen voor: " + scenename);
        //SceneManager.LoadScene(scenename);
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
        SceneManager.LoadScene("TreatmentPlanPage");
    }

    public void LoadWelcomeKids()
    {
        SceneManager.LoadScene("WelcomeChildrenPage");
    }

    public void LoadWelcomeParents()
    {
        SceneManager.LoadScene("WelcomeParentsPage");
    }
    #endregion Navigation

}
