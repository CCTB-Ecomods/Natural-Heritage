using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScalar : MonoBehaviour
{
    public Material normalButtonMaterial;
    public Material activeButtonMaterial;
    public Material normalIconMaterial;
    public Material activeIconMaterial;

    public float buttonActiveScale = 1;

    private GameObject bioOverlayButton;
    private Image bioOverlayButtonBg;
    private Image bioOverlayButtonIcon;

    private GameObject prodOverlayButton;
    private Image prodOverlayButtonBg;
    private Image prodOverlayButtonIcon;

    private OverlayLayers activeLayer = OverlayLayers.NONE;

    void Start()
    {
        bioOverlayButton = transform.Find("BiodiversityOverlayButton").gameObject;
        bioOverlayButtonBg = bioOverlayButton.GetComponent<Image>();
        bioOverlayButtonIcon = bioOverlayButton.transform.Find("Icon").gameObject.GetComponent<Image>();
        
        prodOverlayButton = transform.Find("ProductivityOverlayButton").gameObject;
        prodOverlayButtonBg = prodOverlayButton.GetComponent<Image>();
        prodOverlayButtonIcon = prodOverlayButton.transform.Find("Icon").gameObject.GetComponent<Image>();

        bioOverlayButtonBg.material = normalButtonMaterial;
        bioOverlayButtonIcon.material = normalIconMaterial;

        prodOverlayButtonBg.material = normalButtonMaterial;
        prodOverlayButtonIcon.material = normalIconMaterial;
    }

    public void UpdateBiodiversityBtn()
    {
        if (activeLayer == OverlayLayers.BIODIVERSITY)
        {
            bioOverlayButton.transform.localScale = Vector3.one;
            bioOverlayButtonBg.material = normalButtonMaterial;
            bioOverlayButtonIcon.material = normalIconMaterial;
            activeLayer = OverlayLayers.NONE;
        }
        else
        {
            bioOverlayButton.transform.localScale = new Vector3(buttonActiveScale, buttonActiveScale, 1);
            bioOverlayButtonBg.material = activeButtonMaterial;
            bioOverlayButtonIcon.material = activeIconMaterial;
            activeLayer = OverlayLayers.BIODIVERSITY;
        }

        prodOverlayButton.transform.localScale = Vector3.one;
        prodOverlayButtonBg.material = normalButtonMaterial;
        prodOverlayButtonIcon.material = normalIconMaterial;
    }

    public void UpdateProductivityBtn()
    {
        if (activeLayer == OverlayLayers.PRODUCTIVITY)
        {
            prodOverlayButton.transform.localScale = Vector3.one;
            prodOverlayButtonBg.material = normalButtonMaterial;
            prodOverlayButtonIcon.material = normalIconMaterial;
            activeLayer = OverlayLayers.NONE;
        }
        else
        {
            prodOverlayButton.transform.localScale = new Vector3(buttonActiveScale, buttonActiveScale, 1);
            prodOverlayButtonBg.material = activeButtonMaterial;
            prodOverlayButtonIcon.material = activeIconMaterial;
            activeLayer = OverlayLayers.PRODUCTIVITY;
        }

        bioOverlayButton.transform.localScale = Vector3.one;
        bioOverlayButtonBg.material = normalButtonMaterial;
        bioOverlayButtonIcon.material = normalIconMaterial;
    }
}
