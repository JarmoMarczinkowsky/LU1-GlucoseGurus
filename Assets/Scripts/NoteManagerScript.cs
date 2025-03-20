using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NoteManagerScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("Input Fields")]
    public List<TMP_InputField> lst_InputFields;
    public Button createButton; 

    private bool isTabPressed = false;

    //Color palette:
    //https://coolors.co/8bc348-f5c523-fe5377-0b3954-bfd7ea
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SelectOtherInputField();
    }

    private void SelectOtherInputField()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !isTabPressed)
        {
            isTabPressed = true;

            //if tab is pressed, select the next input field, if the last input field is selected, select the button, if the button is selected, select the first input field
            for (int i = 0; i < lst_InputFields.Count; i++)
            {
                if (lst_InputFields[i].isFocused)
                {
                    if (i == lst_InputFields.Count - 1)
                    {
                        createButton.Select();
                        break;
                    }
                    else
                    {
                        lst_InputFields[i + 1].Select();
                        break;
                    }
                }
                else if (createButton.gameObject == UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject)
                {
                    lst_InputFields[0].Select();
                    break;
                }
            }

        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            isTabPressed = false;
        }
    }

    public void HoverEnterCreateButton()
    {
         
    }

    public void HoverExitCreateButton()
    {

    }
}
