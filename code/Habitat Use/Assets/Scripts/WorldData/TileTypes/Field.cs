using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : GeneralTileType
{
    private static Dictionary<Intensity, GameObject[]> fieldMeshes = new Dictionary<Intensity, GameObject[]>();
    public static TileTypeStruct tileTypeStruct
    {
        get
        {
            return new TileTypeStruct(TileType.FIELD, "Field", (TileData tileData) => new Field(tileData));
        }
        private set { tileTypeStruct = value; }
    }
    public Field(TileData data) : base(data)
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

        Type = data.tileType = TileType.FIELD;

        if (fieldMeshes.Count <= 0)
            fieldMeshes = LoadMeshes();
    }

    public override GameObject[] GetMeshes()
    {
        return fieldMeshes[Data.intensity];
    }
}
