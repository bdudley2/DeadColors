using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayScore : MonoBehaviour
{

    public static int totalScore = 0;
    private bool winBool = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnGUI() {
        //change color
        GUI.color = Color.yellow;
        //create a bigger font size for winning message
        GUIStyle winFontStyle = new GUIStyle(GUI.skin.button);
        winFontStyle.fontSize = 50;

        // x, y, width, height
        GUI.Box(new Rect(Screen.width - 120, Screen.height - 130, 100, 40), "Score\n" + totalScore);

        // If you damage something, add 10 points
        if (GUILayout.Button("damage for 10 points")) {
            totalScore += 10;
            Debug.Log("Added 10 points");
        }

        // If you kill something, add 50 points
        if (GUILayout.Button("kill for 50 points")) {
            totalScore += 50;
            Debug.Log("Added 50 points");
        }

        // make sure the score can go over 300 points
        if (totalScore > 300) totalScore = 300;

        // Press Button to spend points on 
        // Done after we set totalScore to 300 if we have to intentionally
        if (GUILayout.Button("Press to spend 100 points") && totalScore >= 100) {
            totalScore -= 100;
            Debug.Log("Spent 100 points on... something");
        }

        // Press Button if you have enough points
        if (GUILayout.Button("Once you have a score of 3:\n PRESS ME!") && totalScore == 300) {
            winBool = true;
            Debug.Log("You Win!");
        }

        if (winBool) {
            GUI.Box(new Rect(Screen.width / 2 - 150, Screen.height / 2 - 50, 300, 100), "You Win", winFontStyle);
        }
    }
}
