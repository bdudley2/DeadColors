using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movePlayer : MonoBehaviour
{

    public float movementSpeed = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("w")) {
            this.transform.position += this.transform.forward * Time.deltaTime * movementSpeed;
        }
        if (Input.GetKey("a")) {
            this.transform.position -= this.transform.right * Time.deltaTime * movementSpeed;
        }
        if (Input.GetKey("s")) {
            this.transform.position -= this.transform.forward * Time.deltaTime * movementSpeed;
        }
        if (Input.GetKey("d")) {
            this.transform.position += this.transform.right * Time.deltaTime * movementSpeed;
        }
    }
}
