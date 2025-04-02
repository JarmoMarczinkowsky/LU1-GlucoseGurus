using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ParentGuardianInfoScript : MonoBehaviour
{
    public static ParentGuardianInfoScript Instance;

    [Header("UI Elements")]
    public TMP_InputField FirstNameInput;
    public TMP_InputField LastNameInput;
    public Button SaveInfoButton;
    public TMP_Text statusText;

    [Header("Dependencies")]
    private ParentGuardianApiClient parentGuardianApiClient;
    private ApiClientHolder apiClientHolder;


    public void Start()
    {
        apiClientHolder = ApiClientHolder.instance;
        parentGuardianApiClient = apiClientHolder.parentGuardianApiClient;
    }

    public async void SaveParentGuardianInfo()
    {
        if (string.IsNullOrEmpty(FirstNameInput.text) || string.IsNullOrEmpty(LastNameInput.text))
        {
            ShowMessage("Vul alle velden in!", Color.red);
            return;
        }

        ApiClientHolder.ParentGuardianId = System.Guid.NewGuid().ToString();

        ParentGuardian parentGuardian = new ParentGuardian
        {
            id = ApiClientHolder.ParentGuardianId,
            FirstName = FirstNameInput.text,
            LastName = LastNameInput.text
        };

        IWebRequestReponse response = await parentGuardianApiClient.CreateParentGuardian(parentGuardian);

        switch (response)
        {
            case WebRequestData<ParentGuardian> dataResponse:
                ShowMessage("Oudergegevens opgeslagen!", Color.green);
                Debug.Log("Oudergegevens succesvol verzonden!");

                SceneManager.LoadScene("PatientInfoInputPage 1");


                break;
            case WebRequestError errorResponse:
                ShowMessage("Fout bij opslaan: " + errorResponse.ErrorMessage, Color.red);
                Debug.LogError("Fout bij opslaan: " + errorResponse.ErrorMessage);
                break;
            default:
                Debug.LogError("Onbekende respons ontvangen");
                break;
        }
    }

    private void ShowMessage(string message, Color color)
    {
        if (statusText != null)
        {
            statusText.text = message;
            statusText.color = color;
        }
    }

}