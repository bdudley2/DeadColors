using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using TMPro;

public class BobbingGameOverTitle : MonoBehaviour
{
    [Range(0, 30)]
    // Width of the x movement (relative to text's global scale)
    public int xScaleMult = 6;
    [Range(0, 30)]
    // Height of the y movement (relative to text's global scale)
    public int yScaleMult = 1;
    // Speed of the movement
    [Range(0, 3)]
    public float speedScaleMult = 1.2f;

    // Lissajous curve parameters
    [Range(0, 5)]
    // 'a' parameter for lissajous curve
    public int lissajousA = 1;
    [Range(0, 5)]
    // 'b' parameter for lissajous curve
    public int lissajousB = 3;
    // delta = PI / deltaDenom parameter for lissajous curve
    [Range(1, 4)]
    public int deltaDenom = 2;

    // LISSAJOUS: 
    // Uses parameterized movement to smoothly move point in 2D space
    // x = A*sin(at + d), y = B*sin(bt)
    // Some examples: https://www.101computing.net/wp/wp-content/uploads/Lissajous-Curves-768x230.png
    // e.g. a = 1, b = 1, delta = PI / 2 --> circle
    // e.g. a = 1, b = 2, delta = PI / 2 --> figure 8
    // e.g. a = 1, b = 3, delta = PI / 2 --> figure 8 with 3 loops

    private float t;
    private float xRange;
    private float yRange;
    private float delta;

    private TextMeshProUGUI wonText;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // if (HudControl.gameWon == true) {
        //     wonText = GetComponent<TextMeshProUGUI>();
        //     Color tempC = wonText.color;
        //     tempC.r = (tempC.r + 1) % 255;
        //     tempC.g = (tempC.g + 1) % 255;
        //     tempC.b = (tempC.b + 1) % 255;
        //     Debug.Log("Colors: r(" + tempC.r + "), g(" + tempC.g + "), b(" + tempC.b + ")");
        //     wonText.color = new Color(tempC.r, 0, tempC.b, tempC.a);
        // }


        delta = Mathf.PI / deltaDenom;
        t += speedScaleMult * Time.unscaledDeltaTime;
        // Lossy scale gives approximate global scale of text so we can scale movement proportionally
        xRange = transform.lossyScale.x * xScaleMult * 10;
        yRange = transform.lossyScale.y * yScaleMult * 10;

        // Uses Lissajous piecewise functions: x = A*sin(at + d), y = B*sin(bt)
        transform.position = transform.parent.position + new Vector3(xRange * Mathf.Sin(lissajousA * t + delta), yRange * Mathf.Sin(lissajousB * t), 0);
    }
}
