using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class intensityElement : MonoBehaviour
{
    [SerializeField] public Intensity intensity;
    Button button;
    static TileSettingsScript tileSettings;
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
        if (tileSettings == null)
            tileSettings = GameObject.FindGameObjectWithTag("TileSettings").GetComponent<TileSettingsScript>();
    }

    public void OnClick()
    {
        tileSettings.changeIntensity(intensity);
    }
}
