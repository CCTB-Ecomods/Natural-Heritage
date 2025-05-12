using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static AdjacentDirection;
using static AdjacentCellVectors;


public class TileMapEditor : MonoBehaviour
{
    //flattened 2D-Array
    private TileData[] _grid;
    private int arraySize;

    public int Width { get => width; }
    [HideInInspector] [SerializeField] private int width;

    public int Height { get => height; }
    [HideInInspector] [SerializeField] private int height;

    public SaveData[] SaveData()
    {
        TileData[] worldTiles = gameObject.GetComponent<WorldData>().tiles;
        SaveData[] save = new SaveData[worldTiles.Length];

        foreach (var td in worldTiles)
        {
            save[GetArrayIndex(td.xCoordinate, td.yCoordinate, width)] = new SaveData(td.type.Type, td.intensity);
        }

        return save;
    }

    /// <summary>
    /// Creates an array of TileData, saves it in WorlData, while instantiating GameObjects in scene
    /// </summary>
    /// <param name="cellPrefab">the prefabs which will be created in the scene</param>
    /// <param name="width">amount of tiles in x direction</param>
    /// <param name="height">amount of tiles in y direction</param>
    public void CreateGrid(GameObject cellPrefab, int width, int height, SaveData[] save)
    {
        this.width = width;
        this.height = height;
        _grid = new TileData[width * height];

        //fill tile data array with tile data, while creating tile objects
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                _grid[x + y * width] = InstantiateCell(cellPrefab, x, y, width, save);
            }
        }

        //calculate and save adjacent tiles of each tile
        foreach (var tileData in _grid)
        {
            tileData.adjacentTiles = GetAdjTiles(tileData.xCoordinate, tileData.yCoordinate, width, height);
        }

        //set tile map array in world data
        gameObject.GetComponent<WorldData>().tiles = _grid;
    }


    /// <summary>
    /// Create Tile Objects at assigned place and add TileData script to them
    /// </summary>
    /// <returns>TileData component</returns>
    private TileData InstantiateCell(GameObject cellPrefab, int x, int y, int width, SaveData[] save)
    {
        Debug.Log("instantiating");
        //Creates new cell game object in scene
        GameObject cellObj = (GameObject) PrefabUtility.InstantiatePrefab(cellPrefab);
        cellObj.transform.position = GetNextPosition(x, y);
        cellObj.transform.rotation = Quaternion.identity;
        //GameObject cellObj = Instantiate(cellPrefab, GetNextPosition(x, y), Quaternion.identity);
        cellObj.transform.SetParent(gameObject.transform);

        //adds TileData script as component to game object
        TileData data = cellObj.GetComponent<TileData>();
        data.xCoordinate = x;
        data.yCoordinate = y;
        data.type = new River(data);

        if (save != null)
        {
            SaveData tileSave = save[GetArrayIndex(x, y, width)];
            switch (tileSave.tileType)
            {
                case TileType.RIVER:
                    data.type = new River(data);
                    break;
                case TileType.FOREST:
                    data.type = new Forest(data);
                    break;
                case TileType.FIELD:
                    data.type = new Field(data);
                    break;
                case TileType.CITY:
                    data.type = new City(data);
                    break;
                default:
                    throw new Exception("TileType of save not valid type");

            }
            data.intensity = tileSave.intensity;
        }
        else
            Debug.Log("no saved data existing");

        return data;
    }



    /// <summary>
    /// Calculates world position of the tile with given coordinates
    /// </summary>
    /// <returns>world position as Vector3</returns>
    private Vector3 GetNextPosition(int x, int y)
    {
        Vector3 nextPos = Vector3.zero;

        if (y % 2 != 0)
        {
            nextPos.x = 2 * HexMetrics.innerRadius * x;
            nextPos.z = 1.5f * HexMetrics.outerRadius * y;
        }
        else //if row index is even add half a tile offset
        {
            nextPos.x = 2 * HexMetrics.innerRadius * x + HexMetrics.innerRadius;
            nextPos.z = 1.5f * HexMetrics.outerRadius * y;
        }

        return nextPos;
    }


    /// <summary>
    /// Gets an array with the adjacent TileData. If no adjacent tile exists there will be a nullpointer in array.
    /// </summary>
    /// <param name="x">x coordinate</param>
    /// <param name="y">y coordinate</param>
    /// <param name="xMax">amount of tiles there are in x direction</param>
    /// <returns>Array with TileData or null values</returns>
    private TileData[] GetAdjTiles(int x, int y, int xMax, int yMax)
    {
        TileData[] adjList = new TileData[6];
        
        //Returns Vector2Int with coordinates of the adjacent cell in desired direction
        AdjacentCellVectors adjVec = new AdjacentCellVectors();

        adjList[(int)LEFT] = GetAdjTile(adjVec.GetLeft(x, y), xMax, yMax);
        adjList[(int)TOPLEFT] = GetAdjTile(adjVec.GetTopLeft(x, y), xMax, yMax);
        adjList[(int)TOPRIGHT] = GetAdjTile(adjVec.GetTopRight(x, y), xMax, yMax);
        adjList[(int)RIGHT] = GetAdjTile(adjVec.GetRight(x, y), xMax, yMax);
        adjList[(int)BOTTOMRIGHT] = GetAdjTile(adjVec.GetBottomRight(x, y), xMax, yMax);
        adjList[(int)BOTTOMLEFT] = GetAdjTile(adjVec.GetBottomLeft(x, y), xMax, yMax);

        return adjList;
    }


    /// <summary>
    /// Returns specific adjacent TileData out of array of all TileData in map
    /// </summary>
    /// <param name="coord">Coordinates of the adjacent tile to be returned</param>
    /// <param name="xMax">amount of tiles in x direction</param>
    /// <returns>Returns TileData or null depending if there exists tile at position</returns>
    private TileData GetAdjTile(Vector2Int adjTileCoord, int xMax, int yMax)
    {
        if ( (adjTileCoord.x >= 0 && adjTileCoord.x < xMax) && (adjTileCoord.y >= 0 && adjTileCoord.y < yMax))
        {
            int index = GetArrayIndex(adjTileCoord.x, adjTileCoord.y, xMax);
            return _grid[index];
        }
        else
        {
            return null;
        }
    }


    /// <summary>
    /// Calculates one dimensional index of the flattened 2D-Array
    /// </summary>
    /// <param name="x">x coordinate</param>
    /// <param name="y">y coordinate</param>
    /// <param name="width">amount of tiles in x direction</param>
    /// <returns></returns>
    private int GetArrayIndex(int x, int y, int width)
    {
        return x + y * width;
    }
}
