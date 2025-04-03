using Microsoft.Unity.VisualStudio.Editor;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
public class AvatarSelectorScript : MonoBehaviour
{
    public List<UnityEngine.UI.Image> ListAvatars;
    public GameObject previewImage;

    [Header("Dependencies")]
    private ApiClientHolder ApiClientHolder;
    private PatientApiClient patientApiClient;

    private int selectedImage = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ApiClientHolder = ApiClientHolder.instance;
        patientApiClient = ApiClientHolder.patientApiClient;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickImage(int imgSelected)
    {
        Debug.Log($"Clicked image: {imgSelected}");

        previewImage.GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 1);

        if (imgSelected <= ListAvatars.Count - 1)
        {
            previewImage.GetComponent<UnityEngine.UI.Image>().sprite = ListAvatars[imgSelected].GetComponent<UnityEngine.UI.Image>().sprite;

            this.selectedImage = imgSelected;

            //SaveResultToDatabase(imgSelected);
        }
    }

    private async void SaveResultToDatabase(int imgSelected)
    {
        ApiClientHolder.Patient.avatar = imgSelected;

        IWebRequestReponse webRequestResponse = await patientApiClient.UpdatePatient(ApiClientHolder.Patient);

        Debug.Log("webrequestresponse " + webRequestResponse);
        Debug.Log("type of response " + webRequestResponse.GetType());

        switch (webRequestResponse)
        {
            case WebRequestData<Patient> dataResponse:
                // TODO: Handle succes scenario.
                Debug.Log("Succes met het veranderen van avatar");

                break;
            case WebRequestError errorResponse:
                string errorMessage = errorResponse.ErrorMessage;
                Debug.Log("Update avatar error: " + errorMessage);
                // TODO: Handle error scenario. Show the errormessage to the user.
                break;
            default:
                throw new NotImplementedException("No implementation for webRequestResponse of class: " + webRequestResponse.GetType());
        }

    }

    public void SaveAvatar()
    {
        Debug.Log("Save avatar clicked");
        SaveResultToDatabase(selectedImage);
    }
}
