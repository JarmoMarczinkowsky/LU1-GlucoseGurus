using System.Collections.Generic;
//using System.Drawing;
using NUnit.Framework;
using TMPro;

//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TreatmentplanManagerScript : MonoBehaviour
{
    [Header("Single Objects")]
    public Slider slider;
    public TextMeshProUGUI StepCounter;

    //public GameObject PopUpMenuManager;
    [Header("Lists")]
    public List<GameObject> PopUpMenus;
    public List<Button> TreatmentplanMoments;

    //private variables
    private int treatmentStep;
    private int operationStep;

    void Start()
    {


        foreach (var menu in PopUpMenus)
        {
            menu.gameObject.SetActive(false);
        }


        treatmentStep = (int)slider.value;
        StepCounter.text = "Stap " + treatmentStep + " voltooid";

        SetColor();

        operationStep = 3;

        if(treatmentStep == operationStep)
        {
            // Show recovery reminders
        }

    }

    #region TreatmentMoments
    public void SetColor()
    {
        treatmentStep = (int)slider.value;
        StepCounter.text = "Stap " + treatmentStep + " voltooid";

        foreach (var button in TreatmentplanMoments)
        {
            Color color;
            ColorUtility.TryParseHtmlString("#225D8A", out color);
            button.image.color = color;
        }

        if (treatmentStep >= 0 && treatmentStep <= TreatmentplanMoments.Count)
        {
            for (int i = 0; i < treatmentStep; i++)
            {
                var button = TreatmentplanMoments[i];

                Color color; 
                ColorUtility.TryParseHtmlString("#83A9C8", out color);
                button.image.color = color;
            }

            if (treatmentStep != TreatmentplanMoments.Count)
            {
                Color color;
                ColorUtility.TryParseHtmlString("#AACF77", out color);
                TreatmentplanMoments[treatmentStep].image.color = color;
            }
        }
    }
    #endregion TreatmentMoments

    #region EducationalContent

    public void ShowPopUpMenu(int popUpIndex)
    {
        PopUpMenus[popUpIndex].gameObject.SetActive(true);
    }

    public void ClosePopUpMenu(int popUpIndex)
    {
        PopUpMenus[popUpIndex].gameObject.SetActive(false);
    }
    #endregion EducationalContent

}
