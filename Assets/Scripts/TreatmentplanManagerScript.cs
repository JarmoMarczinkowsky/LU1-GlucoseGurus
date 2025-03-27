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

    //[Header("Single Objects")]
    //public Slider slider;
    //public TextMeshProUGUI StepCounter;
    ////public GameObject PopUpMenuManager;

    [Header("Lists")]
    public List<GameObject> PopUpMenus;
    public List<Button> TreatmentplanMoments;
    public Sprite Mango;
    public Sprite completedMango;

    private List<Button> CompletedTreatmentplanMoments = new List<Button>();
    //private variables
    private int treatmentStep;
    private int operationStep;
    private int route;

    public void SetUp()
    {
        foreach (var menu in PopUpMenus)
        {
            menu.gameObject.SetActive(false);
        }

        //CompleteTreatmentInfo(0);

        treatmentStep = 3;
        //treatmentStep = (int)slider.value;
        //StepCounter.text = "Stap " + treatmentStep + " voltooid";
        SetMango();

        operationStep = 3;

        if (treatmentStep == operationStep)
        {
            
            // Show recovery reminders
            // recoveryPopup.gameObject.SetActive(true);
            // Or something in this direction
        }
    }

    #region TreatmentMoments
    public void SetMango()
    {
        //treatmentStep = (int)slider.value;
        //StepCounter.text = "Gebruiker bij stap: " + treatmentStep;

        foreach (var button in TreatmentplanMoments)
        {
            //Color color;
            //ColorUtility.TryParseHtmlString("#969696", out color);
            //button.image.color = color;

            button.GetComponent<Image>().sprite = Mango;

            RectTransform rectTransform = button.GetComponent<RectTransform>();
            Vector2 newSize = new Vector2(130f, 130f);
            rectTransform.sizeDelta = newSize;


        }
        if (treatmentStep >= 0 && treatmentStep <= TreatmentplanMoments.Count)
        {
            //for (int i = 0; i < treatmentStep; i++)
            //{
            //    var button = TreatmentplanMoments[i];

            //    Color color;
            //    ColorUtility.TryParseHtmlString("#F5F5F5", out color);
            //    button.image.color = color;
            //}


            for (int i = 0; i < treatmentStep; i++)
            {
                var button = TreatmentplanMoments[i];

                Color color;
                ColorUtility.TryParseHtmlString("#F5F5F5", out color);
                button.image.color = color;

                RectTransform rectTransform = button.GetComponent<RectTransform>();
                Vector2 newSize = new Vector2(200f, 200f);
                rectTransform.sizeDelta = newSize;
            }
        }

        if (CompletedTreatmentplanMoments != null)
        {
            foreach (var button in CompletedTreatmentplanMoments)
            {
                RectTransform rectTransform = button.GetComponent<RectTransform>();
                Vector2 newSize = new Vector2(200f, 200f);
                rectTransform.sizeDelta = newSize;

                Color color;
                ColorUtility.TryParseHtmlString("#969696", out color);
                button.image.color = color;

                button.GetComponent<Image>().sprite = completedMango;
            }
        }
    }

    public void CompleteTreatmentInfo(int index)
    {
        if(index < treatmentStep && !CompletedTreatmentplanMoments.Contains(TreatmentplanMoments[index]))
        {
            CompletedTreatmentplanMoments.Add(TreatmentplanMoments[index]);
            Debug.Log("Completed Step: " + index);
            SetMango();


            RouteManagerScript routeManager = FindFirstObjectByType<RouteManagerScript>();
            routeManager.SetBasket();

        }

        ClosePopUpMenu(index);
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

    #region Update
    public void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            foreach (var menu in PopUpMenus)
            {
                menu.gameObject.SetActive(false);
            }
        }
        if(Input.GetKey(KeyCode.Space))
        {

            // Werkt niet, called nu btnCompletPopUp6 en zet de pop up in een rap tempo aan en uit
            // Het vinden van btnCompletPopUp6 is opzich wel logisch met deze code, maar dan heb ik nogsteeds hetzelfde probleem:
            // Welk menu wil ik voltooien?

            //Button button = FindFirstObjectByType<Button>();

            //if (button != null)
            //{
            //    Debug.Log(button.name);
            //    //button.GetComponent<Button>();
            //    button.onClick.Invoke();
            //}
        }
    }
    #endregion Update
}
