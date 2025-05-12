using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


public abstract class GeneralTileType 
{
    public TileType Type { get; set; }
    private static TileTypeStruct[] allTypeNames;
    protected TileTypeStruct[] adjacentTypes = null;
    protected TileData Data { get; private set; }


    public GeneralTileType(TileData data)
    {
        Data = data;
        allTypeNames = new[] { 
            River.tileTypeStruct,
            Forest.tileTypeStruct,
            Field.tileTypeStruct,
            City.tileTypeStruct
        };
    }

    public static TileTypeStruct[] GetPossibleTypeNames(IEnumerable<GameObject> hexagons)
    {
        var intersect = allTypeNames;

        foreach (var hexagon in hexagons)
        {
            var adjacencys = hexagon.GetComponent<TileData>().type.adjacentTypes;
            intersect = intersect.Intersect(adjacencys).ToArray();
        }
        return intersect;
    }
    public abstract GameObject[] GetMeshes();
    protected Dictionary<Intensity, GameObject[]> LoadMeshes()
    {
        Dictionary<Intensity, GameObject[]> dic = new Dictionary<Intensity, GameObject[]>();
        
        foreach (Intensity intensity in Enum.GetValues(typeof(Intensity)))
        {
            dic.Add(intensity, Resources.LoadAll<GameObject>("TileMeshes/" + Type + "/" + intensity));
        }

        return dic;
    }
}
public struct TileTypeStruct
{
    public TileType type { get; private set; }
    public string name { get; private set; }
    public Func<TileData, GeneralTileType> getInstance { get; private set; }
    public TileTypeStruct(TileType type, string name, Func<TileData, GeneralTileType> getInstance)
    {
        this.type = type;
        this.name = name;
        this.getInstance = getInstance;
    }
}