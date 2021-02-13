using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Ignore the red underlines when importing UnityEngine.UI (includes Text)

public class ScorePoints : MonoBehaviour
{

    private GameObject ScoreText;
    private Text ScoreTextComponent;

    private GameObject MultiplyText;
    private Text MultiplyTextComponent;
    // public PointSystem myPoints;
    private int score;
    private int scoreOld;

    private float time1;
    private bool isScaling;
    private Vector3 origScale;
    private Vector3 origPos;
    private Color origColor;
    private int CoroutineCount;
    private Color origMColor;


    public bool turnOffScoreAnimations = false;

    void Awake() {
        StopAllCoroutines();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        // Gets the hierarchy child as a GameObject
        // GetChild(i) where i is child index we want (1 being the second child)
        ScoreText = transform.GetChild(1).gameObject;
        MultiplyText = transform.GetChild(2).gameObject;

        // Once we have the gameObject we want, save the text component so we can modify just that
        ScoreTextComponent = ScoreText.GetComponent<Text>();
        MultiplyTextComponent = MultiplyText.GetComponent<Text>();

        //Init score to zero
        score = 0;
        scoreOld = score;
        time1 = 0.0f;
        isScaling = false;

        origScale = ScoreTextComponent.transform.localScale;
        origPos = ScoreText.transform.localPosition;
        origColor = ScoreTextComponent.color;

        CoroutineCount = 0;

        origMColor = MultiplyTextComponent.color;
    }

    // Update is called once per frame
    void Update()
    {
        time1 += Time.deltaTime;
        score = PointSystem.Instance.points;
        // score = myPoints.getPoints();

        // In case our score reaches a size too big for our scoreboard
        if (score >= 10000001) score = 10000001;

        // Set text of the Text Component to "SCORE: <amount>" (right-aligned in Editor)
        ScoreTextComponent.text = score.ToString();

        // Sets fontStyle of Text Component to be Bold
        // ScoreTextComponent.fontStyle = FontStyle.Bold;

        //=================================== Score Multiplier

        if (scoreOld != score) {
            PointSystem.Instance.multiplier++;
            MultiplyTextComponent.color = origMColor;
            time1 = 0.0f;
        }
        if (time1 > 1.5f && PointSystem.Instance.multiplier > 1) {
            // flickerStarted = true;
            Color myC = MultiplyTextComponent.color;
            myC.a = (Mathf.Sin(time1 * 20) + 0.6f) / 2;
            MultiplyTextComponent.color = myC;
        }

        if (scoreOld == score && time1 > 2.5f) {
            // flickerStarted = false;
            PointSystem.Instance.multiplier = 1;
            MultiplyTextComponent.color = origMColor;
        }

        MultiplyTextComponent.text = "x" + Mathf.Log(PointSystem.Instance.multiplier + 1, 2).ToString("F2");

        //=================================== Score Animation

        // little animation for points text when earning new points
        if (scoreOld != score && turnOffScoreAnimations == false) {
            // bigger animation if got kill
            if (score - scoreOld > 140) {
                StartCoroutine(scaleUp(2.0f));
            } else {
                StartCoroutine(scaleUp(1.4f));
                // ScoreTextComponent.transform.localScale 
            }
            scoreOld = score;
        }
    }

    public IEnumerator scaleUp(float scaleFactor) {
        if (isScaling && CoroutineCount == 0) yield break;
        isScaling = true;
        CoroutineCount++;
        if (scaleFactor >= 2.0f) ScoreTextComponent.color = Color.magenta;
        Vector3 topScale = origScale * scaleFactor;
        float startTime = Time.time;
        while (Time.time - startTime < 0.4f && ScoreTextComponent.transform.localScale.x <= topScale.x) {
            ScoreTextComponent.transform.localScale = Vector3.Lerp(ScoreTextComponent.transform.localScale, topScale, (Time.time - startTime) / 0.4f);
            yield return null;
        }
        //-----------------------------
        CoroutineCount--;
        ScoreTextComponent.transform.localScale = topScale;
        // ScoreText.transform.localPosition = topPos;
        while (CoroutineCount != 0) {
            yield return new WaitForSeconds(0.1f);
            if (CoroutineCount == 1) {
                scaleFactor = 1.95f; // set only so we can continue onto second part of code
            }
        }
        if (scaleFactor >= 2.0f) {
            yield return new WaitForSeconds(1.2f);
            ScoreTextComponent.color = origColor;
        } else if (scaleFactor <= 1.9f) {
            isScaling = false;
            yield break;
        }
        isScaling = false;
        // ScoreTextComponent.color = origColor;
        //-----------------------------
        startTime = Time.time;
        while (Time.time - startTime < 0.4f && ScoreTextComponent.transform.localScale.x >= origScale.x) {
            ScoreTextComponent.transform.localScale = Vector3.Lerp(ScoreTextComponent.transform.localScale, origScale, (Time.time - startTime) / 0.4f);
            yield return null;
        }
        ScoreTextComponent.transform.localScale = origScale;
        // ScoreTextComponent.color = origColor;
    }
    // public IEnumerator scaleUp() {
    //     GameObject scalingTextObject = Instantiate(ScoreText);
    //     Color myColor = scalingTextObject.GetComponent<Text>().color;
    //     myColor = new Color(myColor.r, myColor.g, myColor.b, 0.5f);
    //     var alphaColor = myColor.a;
    //     while (alphaColor > 0.0f ) {
    //         alphaColor -= 
    //     }
    //     Destroy(scalingTextObject);
    // }
}
