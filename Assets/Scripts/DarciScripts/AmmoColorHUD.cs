using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoColorHUD : MonoBehaviour
{

    public float enabledOpacity = 0.8f;
    public float disabledOpacity = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ColorEnum gunColorEnum = SG.Instance.getGunColorEnum();
        highlightSelectedColor(gunColorEnum);
    }

    private void highlightSelectedColor(ColorEnum colorEnum) {
        int childIdx = (int) colorEnum;
        for (int i = 0; i < this.transform.childCount; i++) {
            Transform child = this.transform.GetChild(i);

            Image image = child.GetComponent<Image>();
            Text text = child.GetChild(0).GetComponent<Text>();

            Color newImageColor = image.color;
            Color newTextColor = text.color;

            // Change opacity of image and text based on if this is the selected child
            if (i == childIdx) {
                newImageColor.a = enabledOpacity;
                newTextColor = Color.black;
                newTextColor.a = 1.0f;
            } else {
                newImageColor.a = disabledOpacity;
                newTextColor = Color.white;
                newTextColor.a = disabledOpacity;
            }

            image.color = newImageColor;
            text.color = newTextColor;
        }
    }
}
