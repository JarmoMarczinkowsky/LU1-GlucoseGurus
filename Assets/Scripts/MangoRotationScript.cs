using System.Collections;
using UnityEngine;

public class MangoRotationScript : MonoBehaviour
{
    private float wiggleFrequency = 3f; // Hoeveel keer per seconde de afbeelding heen en weer beweegt
    private float wiggleAngle = 15f;   // De hoeveelheid rotatie 

    private void Start()
    {
        StartCoroutine(Wiggling());
    }

    private IEnumerator Wiggling()
    {
        int amountOfWiggles = 0;

        while (amountOfWiggles < 4)
        {
            // Maak een sinusgolf beweging voor de rotatie
            float wiggle = Mathf.Sin(Time.time * wiggleFrequency) * wiggleAngle;
            if(wiggle < 0.3f && wiggle > -0.3f)
            {
                amountOfWiggles++;
            }
            
            transform.rotation = Quaternion.Euler(0f, 0f, wiggle);

            yield return null;
        }
    }
}
