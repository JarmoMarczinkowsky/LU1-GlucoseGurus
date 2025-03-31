using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.Collections.Generic;

public class ParentGuardianInfoScript : MonoBehaviour
{
    public static ParentGuardianInfoScript Instance;

    [Header("UI Elements")]
    public TMP_InputField FirstNameInput;
    public TMP_InputField LastNameInput;
    public Button SaveInfoButton;
    public TMP_Text statusText;

    [Header("Dependencies")]
    public ParentGuardianApiClient parentGuardianApiClient;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public async void SaveParentGuardianInfo()
    {
        if (string.IsNullOrEmpty(FirstNameInput.text) || string.IsNullOrEmpty(LastNameInput.text))
        {
            ShowMessage("Vul alle velden in!", Color.red);
            return;
        }

        ParentGuardian parentGuardian = new ParentGuardian
        {
            id = System.Guid.NewGuid().ToString(),
            FirstName = FirstNameInput.text,
            LastName = LastNameInput.text
        };

        IWebRequestReponse response = await parentGuardianApiClient.CreateParentGuardian(parentGuardian);

        switch (response)
        {
            case WebRequestData<ParentGuardian> dataResponse:
                ShowMessage("Oudergegevens opgeslagen!", Color.green);
                Debug.Log("Oudergegevens succesvol verzonden!");
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