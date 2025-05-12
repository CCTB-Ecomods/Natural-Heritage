using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class GridGenerator : MonoBehaviour
{
    public GameObject tileMap;
    public GameObject cellPrefab;
    public int width, height;

    public void CreateTileMapObj()
    {
        GameObject gridObj = (GameObject) PrefabUtility.InstantiatePrefab(tileMap);
        gridObj.name = "NewTileMap";
        gridObj.tag = "Map";
        var newEditor = gridObj.AddComponent<TileMapEditor>();
        newEditor.CreateGrid(cellPrefab, width, height, null);
    }

    public GameObject RefreshTileMapObj(TileMapEditor tileMapEditor)
    {
        SaveData[] save = tileMapEditor.SaveData();
        string objName = tileMapEditor.gameObject.name;
        int oldWidth = tileMapEditor.Width;
        int oldHeight = tileMapEditor.Height;

        DestroyImmediate(tileMapEditor.gameObject);
        
        GameObject gridObj = (GameObject) PrefabUtility.InstantiatePrefab(tileMap);
        gridObj.name = objName;
        gridObj.tag = "Map";
        var newEditor = gridObj.AddComponent<TileMapEditor>();
        newEditor.CreateGrid(cellPrefab, oldWidth, oldHeight, save);
    
        //foreach (var tile in gridObj.GetComponent<WorldData>().tiles)
        //{
        //    var updater = tile.gameObject.GetComponent<TileMeshUpdater>();
        //    updater.InitMeshInstances();
        //}

        return gridObj;
    }
}
