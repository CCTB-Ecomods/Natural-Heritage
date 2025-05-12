using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct IntensityElement
{
    [SerializeField] public Intensity intensity;
    [SerializeField] public GameObject intensityPanel;
}
[Serializable]
public struct TypeElement
{
    [SerializeField] public TileType type;
    [SerializeField] public GameObject typePanel;
}
public class TileSettingsElements : MonoBehaviour
{
    [SerializeField] public IntensityElement[] intensityElements = new IntensityElement[4];
    [SerializeField] public TypeElement[] typeElements = new TypeElement[4];

    public void showTypes(TileType[] types)
    {
        //disable all panels
        foreach (var type in typeElements)
            type.typePanel.SetActive(false);

        //enable the right ones
        foreach (var type in types)
        {
            var panel = typeElements.First(t => t.type == type).typePanel;
            panel.SetActive(true);
        }
    }
    public void showTypes(List<TileType> types)
    {
        showTypes(types.ToArray());
    }

    public void markType(TileType type)
    {
        var panel = typeElements.First(t => t.type == type).typePanel;
        var textMeshProElement = panel.GetComponentInChildren<TMPro.TextMeshProUGUI>().fontStyle = TMPro.FontStyles.Bold;
    }
    public void markIntensity(Intensity equalIntensity)
    {
        var panel = intensityElements.First(t => t.intensity == equalIntensity).intensityPanel;
        var textMeshProElement = panel.GetComponentInChildren<TMPro.TextMeshProUGUI>().fontStyle = TMPro.FontStyles.Bold;
    }
    public void unmarkAll()
    {
        foreach (var t in typeElements)
            t.typePanel.GetComponentInChildren<TMPro.TextMeshProUGUI>().fontStyle = TMPro.FontStyles.Normal;
        foreach (var i in intensityElements)
            i.intensityPanel.GetComponentInChildren<TMPro.TextMeshProUGUI>().fontStyle = TMPro.FontStyles.Normal;
    }
    public void showIntensitys(Intensity[] intensitys)
    {
        //disable all panels
        foreach (var intensity in intensityElements)
            intensity.intensityPanel.SetActive(false);

        //enable the right ones
        foreach (var intensity in intensitys)
        {
            var panel = intensityElements.First(t => t.intensity == intensity).intensityPanel;
            panel.SetActive(true);
        }
    }

}
