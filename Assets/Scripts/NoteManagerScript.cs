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
    public Button btnCreate; 

    private bool isTabPressed = false;
    private int tabIndex = 0;

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
            switch (tabIndex)
            {
                //case 0:
                //    lst_InputFields[tabIndex].Select();
                //    break;
                case 1:
                    lst_InputFields[tabIndex].Select();
                    break;
                case 2:
                    btnCreate.Select();
                    break;
                default:
                    tabIndex = 0;
                    lst_InputFields[tabIndex].Select();
                    break;
            };

            tabIndex++;

        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            isTabPressed = false;
        }
    }

    //public void HoverEnterCreateButton()
    //{
         
    //}

    //public void HoverExitCreateButton()
    //{

    //}

    public void ChangeTabIndexValue(int index)
    {
        tabIndex = index;
    }
}
