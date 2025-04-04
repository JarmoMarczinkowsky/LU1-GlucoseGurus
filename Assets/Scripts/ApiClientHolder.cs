using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
//using TMPro;
//using UnityEngine.UI;
//using System.Threading.Tasks;
//using System.Collections.Generic;
//using UnityEngine.SceneManagement;

public class ApiClientHolder : MonoBehaviour
{
    [Header("Dependencies")]
    public UserApiClient userApiClient;
    public NoteApiClient noteApiClient;
    public CareMomentApiClient careMomentApiClient;
    public TrajectCareMomentClient trajectCareMomentClient;
    public TrajectApiClient trajectApiClient;
    public DoctorApiClient doctorApiClient;
    public PatientApiClient patientApiClient;
    public ParentGuardianApiClient parentGuardianApiClient;

    [Header("ImportantInfo")]
    public static string ParentGuardianId;
    public static string Route;
    public static Patient Patient = new Patient();
    public static List<CareMoment> CareMoments = new List<CareMoment>();


    public static ApiClientHolder instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this);
        }
    }   
}
