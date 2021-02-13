using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobbingPauseTitle : MonoBehaviour
{

    private float newY;
    private float newX;
    private float upOrDown;
    private float leftOrRight;
    private Vector3 baseState;
    // Start is called before the first frame update
    void Start()
    {
        baseState = transform.position;
        baseState.y -= Screen.height/8;
        // Debug.Log("baseState: " + baseState);
        float y = Random.Range(-80.0f, 80.0f);
        float x = Random.Range(-100.0f, 100.0f);
        transform.position += new Vector3(x, y, 0.0f); //start at random spot on HUD
        newY = y;
        newX = x;
        upOrDown = Random.Range(-1.0f, 1.0f);
        leftOrRight = Random.Range(-1.0f, 1.0f);
        if (upOrDown == 0) upOrDown = -0.1f;
        if (leftOrRight == 0) leftOrRight = -0.1f;
    }

    // Update is called once per frame
    void Update()
    {

        if (upOrDown > 0.0f) {
            newY += (0.001f);
            // if (/*newY > 100.0f || */transform.position.y > -20) {
            //     upOrDown *= -1.0f;
            // }
        } else {
            newY -= (0.001f);
            // if (/*newY < -100.0f ||*/ transform.position.y < -(4*Screen.height)/5) {
            //     upOrDown *= -1.0f;
            // }
        }
        if (leftOrRight > 0.0f) {
            newX += (0.001f);
            // if (/*newX > 130.0f ||*/ transform.position.x > Screen.width/2) {
            //     leftOrRight *= -1.0f;
            // }
        } else {
            newX -= (0.001f);
            // if (/*newX < -130.0f ||*/ transform.position.x < -Screen.width/2) {
            //     leftOrRight *= -1.0f;
            // }
        }

        transform.position = new Vector3(baseState.x - (Mathf.Cos(newX * 7) * Screen.width/12), baseState.y + (Mathf.Sin(newY * 5) * Screen.height/20), baseState.z);
        // Debug.Log(transform.position);
    }
}
