using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class displayGameOverStats : MonoBehaviour
{

    private TextMeshProUGUI statText;
    public PointSystem myPoints;
    public WaveController myWaves;

    // Update is called once per frame
    void Update()
    {
        int scoreNum = PointSystem.Instance.points;
        int waveNum = WaveController.Instance.getWaveNum() - 1;
        int totalKills = WaveController.Instance.getTotalKills();

        statText = transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();

        statText.text = scoreNum.ToString() + "\n" + totalKills.ToString() + "\n" + waveNum.ToString() + "/10";
    }
}
