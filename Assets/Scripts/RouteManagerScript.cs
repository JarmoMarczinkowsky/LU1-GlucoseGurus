using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using TMPro;


public class RouteManagerScript : MonoBehaviour
{
    [Header("Routes")]
    public GameObject RouteA;
    public GameObject RouteB;

    [Header("Baskets")]
    public GameObject theBasket;
    public List<Sprite> BasketSprites;

    [Header("Dependencies")]
    private ApiClientHolder ApiClientHolder;
    private PatientApiClient patientApiClient;
    private TrajectApiClient trajectApiClient;

    private Patient patient;
    private string route;
    private int mangoCount = 0;

    async void Start()
    {
        ApiClientHolder = ApiClientHolder.instance;
        patientApiClient = ApiClientHolder.patientApiClient;
        trajectApiClient = ApiClientHolder.trajectApiClient;

        mangoCount = 0;

        //route = "A";
        //string ParentGuardian = ApiClientHolder.ParentGuardianId;        
        //string ParentGuardian = "3F2504E0-4F89-11D3-9A0C-0305E82C3301";

        // Voor wanneer de patientgegevens nog onbekend zijn
        if (ApiClientHolder.Patient == null)
        {
            IWebRequestReponse webRequestResponse = await patientApiClient.ReadPatientsByParentGuardian(ApiClientHolder.ParentGuardianId);

            switch (webRequestResponse)
            {
                case WebRequestData<List<Patient>> dataResponse:

                    foreach(Patient _patient in dataResponse.Data)
                    {
                        patient = _patient;
                        ApiClientHolder.Patient = patient;

                    }
                    break;
                case WebRequestError errorResponse:
                    string errorMessage = errorResponse.ErrorMessage;

                    Debug.Log("Read patient error: " + errorMessage);

                    // TODO: Handle error scenario. Show the errormessage to the user.
                    break;
                default:
                    throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
            }
        }
        // Voor wanneer de patient gegevens bekend zijn
        else
        {
            patient = ApiClientHolder.Patient;
        }

        // Als de route nog onbekend is
        if (route == null)
        {
            IWebRequestReponse webRequestResponse2 = await trajectApiClient.ReadTrajectById(patient.trajectId);

            switch (webRequestResponse2)
            {
                case WebRequestData<Traject> dataResponse2:

                    Traject traject = dataResponse2.Data;
                    route = traject.name[0].ToString();
                     ApiClientHolder.Route = route;

                    break;
                case WebRequestError errorResponse2:
                    break;
                default:
                    throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse2.GetType());
            }
        }
        // Voor wanneer de route bekend is
        else
        {
            route = ApiClientHolder.Route;
        }


        if (route == "A")
        {
            // Chose Route A
            RouteA.SetActive(true);
            RouteB.SetActive(false);

            TreatmentplanManagerScript treatmentplanManagerScript = RouteA.GetComponentInChildren<TreatmentplanManagerScript>();
            treatmentplanManagerScript.SetUp("A");
        }
        else if (route == "B")
        {
            // Chose Route B
            RouteA.SetActive(false);
            RouteB.SetActive(true);

            TreatmentplanManagerScript treatmentplanManagerScript = RouteB.GetComponentInChildren<TreatmentplanManagerScript>();
            treatmentplanManagerScript.SetUp("B");
        }
    }

    public void SetBasket()
    {
        mangoCount++;

        if (mangoCount <= BasketSprites.Count)
        {
            theBasket.GetComponent<Image>().sprite = BasketSprites[mangoCount];
        }
    }
}


