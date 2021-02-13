using System;
using System.Collections.Generic;
using UnityEngine;

public class PointSystem : MonoBehaviour {

    public static PointSystem Instance { get; private set; }
    public int points = 0;

    public int multiplier = 1;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            // DontDestroyOnLoad(gameObject); // use if you want to keep track of high scores across multiple games
        } else {
            Destroy(gameObject);
        }
    }

    // Initializes PointSystem for player starting at a choosen amount of points
    void Start() {
        points = 0;
        multiplier = 1;
    }


    //Adds the specified points to the corresponding point system
    public void addPoints(int points_) {
        if (points + (points_ * (int)Mathf.Ceil(Mathf.Log(multiplier + 1, 2))) < 0) {
            Debug.LogError("Points shouldn't go below 0 -- don't update player points");
        } else {
            points += (points_ * (int)Mathf.Ceil(Mathf.Log(multiplier + 1, 2)));
        }
    }


    // Get Player's current points 
    // @returns an integer
    public int getPoints() {
        return points;
    }


    // Set Player's points to a specific amount (currently not used anywhere)
    public void setPoints(int newPoints) {
        if (newPoints < 0) {
            Debug.LogError("Points shouldn't go below 0 -- don't update player points");
        } else {
            points = newPoints;
        }
    }
}