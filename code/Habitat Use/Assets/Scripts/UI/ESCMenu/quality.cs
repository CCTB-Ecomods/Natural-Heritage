using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class quality : MonoBehaviour
{
    private TextMeshProUGUI text;
    private bool lowQuality = false;

    public void ToggleQuality()
    {
        if (text == null)
            text = transform.Find("QualityText").GetComponent<TextMeshProUGUI>();

        lowQuality = ToggleShadows.togglePerformance();
       
        text.text = lowQuality ? "High Quality" : "Low Quality";
    }
}
