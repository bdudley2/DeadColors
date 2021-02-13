using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dayAndNightCycle : MonoBehaviour
{

    public float timeConstant = 4.0f;
    private float x;


    // Start is called before the first frame update
    void Start()
    {
        transform.localRotation = Quaternion.Euler(100.0f, 30.0f, 40.0f);
        x = transform.rotation.x;
    }

    // Update is called once per frame
    void Update()
    {
        x += (Time.deltaTime * timeConstant) % 360;
        transform.localRotation = Quaternion.Euler(x, 0.0f, 0.0f);
    }
}
