using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpdateWaveHUD : MonoBehaviour
{
    private GameObject waveController;

    private Text waveLabelText;
    private Text waveProgressText;

    // Braydon addition
    private TextMeshProUGUI waveAnnouncementText;
    private int oldWave;
    private bool newWaveBool;
    private float timeWait;

    // Start is called before the first frame update
    void Start()
    {
        waveLabelText = transform.GetChild(0).gameObject.GetComponent<Text>();
        waveProgressText = transform.GetChild(1).gameObject.GetComponent<Text>();
        waveController = GameObject.FindWithTag("waveController");

        // Braydon addition
        waveAnnouncementText = transform.GetChild(2).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        waveAnnouncementText.transform.position = new Vector3(-Screen.width/5, (4*Screen.height)/5, 0.0f);
        oldWave = 0;
        newWaveBool = false;
        timeWait = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        int waveNum = WaveController.Instance.getWaveNum();

        waveLabelText.text = "WAVE " + waveNum.ToString() + ":";

        int waveKills = WaveController.Instance.getWaveKills();
        int waveSize = WaveController.Instance.getWaveSize();
        waveProgressText.text = waveKills.ToString() + "/" + waveSize.ToString();


        // Braydon addition
        WaveAnnouncementAnimation(waveNum);
    }

    // Little Wave Announcement Animation
    // *** Relies on animation position moving relative to where the Wave HUD (parent) Display is
    void WaveAnnouncementAnimation(int waveNum) {
        // Setting up the text and positions
        waveAnnouncementText.text = "WAVE " + waveNum.ToString();
        if (oldWave != waveNum) {
            newWaveBool = true;
            oldWave = waveNum;
        } else {
            if (newWaveBool == false) waveAnnouncementText.transform.position = new Vector3(-Screen.width/5, (4*Screen.height)/5, 0.0f); //reset animation position off screen
        }

        // Animation
        timeWait += Time.deltaTime;
        if (newWaveBool && timeWait > 0.6f) {
            double animSpeed = Screen.width;
            animSpeed *= 2.2;
            if (waveAnnouncementText.transform.position.x < (Screen.width * 0.6)) animSpeed *= 0.7;
            // slow down the text when it's in the middle of the screen
            if (waveAnnouncementText.transform.position.x > (Screen.width * 0.38) &&
                waveAnnouncementText.transform.position.x < (Screen.width * 0.6)) {
                    animSpeed *= 0.1;
                    // slow down slightly towards end
                    if (waveAnnouncementText.transform.position.x > (Screen.width * 0.53)) {
                        animSpeed *= 0.6;
                    }
            }
            waveAnnouncementText.transform.position += new Vector3((float)animSpeed * Time.deltaTime, 0.0f, 0.0f);
            if (waveAnnouncementText.transform.position.x > (Screen.width * 1.5)) newWaveBool = false;
        }
    }
}