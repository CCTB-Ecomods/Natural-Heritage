using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(GridGenerator))]
public class GridInspector : Editor
{
    private GridGenerator _generator;

    void OnEnable()
    {
        _generator = (GridGenerator)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(5f);
        if (GUILayout.Button("Generate Hex Grid"))
        {
            _generator.CreateTileMapObj();
        }
    }

    private TileMapEditor[] GetAllTileMapEditors()
    {
        throw new System.NotImplementedException();
    }
}
