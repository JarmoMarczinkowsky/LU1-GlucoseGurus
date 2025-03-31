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
    public PatientApiClient patientApiClient;

    private string route;
    private int mangoCount = 0;
    private TreatmentplanManagerScript treatmentplanManagerScript;

    async void Start()
    {
        mangoCount = 0;
        route = "A";

        // Top, maar hoe krijg ik nu de parentguardianId?
        // Of kan ik de route info smokkelen van Mathijs zijn werk?

        string ParentGuardian = "???";

        
        IWebRequestReponse webRequestResponse = await patientApiClient.ReadPatientsByParentGuardian(ParentGuardian);

        switch (webRequestResponse)
        {
            case WebRequestData<Patient> dataResponse:

                Patient patient = dataResponse.Data;
                route = patient.trajectId;

                break;
            case WebRequestError errorResponse:
                string errorMessage = errorResponse.ErrorMessage;

                Debug.Log("Read notes error: " + errorMessage);
                // TODO: Handle error scenario. Show the errormessage to the user.
                break;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
        }

        if (route == "A")
        {
            // Chose Route A
            RouteA.SetActive(true);
            RouteB.SetActive(false);

            treatmentplanManagerScript = RouteA.GetComponentInChildren<TreatmentplanManagerScript>();
            treatmentplanManagerScript.SetUp(route);
        }
        else if (route == "B")
        {
            // Chose Route B
            RouteA.SetActive(false);
            RouteB.SetActive(true);

            treatmentplanManagerScript = RouteB.GetComponentInChildren<TreatmentplanManagerScript>();
            treatmentplanManagerScript.SetUp(route);
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


