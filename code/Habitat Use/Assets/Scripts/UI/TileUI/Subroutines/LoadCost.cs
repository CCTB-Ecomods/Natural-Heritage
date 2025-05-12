using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TextMeshProUGUI))]
public class LoadCost : MonoBehaviour
{
    [SerializeField] ChangeType changeType;
    public static List<LoadCost> instances { get; private set; }
    public static selected_dictionary _selected_dictionary;
    private TextMeshProUGUI text; 
    private CultureInfo _de;

    private Intensity _intensity;
    private TileType _type;

    [SerializeField] intensityElement intensityElement;
    [SerializeField] typeElement typeElement;

    private void Awake()
    {
        if (_selected_dictionary == null)
            _selected_dictionary = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<selected_dictionary>();
        if (instances == null)
            instances = new List<LoadCost>();
        text = GetComponent<TextMeshProUGUI>();
        _de = new CultureInfo("de-DE");
        instances.Add(this);

        if (changeType == ChangeType.Intensity)
        {
            _intensity = intensityElement.intensity;
        }
        if (changeType == ChangeType.Type)
        {
            _type = typeElement.type;
        }
    }

    public void UpdateCost()
    {
        
        //int selectedCells = _selected_dictionary.selectedTable.Count;
        int cost = 0;
        if (changeType == ChangeType.Intensity)
            foreach (var selectedTile in _selected_dictionary.selectedTable.Values)
                if (selectedTile.GetComponent<TileData>().intensity != _intensity)
                    cost += Economy.intensityChangeCost;
        if (changeType == ChangeType.Type)
            foreach (var selectedTile in _selected_dictionary.selectedTable.Values)
                if (selectedTile.GetComponent<TileData>().type.Type != _type)
                    cost += Economy.usetypeChangeCost;

        if (cost == 0)
            text.SetText("0");
        else
            text.SetText(cost.ToString("##,#", _de));
    }
}
