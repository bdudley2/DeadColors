using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarDamageAnimation : MonoBehaviour
{
    private float goUpTimer;
    private float fadeTimer;
    private float alphaFadeSpeed;
    private float fallSpeed;
    private SpriteRenderer SR;

    private void Awake()
    {
        SR = transform.GetComponentInChildren<SpriteRenderer>();
        goUpTimer = 0f;
        fadeTimer = 0.2f;
        alphaFadeSpeed = 5f;
        fallSpeed = 0.7f;
    }

    private void Update()
    {
        goUpTimer -= Time.deltaTime;
        if(goUpTimer < 0)
        {
            transform.position += Vector3.up * fallSpeed * Time.deltaTime;
            fadeTimer -= Time.deltaTime;
            if (fadeTimer < 0) {
                SR.color = new Color(SR.color.r, SR.color.g, SR.color.b, SR.color.a - (alphaFadeSpeed * Time.deltaTime));
                if (SR.color.a <= 0) {
                    Destroy(gameObject);
                }
            }
        }
    }
}
