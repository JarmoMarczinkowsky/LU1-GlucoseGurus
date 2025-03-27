using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;


public class RouteManagerScript : MonoBehaviour
{
    public GameObject RouteA;
    public GameObject RouteB;
    public GameObject Basket;
    public List<Sprite> Baskets;


    private int mangoCount = 0;
    private TreatmentplanManagerScript treatmentplanManagerScript;
    private int route;

    void Start()
    {
        mangoCount = 0;

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

    public void SetBasket()
    {
        mangoCount++;
        if(mangoCount <= Baskets.Count)
        {
            Basket.GetComponent<Image>().sprite = Baskets[mangoCount];
        }
    }



    void Update()
    {
        
    }
}
