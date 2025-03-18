using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PageManagerScript : MonoBehaviour
{
    public GameObject Uitklapmenu;
    //public GameObject PopUpMenuManager;
    public List<GameObject> PopUpMenus;

    void Start()
    {
        Uitklapmenu.SetActive(false);

        foreach(var menu in PopUpMenus)
        {
            menu.gameObject.SetActive(false);
        }
    }

    public void ToggleMenu()
    {

        if(Uitklapmenu.activeSelf == false)
        {
            Uitklapmenu.SetActive(true);
        }
        else
        {
            Uitklapmenu.SetActive(false);
        }
    }

    public void SwitchScene(string scenename)
    {
        Debug.Log("Gekozen voor: " + scenename);
        //SceneManager.LoadScene(scenename);
    }

    public void ShowPopUpMenu(int popUpIndex)
    {
        PopUpMenus[popUpIndex].gameObject.SetActive(true);
    }

    public void ClosePopUpMenu(int popUpIndex)
    {
        PopUpMenus[popUpIndex].gameObject.SetActive(false);
    }

}
