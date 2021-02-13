using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleControls : MonoBehaviour
{
    private GameObject textChild;
    private bool ControlsActive;

    // Start is called before the first frame update
    void Start()
    {
        ControlsActive = false;
        textChild = transform.GetChild(0).GetChild(1).gameObject;
        textChild.SetActive(ControlsActive);
    }

    // Update is called once per frame
    void Update()
    {
        // allow to toggle even in pause menu (don't add timeDelta constraint)
        if (Input.GetKeyDown(KeyCode.I)) {
            ControlsActive = !ControlsActive;
            textChild.SetActive(ControlsActive);
        }
    }
}
