using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class River : GeneralTileType
{
    private static Dictionary<Intensity, GameObject[]> riverMeshes = new Dictionary<Intensity, GameObject[]>();
    public static TileTypeStruct tileTypeStruct { 
        get {
            return new TileTypeStruct(TileType.RIVER, "River", (TileData tileData) => new River(tileData));
        }
        private set { tileTypeStruct = value; }
    }

    public River(TileData data) : base(data)
    {
        if (adjacentTypes == null)
        {
            adjacentTypes = new[] {
                tileTypeStruct
            };
            //DEBUG: MapCreation
            //  adjacentTypes = new[] {
            //      tileTypeStruct,
            //      Forest.tileTypeStruct,
            //      Field.tileTypeStruct,
            //      City.tileTypeStruct
            //};
        }

        Type = data.tileType = TileType.RIVER;
        if (riverMeshes.Count <= 0)
            riverMeshes = LoadMeshes();
    }

    public override GameObject[] GetMeshes()
    {
        return riverMeshes[Data.intensity];
    }
}
