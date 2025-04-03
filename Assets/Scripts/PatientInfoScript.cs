using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.Collections.Generic;
using Unity.VisualScripting;
using static PatientInfoScript;
using UnityEngine.SceneManagement;

public class PatientInfoScript : MonoBehaviour
{
    public static PatientInfoScript Instance;

    private Patient patient1;

    [Header("UI Elements")]
    public TMP_InputField FirstNameInput;
    public TMP_InputField LastNameInput;
    public TMP_InputField DoctorInput;
    public Button SaveInfoButton;
    public TMP_Text statusText;

    [Header("Dependencies")]
    private PatientApiClient patientApiClient;
    private ApiClientHolder ApiClientHolder;
    private TrajectApiClient trajectApiClient;
    private ParentGuardianApiClient parentGuardianApiClient;
    private CareMomentApiClient careMomentApiClient;
    private TrajectCareMomentClient trajectCareMomentClient;

    private string parentGuardianId;
    private List<CareMoment> careMomentList;
    private int _step = 0;




    public void Start()
    {
        ApiClientHolder = ApiClientHolder.instance;
        patientApiClient = ApiClientHolder.patientApiClient;
        trajectApiClient = ApiClientHolder.trajectApiClient;
        parentGuardianApiClient = ApiClientHolder.parentGuardianApiClient;
        careMomentApiClient = ApiClientHolder.careMomentApiClient;
        trajectCareMomentClient = ApiClientHolder.trajectCareMomentClient;

    }

