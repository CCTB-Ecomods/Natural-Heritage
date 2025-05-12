using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[CustomEditor(typeof(TileMapEditor))]
public class TileMapEditorInspector : Editor
{
    private TileMapEditor mapEditor;
    private GridGenerator generator;
    private selected_dictionary selection;
    private IGameLogic gameLogic;

    private bool showWarning;
    private PressedButton prevButton = PressedButton.NONE;
    

    void OnEnable()
    {
        mapEditor = (TileMapEditor)target;
        generator = GameObject.FindWithTag("GridGenerator").GetComponent<GridGenerator>();
        selection = GameObject.FindWithTag("EventSystem").GetComponent<selected_dictionary>();
        gameLogic = GameObject.FindWithTag("GameManager").GetComponent<IGameLogic>();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (showWarning)
        {
            GUILayout.Space(5f);
            DisplayWarning();
            GUILayout.Space(5f);
        }
        else
        {
            GUILayout.Space(5f);
            DisplayEditButton();
            GUILayout.Space(5f);
            DisplayRefreshButton();
            GUILayout.Space(5f);
            DisplaySaveButton();
            GUILayout.Space(2f);
            DisplayExportButton();
        }
    }

    private void DisplayWarning()
    {
        GUILayout.Label("Prefab with name \"" + target.name + "\" already exists. Overwrite?");

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Yes"))
        {
            switch (prevButton)
            {
                case PressedButton.SAVE:
                    SaveAsPrefab(mapEditor.gameObject, mapEditor.gameObject.name, false);
                    DestroyImmediate(mapEditor.gameObject);
                    break;
                case PressedButton.EXPORT:
                    SaveAsPrefab(mapEditor.gameObject, mapEditor.gameObject.name, true);
                    DestroyImmediate(mapEditor.gameObject);
                    break;
                case PressedButton.REFRESH:
                    GameObject newMap = generator.RefreshTileMapObj(mapEditor);
                    SaveAsPrefab(newMap, newMap.name, false);
                    break;
                default:
                    throw new Exception("no valid button pressed beforehand");
            }

            showWarning = false;
            prevButton = PressedButton.NONE;
        }
        if (GUILayout.Button("No"))
        {
            showWarning = false;
            prevButton = PressedButton.NONE;
        }
        GUILayout.EndHorizontal();
    }

    private void DisplayEditButton()
    {
        if (GUILayout.Button("Set as edited"))
        {
            gameLogic.SetWorld(mapEditor.gameObject.GetComponent<WorldData>());
        }
    }

    private void DisplayRefreshButton()
    {
        if (GUILayout.Button("Refresh"))
        {             
            showWarning = SearchTileMapPrefabWithName(mapEditor.gameObject.name);
            prevButton = PressedButton.REFRESH;

            if (!showWarning)
            {
                GameObject newMap = generator.RefreshTileMapObj(mapEditor);
                SaveAsPrefab(newMap, newMap.name, false);
            }
        }

    }

    private void DisplaySaveButton()
    {
        try
        {
            if (GUILayout.Button("Save \"" + mapEditor.gameObject.name + "\" as Prefab"))
            {
                showWarning = SearchTileMapPrefabWithName(mapEditor.gameObject.name);
                prevButton = PressedButton.SAVE;

                if (!showWarning)
                {
                    SaveAsPrefab(mapEditor.gameObject, mapEditor.gameObject.name, false);
                    DestroyImmediate(mapEditor.gameObject);
                }
            }
        }
        catch { }
    }

    private void DisplayExportButton()
    {
        try
        {
            if (GUILayout.Button("Export \"" + mapEditor.gameObject.name + "\" as Prefab"))
            {
                showWarning = SearchTileMapPrefabWithName(mapEditor.gameObject.name);
                prevButton = PressedButton.EXPORT;

                if (!showWarning)
                {
                    SaveAsPrefab(mapEditor.gameObject, mapEditor.gameObject.name, true);
                    DestroyImmediate(mapEditor.gameObject);
                }
            }
        }
        catch { }
    }

    private bool SearchTileMapPrefabWithName(string name)
    {
        string path = "Assets/Prefabs/TileMaps/" + name + ".prefab";
        var obj = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));

        return obj != null;
    }

    private void SaveAsPrefab(GameObject obj, string prefabName, bool destroyEditorComponent)
    {
        selection.deselectAll();

        bool saveSucces;
        string path = "Assets/Prefabs/TileMaps/" + prefabName + ".prefab";

        if (destroyEditorComponent)
            DestroyImmediate(mapEditor);

        try
        {
            PrefabUtility.SaveAsPrefabAsset(obj, path, out saveSucces);
        } 
        catch 
        {
            string directoryPath = "Assets/Prefabs/TileMaps/";
            Debug.LogWarning("Created new path:  \'" + directoryPath + "\'");
            Directory.CreateDirectory(directoryPath);
            string tileMapPath = directoryPath + prefabName + ".prefab";
            PrefabUtility.SaveAsPrefabAsset(obj, tileMapPath, out saveSucces);
        }

        if (saveSucces)
        {
            Debug.Log("Prefab saved status: " + saveSucces);
        }
        else
        {
            throw new Exception("Exporting tile map as prefab not sucessful!");
        }
    }
}

public enum PressedButton
{
    NONE,
    SAVE,
    EXPORT,
    REFRESH
}
