using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using TMPro;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.UI;

public class Tooltips : MonoBehaviour
{
    private static CultureInfo _de;

    private static GameObject _currentTooltip;
    
    private static float _timer = 1;
    private static float _timePassed = 0;

    private static GameObject _tileTooltipObj;
    private static TextMeshProUGUI _tileTooltipNameText;
    private static TextMeshProUGUI _tileTooltipValueText;

    private static GameObject _balanceTooltipObj;
    private static TextMeshProUGUI _balanceTooltipNameText;
    private static TextMeshProUGUI _balanceTooltipValueText;

    void Start()
    {
        _tileTooltipObj = transform.Find("TileTooltip").gameObject;
        _tileTooltipNameText = _tileTooltipObj.transform.Find("NameText").GetComponent<TextMeshProUGUI>();
        _tileTooltipValueText = _tileTooltipObj.transform.Find("ValueText").GetComponent<TextMeshProUGUI>();

        _balanceTooltipObj = transform.Find("BalanceTooltip").gameObject;
        _balanceTooltipNameText = _balanceTooltipObj.transform.Find("NameText").GetComponent<TextMeshProUGUI>();
        _balanceTooltipValueText = _balanceTooltipObj.transform.Find("ValueText").GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (_timePassed >= _timer)
        {
            _currentTooltip?.SetActive(true);
            _currentTooltip?.SetActive(false);
            _currentTooltip?.SetActive(true);
        }
        _timePassed += Time.deltaTime;
    }

    public static void HideTooltip() 
    {
        _currentTooltip?.SetActive(false);
        _currentTooltip = null; 
    }

    public static void ShowTileTooltip(TileData tileData, float timer, Vector2 pos)
    {
        HideTooltip();

        _timer = timer;
        _timePassed = 0;
        SetTileTooltipValues(tileData);
        _tileTooltipObj.transform.position = pos;
        _currentTooltip = _tileTooltipObj;
    }
    private static void SetTileTooltipValues(TileData tileData)
    {
        _tileTooltipNameText.text = "";
        _tileTooltipValueText.text = "";

        if (LayerUI.activeLayer == OverlayLayers.BIODIVERSITY)
        {
            _tileTooltipNameText.text = 
                "Intensity: \n" +
                "Area Size: \n" +
                "Neighbur Tiles: \n" +
                "Temporary Effects: \n" +
                "Overall Biodiversity: ";

            float overallBio = tileData.currentBiodiversity;

            foreach (var val in tileData.biodiversityFactors)
            {
                _tileTooltipValueText.text += (val >= 0) ? "<color=#2F8521> +" : "<color=#FB6D3B> ";
                double roundedVal = Math.Round(val, 1);
                _tileTooltipValueText.text += roundedVal.ToString() + "\n</color>";
            }

            _tileTooltipValueText.text += (overallBio >= 0) ? "<color=#2F8521> +" : "<color=#FB6D3B> ";
            _tileTooltipValueText.text += overallBio.ToString() + "</color>";
        }
        else if (LayerUI.activeLayer == OverlayLayers.PRODUCTIVITY)
        {
            _tileTooltipNameText.text = 
                "Intensity: \n" +
                "Area Size: \n" +
                "Neighbour Tiles: \n" +
                "Biodiversity: \n" +
                "Temporary Effects: \n" +
                "Overall Productivity: ";

            float overallProd = tileData.currentProductivity;
			
            foreach (var val in tileData.productivityFactors)
            {
                _tileTooltipValueText.text += (val >= 0) ? "<color=#2976FF>$ +" : "<color=#FB6D3B>$ ";
                double roundedVal = Math.Round(val, 1);
                _tileTooltipValueText.text += roundedVal.ToString() + "\n</color>";
            }

            _tileTooltipValueText.text += (overallProd >= 0) ? "<color=#2976FF>$ +" : "<color=#FB6D3B>$ ";
            _tileTooltipValueText.text += overallProd.ToString() + "</color>";
        }
    }

    public static void ShowBalanceTooltip(WorldData worldData, float timer)
    {
        HideTooltip();

        _timer = timer;
        _timePassed = 0;
        SetBalanceTooltipValues(worldData);
        _currentTooltip = _balanceTooltipObj;
    }
    private static void SetBalanceTooltipValues(WorldData worldData)
    {
        StringBuilder valueSb = new StringBuilder();

        _balanceTooltipNameText.text = "Industry Value: \nCitizens: ";

        var industryValue = worldData.landscapeValues.IndustryValue;
        if (industryValue >= 0) valueSb.Append("<color=#2976FF>$ +");
        else valueSb.Append("<color=#FB6D3B>$ ");
        valueSb.Append(industryValue.ToString() + "\n</color>");

        var citizenCost = - worldData.populationSize;
        if (citizenCost >= 0) valueSb.Append("<color=#2976FF>$ +");
        else valueSb.Append("<color=#FB6D3B>$ ");
        valueSb.Append(citizenCost + "\n</color>");

        if (worldData.revenue.Count > 0)
        {
            _balanceTooltipNameText.text += "\nBonuses: \n";
            valueSb.Append("\n");

            foreach (var bonus in worldData.revenue)
            {
                _balanceTooltipNameText.text += " " + bonus.GetName() + ": \n";
                if (bonus.GetValue() >= 0) valueSb.Append("<color=#2976FF>$ +");
                else valueSb.Append("<color=#FB6D3B>$ ");
                valueSb.Append(bonus.GetValue().ToString() + "\n");
            }
        }

        _balanceTooltipValueText.text = valueSb.ToString();
    }
}