    public async void SavePatientInfo()
    {
        if (string.IsNullOrEmpty(FirstNameInput.text) ||
            string.IsNullOrEmpty(LastNameInput.text) ||
            string.IsNullOrEmpty(DoctorInput.text))
        {
            ShowMessage("Vul alle velden in!", Color.red);
            return;
        }

        // Create the new Traject

        Traject traject = new Traject
        {
            id = System.Guid.NewGuid().ToString(),
            name = ApiClientHolder.Route + "-" + FirstNameInput.text + LastNameInput.text
        };

        IWebRequestReponse response1 = await trajectApiClient.CreateTraject(traject);

        switch (response1)
        {
            case WebRequestData<Traject> dataResponse:
                ShowMessage("Traject aangemaakt!", Color.green);
                Debug.Log("Traject succesvol aangemaakt!");
                break;
            case WebRequestError errorResponse:
                ShowMessage("Fout bij opslaan: " + errorResponse.ErrorMessage, Color.red);
                Debug.LogError("Fout bij opslaan: " + errorResponse.ErrorMessage);
                break;
            default:
                Debug.LogError("Onbekende respons ontvangen");
                break;
        }



        // This is were we collect all the CareMoments, and add them to a List

        IWebRequestReponse response4 = await careMomentApiClient.ReadCareMoments();

        switch (response4)
        {
            case WebRequestData<List<CareMoment>> dataResponse4:

                List<CareMoment> CareMoments = dataResponse4.Data;

                foreach(CareMoment careMoment1 in CareMoments)
                {
                    //Debug.Log("Caremoment: " + careMoment1);
                }

                careMomentList = new List<CareMoment>();

                // Route A
                if (ApiClientHolder.Route == "A")
                {
                    foreach (CareMoment careMoment in CareMoments)

                    if (careMoment.name[0] == 'A')
                    {
                        careMomentList.Add(careMoment);
                    }
                }

                // Route B
                else if(ApiClientHolder.Route == "B")
                {
                    foreach (CareMoment careMoment in CareMoments)
                    {
                        if (careMoment.name[0] == 'B')
                        {
                            careMomentList.Add(careMoment);
                        }
                    }
                }

                //foreach (CareMoment careMoment1 in careMomentList)
                //{
                //    Debug.Log("Caremoment: " + careMoment1);
                //}

                break;
            case WebRequestError errorResponse4:
                Debug.LogError("Fout bij opslaan: " + errorResponse4.ErrorMessage);
                break;
            default:
                Debug.LogError("Onbekende respons ontvangen");
                break;
        }

        // When we collected all the careMoments we add them to TrajectCareMoment 'Koppeltabel'

        foreach(var careMoment in careMomentList)
        {

            TrajectCareMoment trajectCareMoment = new TrajectCareMoment 
            {
                trajectId = traject.id,
                CareMomentId = careMoment.id,
                name = careMoment.name,
                step = _step,

            };
            _step++;

            IWebRequestReponse response5 = await trajectCareMomentClient.CreateTrajectCareMoment(trajectCareMoment);


            switch (response5)
            {
                case WebRequestData<TrajectCareMoment> dataResponse5:
                    break;
                case WebRequestError errorResponse5:
                    Debug.LogError("Fout bij opslaan: " + errorResponse5.ErrorMessage);
                    break;
                default:
                    Debug.LogError("Onbekende respons ontvangen");
                    break;
            }
        }


        if (ApiClientHolder.ParentGuardianId == null)
        {

            IWebRequestReponse response3 = await parentGuardianApiClient.ReadParentGuardians();

            switch (response3)
            {
                case WebRequestData<List<ParentGuardian>> dataResponse3:

                    parentGuardianId = dataResponse3.Data[0].id;
                    ApiClientHolder.ParentGuardianId = parentGuardianId;

                    break;
                case WebRequestError errorResponse3:
                    Debug.LogError("Fout bij opslaan: " + errorResponse3.ErrorMessage);
                    break;
                default:
                    Debug.LogError("Onbekende respons ontvangen");
                    break;
            }
        }
        else
        {
            parentGuardianId = ApiClientHolder.ParentGuardianId;
        }

        // There is currently only one doctor, Henk De Groot (create him in the database)
        // doctorId = 022f4e4f-d4b7-4b41-bbd7-17255819aef8
        Patient patient = new Patient
        {
            id = System.Guid.NewGuid().ToString(),
            firstName = FirstNameInput.text,
            lastName = LastNameInput.text,
            trajectId = traject.id,
            parentGuardianId = parentGuardianId,
            //doctorId = DoctorInput.text
            doctorId = "022f4e4f-d4b7-4b41-bbd7-17255819aef8"
        };

        IWebRequestReponse response2 = await patientApiClient.CreatePatient(patient);

        switch (response2)
        {
            case WebRequestData<Patient> dataResponse:

                ApiClientHolder.Patient = dataResponse.Data;

                ShowMessage("Patiëntgegevens opgeslagen!", Color.green);
                Debug.Log("Patiëntgegevens succesvol verzonden!");

                SceneManager.LoadScene("TreatmentplanPage");
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

    //public async void LoadPatientInfo()
    //{
    //    //string parentGuardianId = ParentGuardianInput.text;

    //    //if (string.IsNullOrEmpty(parentGuardianId))
    //    //{
    //    //    ShowMessage("Voer een ouder/verzorger ID in!", Color.red);
    //    //    return;
    //    //}


    //    // Let op, patient is nog leeg, dit werkt dus nog NIET
    //    IWebRequestReponse response = await patientApiClient.ReadPatientsByParentGuardian(patient1.parentGuardianId);

    //    switch (response)
    //    {
    //        case WebRequestData<List<Patient>> dataResponse:
    //            if (dataResponse.Data.Count > 0)
    //            {
    //                Patient patient = dataResponse.Data[0]; // Neem de eerste patiënt uit de lijst
    //                FirstNameInput.text = patient.firstName;
    //                LastNameInput.text = patient.lastName;
    //                TrajectInput.text = patient.trajectId;
    //                DoctorInput.text = patient.doctorId;
    //                ShowMessage("Patiëntgegevens geladen!", Color.green);
    //            }
    //            else
    //            {
    //                ShowMessage("Geen patiëntgegevens gevonden!", Color.yellow);
    //            }
    //            break;
    //        case WebRequestError errorResponse:
    //            ShowMessage("Fout bij laden: " + errorResponse.ErrorMessage, Color.red);
    //            Debug.LogError("Fout bij laden: " + errorResponse.ErrorMessage);
    //            break;
    //        default:
    //            Debug.LogError("Onbekende respons ontvangen");
    //            break;
    //    }
    //}

    //public async void UpdatePatientInfo()
    //{
    //    if (string.IsNullOrEmpty(FirstNameInput.text) ||
    //        string.IsNullOrEmpty(LastNameInput.text) ||
    //        string.IsNullOrEmpty(TrajectInput.text) ||
    //        string.IsNullOrEmpty(DoctorInput.text))
    //    {
    //        ShowMessage("Vul alle velden in!", Color.red);
    //        return;
    //    }

    //    Patient patient = new Patient
    //    {
    //        id = System.Guid.NewGuid().ToString(), // In een echt scenario moet dit de juiste ID zijn!
    //        firstName = FirstNameInput.text,
    //        lastName = LastNameInput.text,
    //        //trajectId = TrajectInput.text,
    //        doctorId = DoctorInput.text
    //    };

    //    if (TrajectInput.text != "A" || TrajectInput.text != "B")
    //    {
    //        // Show error
    //    }
    //    else
    //    {

    //        IWebRequestReponse response = await patientApiClient.UpdatePatient(patient);

    //        switch (response)
    //        {
    //            case WebRequestData<Patient> dataResponse:
    //                ShowMessage("Patiëntgegevens bijgewerkt!", Color.green);
    //                Debug.Log("Patiëntgegevens succesvol geüpdatet!");
    //                break;
    //            case WebRequestError errorResponse:
    //                ShowMessage("Fout bij updaten: " + errorResponse.ErrorMessage, Color.red);
    //                Debug.LogError("Fout bij updaten: " + errorResponse.ErrorMessage);
    //                break;
    //            default:
    //                Debug.LogError("Onbekende respons ontvangen");
    //                break;
    //        }
    //    }
    //}

    //public async void DeletePatientInfo()
    //{
    //    string patientId = System.Guid.NewGuid().ToString(); // In een echt scenario moet dit de juiste ID zijn!

    //    if (string.IsNullOrEmpty(patientId))
    //    {
    //        ShowMessage("Vul ouder/verzorger ID en patiënt ID in!", Color.red);
    //        return;
    //    }

    //    IWebRequestReponse response = await patientApiClient.DeletePatient(patient1.parentGuardianId, patientId);

    //    switch (response)
    //    {
    //        case WebRequestData<string> dataResponse:
    //            ShowMessage("Patiënt verwijderd!", Color.green);
    //            Debug.Log("Patiënt succesvol verwijderd!");
    //            break;
    //        case WebRequestError errorResponse:
    //            ShowMessage("Fout bij verwijderen: " + errorResponse.ErrorMessage, Color.red);
    //            Debug.LogError("Fout bij verwijderen: " + errorResponse.ErrorMessage);
    //            break;
    //        default:
    //            Debug.LogError("Onbekende respons ontvangen");
    //            break;
    //    }
    //}

}
