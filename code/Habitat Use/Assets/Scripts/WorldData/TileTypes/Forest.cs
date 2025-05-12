using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forest : GeneralTileType
{
    private static Dictionary<Intensity, GameObject[]> forestMeshes = new Dictionary<Intensity, GameObject[]>();
    public static TileTypeStruct tileTypeStruct
    {
        get
        {
            return new TileTypeStruct(TileType.FOREST, "Forest", (TileData tileData) => new Forest(tileData));
        }
        private set { tileTypeStruct = value; }
    }

    public Forest(TileData data) : base(data)
    {
        if (adjacentTypes == null)
        {
            adjacentTypes = new[] {
                Forest.tileTypeStruct,
                Field.tileTypeStruct,
            };

            //DEBUG: MapCreation
            //  adjacentTypes = new[] {
            //      tileTypeStruct,
            //      Forest.tileTypeStruct,
            //      Field.tileTypeStruct,
            //      River.tileTypeStruct
            //};
        }

        Type = data.tileType = TileType.FOREST;

        if (forestMeshes.Count <= 0)
            forestMeshes = LoadMeshes();
    }

    public override GameObject[] GetMeshes()
    {
        return forestMeshes[Data.intensity];
    }
}
