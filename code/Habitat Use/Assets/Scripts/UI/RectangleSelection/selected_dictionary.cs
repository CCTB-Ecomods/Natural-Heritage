using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selected_dictionary : MonoBehaviour
{
    public GameObject tileSettings;
    public Dictionary<int, GameObject> selectedTable = new Dictionary<int, GameObject>();

    /// <summary>
    /// add one selected gameObject
    /// </summary>
    /// <param name="go"></param>
    public void addSelected(GameObject go)
    {
        addSelectedWithoutUIUpdate(go);
        updateTileSettings();
    }


    public void addSelectedWithoutUIUpdate(GameObject go)
    {
        if (!go.CompareTag("Hexagon")) return; //you should only be able to select Hexagons
        int id = go.GetInstanceID();

        if (!(selectedTable.ContainsKey(id)))
        {
            selectedTable.Add(id, go);
            go.AddComponent<selection_component>();
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            deselect(id);
        }
    }

    /// <summary>
    /// add a list of selected gameobjects. Updates UI only once
    /// </summary>
    /// <param name="gos"></param>
    public void addSelected(List<GameObject> gos)
    {
        gos.ForEach(go => { 
            if (!go.CompareTag("Hexagon")) gos.Remove(go);
        });
        gos.ForEach(go => {
            int id = go.GetInstanceID();
            if (!(selectedTable.ContainsKey(id)))
            {
                selectedTable.Add(id, go);
                go.AddComponent<selection_component>();
            }
            else if (Input.GetKey(KeyCode.LeftControl))
            {
                deselect(id);
            }
        });
        updateTileSettings();
    }

    public void deselect(int id)
    {
        Destroy(selectedTable[id].GetComponent<selection_component>());
        selectedTable.Remove(id);
        updateTileSettings();
    }

    public void deselectAll()
    {
        foreach(KeyValuePair<int,GameObject> pair in selectedTable)
        {
            if(pair.Value != null)
            {
                Destroy(selectedTable[pair.Key].GetComponent<selection_component>());
            }
        }
        selectedTable.Clear();
        updateTileSettings();
    }

    public void updateTileSettings()
    {
        tileSettings.GetComponent<TileSettingsScript>().updateData(selectedTable);
    }
}
