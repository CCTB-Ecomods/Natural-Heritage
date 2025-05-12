using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(TileSettingsElements))]
public class TileSettingsScript : MonoBehaviour
{
    private TileSettingsElements _tileSettingsElements;

    private Dictionary<int, GameObject> _selectedTable;
    public TileType[] _notChangeableTileTypes = { TileType.CITY, TileType.RIVER };

    private WorldData world;
	
    private void Awake()
    {
        _tileSettingsElements = GetComponent<TileSettingsElements>();
        world = GameObject.FindGameObjectWithTag("GameManager").GetComponent<IGameLogic>().GetWorld();
    }
    private void Start()
    {
        activateMenu(false);
    }
    public void updateData(Dictionary<int, GameObject> selectedTable)
    {
        if (selectedTable.Count == 0)
        {
            activateMenu(false);
            return; //none selected
        }
        activateMenu(true);
        LoadCost.instances.ForEach(c => c.UpdateCost());
        //find possible types for TypeDropdown
        var typeStructs = GeneralTileType.GetPossibleTypeNames(selectedTable.Values);
        List<TileType> types = new List<TileType>();
        
        foreach (var typeStruct in typeStructs)
        {
            types.Add(typeStruct.type);
        }
        _tileSettingsElements.showTypes(types);

        _selectedTable = selectedTable;

        //find active type and intensity if possible
        var first = selectedTable.First().Value.GetComponent<TileData>();
        bool allTypesEqual = true;
        TileType equalType = first.type.Type;

        bool allIntensityEqual = true;
        Intensity equalIntensity = first.intensity;
        if (selectedTable.Count > 0)
            foreach (GameObject selectedHex in _selectedTable.Values)
            {
                var tileData = selectedHex.GetComponent<TileData>();
                if (tileData.type.Type != equalType)
                {
                    allTypesEqual = false;
                }
                if (tileData.intensity != equalIntensity)
                {
                    allIntensityEqual = false;
                }
                if (!allTypesEqual && !allIntensityEqual) break;
            }
        _tileSettingsElements.unmarkAll();
        if (allTypesEqual)
            _tileSettingsElements.markType(equalType);
        if (allIntensityEqual)
            _tileSettingsElements.markIntensity(equalIntensity);
    }
	
    private void activateMenu(bool b)
    {
        gameObject.SetActive(b);
    }

    public void changeType(TileType type)
    {
        var typeStructs = GeneralTileType.GetPossibleTypeNames(_selectedTable.Values);
        var Type = typeStructs.First(t => t.type == type);
        foreach (GameObject selectedHex in _selectedTable.Values)
        {
            HexagonUi hexUI = selectedHex.GetComponent<HexagonUi>();
            TileData tileData = selectedHex.GetComponent<TileData>();
            if (tileData.type.Type == type) continue;
            tileData.type = Type.getInstance(tileData);
            hexUI.closeWithChange();
            Economy.PayLandUseChange(world, false);  //pay for a tile type change
            selectedHex.GetComponent<TileMeshUpdater>().UpdateMesh();
        }
        TopBarUiController.UpdateBalanceChange();
    }

    internal void changeIntensity(Intensity intensity)
    {
        foreach (GameObject selectedHex in _selectedTable.Values)
        {
            HexagonUi hexUI = selectedHex.GetComponent<HexagonUi>();
            TileData tileData = selectedHex.GetComponent<TileData>();
            if (tileData.intensity == intensity) continue;
            tileData.intensity = intensity;
            hexUI.closeWithChange();
            Economy.PayLandUseChange(world, true);  //pay for a tile use change
            selectedHex.GetComponent<TileMeshUpdater>().UpdateMesh();
        }
        TopBarUiController.UpdateBalanceChange();
    }
}
