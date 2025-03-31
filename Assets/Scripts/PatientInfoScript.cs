using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.Collections.Generic;

public class PatientInfoScript : MonoBehaviour
{
    public static PatientInfoScript Instance;

    private Patient patient1;

    [Header("UI Elements")]
    public TMP_InputField FirstNameInput;
    public TMP_InputField LastNameInput;
    public TMP_InputField TrajectInput;
    public TMP_InputField DoctorInput;
    public Button SaveInfoButton;
    public TMP_Text statusText;

    [Header("Dependencies")]
    public PatientApiClient patientApiClient;

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

    public async void SavePatientInfo()
    {
        if (string.IsNullOrEmpty(FirstNameInput.text) ||
            string.IsNullOrEmpty(LastNameInput.text) ||
            string.IsNullOrEmpty(TrajectInput.text) ||
            string.IsNullOrEmpty(DoctorInput.text))
        {
            ShowMessage("Vul alle velden in!", Color.red);
            return;
        }


        // Maak een patiëntobject
        Patient patient = new Patient
        {
            id = System.Guid.NewGuid().ToString(), // Nieuwe ID voor een nieuwe patiënt
            firstName = FirstNameInput.text,
            lastName = LastNameInput.text,
            trajectId = TrajectInput.text,
            doctorId = DoctorInput.text
        };

        // Verstuur patiëntgegevens naar de API
        IWebRequestReponse response = await patientApiClient.CreatePatient(patient);

        switch (response)
        {
            case WebRequestData<Patient> dataResponse:
                ShowMessage("Patiëntgegevens opgeslagen!", Color.green);
                Debug.Log("Patiëntgegevens succesvol verzonden!");
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

    public async void LoadPatientInfo()
    {
        //string parentGuardianId = ParentGuardianInput.text;

        //if (string.IsNullOrEmpty(parentGuardianId))
        //{
        //    ShowMessage("Voer een ouder/verzorger ID in!", Color.red);
        //    return;
        //}


        // Let op, patient is nog leeg, dit werkt dus nog NIET
        IWebRequestReponse response = await patientApiClient.ReadPatientsByParentGuardian(patient1.parentGuardianId);

        switch (response)
        {
            case WebRequestData<List<Patient>> dataResponse:
                if (dataResponse.Data.Count > 0)
                {
                    Patient patient = dataResponse.Data[0]; // Neem de eerste patiënt uit de lijst
                    FirstNameInput.text = patient.firstName;
                    LastNameInput.text = patient.lastName;
                    TrajectInput.text = patient.trajectId;
                    DoctorInput.text = patient.doctorId;
                    ShowMessage("Patiëntgegevens geladen!", Color.green);
                }
                else
                {
                    ShowMessage("Geen patiëntgegevens gevonden!", Color.yellow);
                }
                break;
            case WebRequestError errorResponse:
                ShowMessage("Fout bij laden: " + errorResponse.ErrorMessage, Color.red);
                Debug.LogError("Fout bij laden: " + errorResponse.ErrorMessage);
                break;
            default:
                Debug.LogError("Onbekende respons ontvangen");
                break;
        }
    }

    public async void UpdatePatientInfo()
    {
        if (string.IsNullOrEmpty(FirstNameInput.text) ||
            string.IsNullOrEmpty(LastNameInput.text) ||
            string.IsNullOrEmpty(TrajectInput.text) ||
            string.IsNullOrEmpty(DoctorInput.text))
        {
            ShowMessage("Vul alle velden in!", Color.red);
            return;
        }

        Patient patient = new Patient
        {
            id = System.Guid.NewGuid().ToString(), // In een echt scenario moet dit de juiste ID zijn!
            firstName = FirstNameInput.text,
            lastName = LastNameInput.text,
            //trajectId = TrajectInput.text,
            doctorId = DoctorInput.text
        };

        if (TrajectInput.text != "A" || TrajectInput.text != "B")
        {
            // Show error
        }
        else
        {

            IWebRequestReponse response = await patientApiClient.UpdatePatient(patient);

            switch (response)
            {
                case WebRequestData<Patient> dataResponse:
                    ShowMessage("Patiëntgegevens bijgewerkt!", Color.green);
                    Debug.Log("Patiëntgegevens succesvol geüpdatet!");
                    break;
                case WebRequestError errorResponse:
                    ShowMessage("Fout bij updaten: " + errorResponse.ErrorMessage, Color.red);
                    Debug.LogError("Fout bij updaten: " + errorResponse.ErrorMessage);
                    break;
                default:
                    Debug.LogError("Onbekende respons ontvangen");
                    break;
            }
        }
    }

    public async void DeletePatientInfo()
    {
        string patientId = System.Guid.NewGuid().ToString(); // In een echt scenario moet dit de juiste ID zijn!

        if (string.IsNullOrEmpty(patientId))
        {
            ShowMessage("Vul ouder/verzorger ID en patiënt ID in!", Color.red);
            return;
        }

        IWebRequestReponse response = await patientApiClient.DeletePatient(patient1.parentGuardianId, patientId);

        switch (response)
        {
            case WebRequestData<string> dataResponse:
                ShowMessage("Patiënt verwijderd!", Color.green);
                Debug.Log("Patiënt succesvol verwijderd!");
                break;
            case WebRequestError errorResponse:
                ShowMessage("Fout bij verwijderen: " + errorResponse.ErrorMessage, Color.red);
                Debug.LogError("Fout bij verwijderen: " + errorResponse.ErrorMessage);
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
