using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IArea
{
    int GetSize();

    List<TileData> GetTileList();
    void AddTile(TileData tile);

    Intensity GetIntensity();
    TileType GetTileType();

	float GetAverageBiodiversity();
	float GetAverageProductivity();
}
