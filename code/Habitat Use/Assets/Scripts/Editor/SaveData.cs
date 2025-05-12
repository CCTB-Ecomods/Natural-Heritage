using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    public readonly TileType tileType;
    public readonly Intensity intensity;

    public SaveData(TileType tileType, Intensity intensity)
    {
        this.tileType = tileType;
        this.intensity = intensity;
    }
}
