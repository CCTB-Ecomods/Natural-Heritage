using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : GeneralTileType
{
    private static Dictionary<Intensity, GameObject[]> cityMeshes = new Dictionary<Intensity, GameObject[]>();
    public static TileTypeStruct tileTypeStruct
    {
        get
        {
            return new TileTypeStruct(TileType.CITY, "City", (TileData tileData) => new City(tileData));
        }
        private set { tileTypeStruct = value; }
    }

    public City(TileData data) : base(data)
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
          //      River.tileTypeStruct
          //};
        }
        Type = data.tileType = TileType.CITY;

        if (cityMeshes.Count <= 0)
            cityMeshes = LoadMeshes();
    }

    public override GameObject[] GetMeshes()
    {
        return cityMeshes[Data.intensity];
    }
}
