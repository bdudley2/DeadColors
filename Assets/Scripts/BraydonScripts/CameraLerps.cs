using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLerps : MonoBehaviour
{
    private GameObject player;
    private GameObject playerCam;
    private GameObject cam1;
    private GameObject cam2;
    private GameObject cam3;

    public static bool readyToStartGame;
    private int camSelector;
    private float totalTime;
    private int textCount;

    private Vector3 cam1Start;
    private Vector3 cam2Start;
    private Vector3 cam3Start;

    public Vector3 cam1End;
    public Vector3 cam2End;
    public Vector3 cam3End;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        player.transform.GetChild(0).gameObject.SetActive(false); //turns player cyclinder to be invisible
        playerCam = GameObject.FindWithTag("MainCamera");
        cam1 = transform.GetChild(0).GetChild(0).gameObject;
        cam2 = transform.GetChild(0).GetChild(1).gameObject;
        cam3 = transform.GetChild(0).GetChild(2).gameObject;
        Time.timeScale = 0;
        readyToStartGame = false;
        playerCam.SetActive(false);
        cam1.SetActive(true);
        cam2.SetActive(false);
        cam3.SetActive(false);
        camSelector = 0;
        totalTime = 0.0f;
        textCount = 0;
        cam1Start = cam1.transform.position;
        cam2Start = cam2.transform.position;
        cam3Start = cam3.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        totalTime = Time.deltaTime;
        swapCamCheck();

        if (Input.GetKeyDown("space")) {
            textCount++;
            if (textCount >= 10) readyToStartGame = true;
        }
        if (readyToStartGame) {
            Time.timeScale = 1;
            playerCam.SetActive(true);
        }
    }

    void swapCamCheck() {
        if (totalTime < 5) {
            cam3.SetActive(false);
            cam1.SetActive(true);
            lerpCamPos(cam1, cam1Start, cam1End);
        } else if (totalTime < 10) {
            cam1.SetActive(false);
            cam2.SetActive(true);
            lerpCamPos(cam2, cam2Start, cam2End);
        } else if (totalTime < 15) {
            cam2.SetActive(false);
            cam3.SetActive(true);
            lerpCamPos(cam3, cam3Start, cam3End);
        } else {
            resetCamPositions();
            totalTime = 0.0f;
        }       
    }

    void lerpCamPos(GameObject cam, Vector3 start, Vector3 end) {
        cam.transform.position = Vector3.Lerp(start, end, (totalTime % 5) / 5);
    }

    void resetCamPositions() {
        cam1.transform.position = cam1Start;
        cam2.transform.position = cam2Start;
        cam3.transform.position = cam3Start;
    }

    void swapText() {
        switch(textCount) {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
            case 6:
                break;
            case 7:
                break;
            case 8:
                break;
            case 9:
                break;
            case 10:
                break;
            case 11:
                break;
            case 12:
                break;
            case 13:
                break;
            case 14:
                break;
        }
    }
}
