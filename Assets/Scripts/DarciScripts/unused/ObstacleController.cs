using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public float speed = 1.0f;
    public Vector3 pointA;
    public Vector3 pointB;

    // Start
    void Start()
    {
        pointA = this.transform.position;
        pointB = this.transform.position;
        pointB.x += 4;
    }

    // Update is called once per frame
    void Update()
    {
        float time = Mathf.PingPong(Time.time*speed, 1);
        transform.position = Vector3.Lerp(pointA, pointB, time);
    }
}
