using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;

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
    public List<GameObject> RecoverReminders;

    [Header("Sprites")]
    public Sprite Mango;
    public Sprite completedMango;


    [Header("Dependencies")]
    private ApiClientHolder ApiClientHolder;
    private TrajectApiClient trajectApiClient;
    private TrajectCareMomentClient trajectCareMomentClient;
    private CareMomentApiClient careMomentApiClient;
    private PatientApiClient patientApiClient;

    //private variables
    private List<Button> CompletedTreatmentplanMoments = new List<Button>();
    private int treatmentStep;
    private int operationStep;
    private int operationStep2;
    private string route;

    private List<TrajectCareMoment> TrajectCareMoments = new List<TrajectCareMoment>();

    public async void SetUp(string _route)
    {
        ApiClientHolder = ApiClientHolder.instance;

        trajectApiClient = ApiClientHolder.trajectApiClient;
        trajectCareMomentClient = ApiClientHolder.trajectCareMomentClient;
        careMomentApiClient = ApiClientHolder.careMomentApiClient;
        patientApiClient = ApiClientHolder.patientApiClient;

        route = _route;


        // Turn all popup menus off, just incase
        foreach (var menu in PopUpMenus)
        {
            menu.gameObject.SetActive(false);
        }

        if(route == "A")
        {
            // Route A
            operationStep = 5;
            operationStep2 = 0;
        }
        if (route == "B")
        {
            // Route B
            operationStep = 2;
            operationStep2 = 6;
        }

        IWebRequestReponse webRequestResponse = await trajectCareMomentClient.ReadTrajectCareMoments();

        switch (webRequestResponse)
        {
            case WebRequestData<List<TrajectCareMoment>> dataResponse:
                List<TrajectCareMoment> _trajectCareMoments = dataResponse.Data;

                _trajectCareMoments.ForEach(trajectCareMoment => TrajectCareMoments.Add(trajectCareMoment));

                //Debug.Log("List of traject caremoments: ");
                //_trajectCareMoments.ForEach(trajectCareMoment => Debug.Log(trajectCareMoment.name));
                // TODO: Handle succes scenario.

                break;
            case WebRequestError errorResponse:
                string errorMessage = errorResponse.ErrorMessage;
                Debug.Log("Read notes error: " + errorMessage);

                // TODO: Handle error scenario. Show the errormessage to the user.

                break;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
        }

        foreach (TrajectCareMoment trajectCareMoment in TrajectCareMoments)
        {
            if(trajectCareMoment.isCompleted == true)
            {
                // Completes any of the trajectcaremoments if they're done

                // +/- 1 bij de step
                CompletedTreatmentplanMoments.Add(TreatmentplanMoments[trajectCareMoment.step]);
                //Debug.Log("Completed Step: " + trajectCareMoment.step);

                // Then increases the number of mango's in the basket by one
                RouteManagerScript routeManager = FindFirstObjectByType<RouteManagerScript>();
                routeManager.SetBasket();

                // Lastly, progresses the application to the next step of the program
                treatmentStep++;
            }
        }

        // Then I need to add the description and videos/fotos that fit the context/topic
        // Currently not neccesary 

        // Then we need to update the visuals, depending on where the user left off
        SetMango();


        // lastly we need to show any of the reminders when it's neccesary
        if (treatmentStep == operationStep)
        {
            // Show recovery reminders
            // recoveryPopup.gameObject.SetActive(true);
            // Or something in this direction
        }
        else if(treatmentStep == operationStep2 && operationStep2 !=0)
        {
            // recoveryPopup.gameObject.SetActive(true);
            // Or something in this direction
        }
    }


    #region TreatmentMoments
    public void SetMango()
    {
        // all buttons are set to "not yet available"
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

        //// some buttons are set to "available"
        //// might have to delete this part
        //if (treatmentStep >= 0 && treatmentStep <= TreatmentplanMoments.Count)
        //{

        //    for (int i = 0; i < treatmentStep; i++)
        //    {
        //        var button = TreatmentplanMoments[i];

        //        Color color;
        //        ColorUtility.TryParseHtmlString("#F5F5F5", out color);
        //        button.image.color = color;

        //        RectTransform rectTransform = button.GetComponent<RectTransform>();
        //        Vector2 newSize = new Vector2(200f, 200f);
        //        rectTransform.sizeDelta = newSize;
        //    }
        //}

        // some buttons are set to "completed"
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

    public async void CompleteTreatmentInfo(int index)
    {
        // Adds the completed steps to a seperate list, aslong as it wasn't added to that list before
        // Watch out for the "<" sign, it might have to be "<="
        if(index < treatmentStep && !CompletedTreatmentplanMoments.Contains(TreatmentplanMoments[index]))
        {
            // update the trajectCaremoment
            TrajectCareMoments[index].isCompleted = true;
            IWebRequestReponse webRequestResponse = await trajectCareMomentClient.UpdateTrajectCareMoment(TrajectCareMoments[index]);

            switch (webRequestResponse)
            {
                case WebRequestData<TrajectCareMoment> dataResponse:

                    // TODO: Handle succes scenario.

                    CompletedTreatmentplanMoments.Add(TreatmentplanMoments[index]);

                    SetMango();
                    RouteManagerScript routeManager = FindFirstObjectByType<RouteManagerScript>();
                    routeManager.SetBasket();

                    break;
                case WebRequestError errorResponse:
                    string errorMessage = errorResponse.ErrorMessage;
                    Debug.Log("Read notes error: " + errorMessage);

                    // TODO: Handle error scenario. Show the errormessage to the user.

                    break;
                default:
                    throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
            }            
        }

        ClosePopUpMenu(index);
    }


    #endregion TreatmentMoments

    #region EducationalContent

    // Small function to turn the popup gameobject on
    public void ShowPopUpMenu(int popUpIndex)
    {
        PopUpMenus[popUpIndex].gameObject.SetActive(true);
    }

    // Small function to turn the popup gameobject off
    public void ClosePopUpMenu(int popUpIndex)
    {
        PopUpMenus[popUpIndex].gameObject.SetActive(false);
    }
    #endregion EducationalContent

    #region Update
    public void Update()
    {
        // Closes all the menus when the user presses escape
        if (Input.GetKey(KeyCode.Escape))
        {
            foreach (var menu in PopUpMenus)
            {
                menu.gameObject.SetActive(false);
            }

            // another loop for more menus
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
