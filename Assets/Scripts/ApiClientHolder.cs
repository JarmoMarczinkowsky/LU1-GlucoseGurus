using UnityEngine;

public class ApiClientHolder : MonoBehaviour
{
    [Header("Dependencies")]
    public UserApiClient userApiClient;
    public NoteApiClient noteApiClient;
    public CareMomentApiClient careMomentApiClient;
    public TrajectCareMomentClient tajectCareMomentClient;
    public TrajectApiClient trajectApiClient;
    public DoctorApiClient doctorApiClient;
    public PatientApiClient patientApiClient;
    public ParentGuardianApiClient parentGuardianApiClient;


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
