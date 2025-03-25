using UnityEngine;

public class RouteManagerScript : MonoBehaviour
{
    public GameObject RouteA;
    public GameObject RouteB;

    private TreatmentplanManagerScript treatmentplanManagerScript;
    private int route;

    void Start()
    {
        route = 1;

        if(route == 0)
        {
            // Chose Route A
            RouteA.SetActive(true);
            RouteB.SetActive(false);

            treatmentplanManagerScript = RouteA.GetComponentInChildren<TreatmentplanManagerScript>();
            treatmentplanManagerScript.SetUp();
        }
        else if(route == 1)
        {
            // Chose Route B
            RouteA.SetActive(false);
            RouteB.SetActive(true);

            treatmentplanManagerScript = RouteB.GetComponentInChildren<TreatmentplanManagerScript>();
            treatmentplanManagerScript.SetUp();
        }
    }

    void Update()
    {
        
    }
}
