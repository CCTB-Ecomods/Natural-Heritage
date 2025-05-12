using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EditorLogic : MonoBehaviour, IGameLogic
{
    public int time;
    public WorldData world;
    public void SetWorld(WorldData w) { world = w; }
    public WorldData GetWorld() { return world; }

    public void OnRoundChange()
    {
        CalculateTileData();
        UpdateTileMeshes();
        UpdateUI();
    }

    private void CalculateTileData()
    {
        world.areas = Nature.CalculateAreas(world.tiles);
    }

    private void UpdateTileMeshes()
    {
        foreach (TileData tileData in world.tiles)
        {
            tileData.gameObject.GetComponent<TileMeshUpdater>().UpdateMesh();
        }
    }

    private void UpdateUI()
    {
        HexagonUi.onUpdate();
        //ResidentApprovalUi.onUpdate();
    }

    public ElectionSystem GetElectionSystem()
    {
        throw new System.NotImplementedException();
    }

    public void AddRoundChangeEventListener(UnityAction call)
    {
        throw new System.NotImplementedException();
    }
}
