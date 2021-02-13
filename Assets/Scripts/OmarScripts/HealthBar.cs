using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//code derived by YouTube Tutorial by CodeMonkey
//https://www.youtube.com/watch?v=0T5ei9jN63M
public class HealthBar : MonoBehaviour
{
    private void Update() {
        // Braydon Addition - have health bar always facing player
        transform.rotation = Camera.main.transform.rotation;
    }
}
