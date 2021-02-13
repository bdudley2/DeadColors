using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Ignore the red underlines when importing UnityEngine.UI (includes Text)

public class PlayerHealthHUD : MonoBehaviour
{

    private GameObject HealthText;
    private Text HealthTextComponenet;
    //public static PointSystem myPoints;
    public static HealthSystem healthSystemHUD;
    private float health;

    // Start is called before the first frame update
    void Start()
    {
        // Gets the hierarchy child as a GameObject
        // GetChild(i) where i is child index we want (1 being the second child)
        HealthText = transform.GetChild(1).gameObject;

        // Once we have the gameObject we want, save the text component so we can modify just that
        HealthTextComponenet = HealthText.GetComponent<Text>();

        //Init health to zero
        health = 0;

        //myPoints = new PointSystem(0);
        healthSystemHUD = new HealthSystem(100);
    }

    // Update is called once per frame
    void Update()
    {
        health = healthSystemHUD.getHealth();

        // In case our score reaches a size too big for our scoreboard
        //if (score >= 1000000000) score = 999999999;

        // Set text of the Text Component to "SCORE: <amount>" (right-aligned in Editor)
        HealthTextComponenet.text = (Mathf.Round(health)).ToString() + "%";

        // Sets fontStyle of Text Component to be Bold
        // HealthTextComponenet.fontStyle = FontStyle.Bold;
    }
}
