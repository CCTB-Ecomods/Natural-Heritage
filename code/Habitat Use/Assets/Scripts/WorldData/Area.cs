using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Area : IArea
{
    private int _areaCreationId;
    private Intensity _areaIntensity;
    private TileType _tileType;
    private List<TileData> _tilesInArea = new List<TileData>();

    public Intensity GetIntensity()
    {
        if (_tilesInArea != null)
            _areaIntensity = _tilesInArea[0].intensity;
        return _areaIntensity;
    }

    public int GetSize()
    {
        return _tilesInArea.Count;
    }

    public List<TileData> GetTileList()
    {
        return _tilesInArea;
    }

    public void AddTile(TileData tile)
    {
        _tilesInArea.Add(tile);
    }

    public TileType GetTileType()
    {
        if (_tilesInArea != null)
            _tileType = _tilesInArea[0].type.Type;
        return _tileType;
    }

	public float GetAverageBiodiversity()
	{
		int i = 0;
		float avg = 0;
		foreach (TileData td in _tilesInArea) {
			i++;
			avg += td.currentBiodiversity;
		}
		return avg/i;
	}
	
	public float GetAverageProductivity()
	{
		int i = 0;
		float avg = 0;
		foreach (TileData td in _tilesInArea) {
			i++;
			avg += td.currentProductivity;
		}
		return avg/i;
	}

}
