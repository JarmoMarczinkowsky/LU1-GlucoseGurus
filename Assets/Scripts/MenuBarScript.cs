//using Microsoft.Unity.VisualStudio.Editor;
using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class MenuBarScript : MonoBehaviour
{
    [Header("Single Objects")]
    public GameObject DropdownMenu;
    public List<Sprite> ListAvatars;
    public Image chosenAvatar;

    private int avatarCount;

    public void Start()
    {
        if(ApiClientHolder.Patient != null)
        {
            avatarCount = ApiClientHolder.Patient.avatar;
        }
        else
        {
            avatarCount = 0;
        }

        if (DropdownMenu != null)
        {
            DropdownMenu.SetActive(false);
        }
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

    public void SwitchScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    #endregion Navigation


    public void Update()
    {
        if (ApiClientHolder.Patient != null)
        {
            if(ApiClientHolder.Patient.avatar != avatarCount)
            {
                avatarCount = ApiClientHolder.Patient.avatar;

                if (avatarCount <= ListAvatars.Count - 1)
                {
                    chosenAvatar.GetComponent<UnityEngine.UI.Image>().sprite = ListAvatars[avatarCount];
                }
            }
        }
    }
}
