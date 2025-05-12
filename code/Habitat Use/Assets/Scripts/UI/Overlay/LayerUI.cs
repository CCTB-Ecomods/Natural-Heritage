using UnityEngine;

public class LayerUI : MonoBehaviour
{
    static public OverlayLayers activeLayer = OverlayLayers.NONE;
    static private WorldData worldData;

    private void Start()
    {
        worldData = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameLogic>().GetWorld();
    }

    public void deactivateAllLayers()
    {
        foreach (TileData TileData in worldData.tiles)
        {
            ActivateMapLayer(true);
            TileData.gameObject.GetComponent<HexagonUi>().activateBiodiversityOverlay(false);
            TileData.gameObject.GetComponent<HexagonUi>().activateProductivityOverlay(false);
        }
        activeLayer = OverlayLayers.NONE;
    }

    public void enableBiodiversityLayer()
    {
        ActivateMapLayer(false);

        if (activeLayer != OverlayLayers.BIODIVERSITY)
        {
            foreach (TileData TileData in worldData.tiles)
            {
                TileData.gameObject.GetComponent<HexagonUi>().activateProductivityOverlay(false);
                TileData.gameObject.GetComponent<HexagonUi>().activateBiodiversityOverlay(true);
                activeLayer = OverlayLayers.BIODIVERSITY;
            }
        }
        else
        {
            foreach (TileData TileData in worldData.tiles)
            {
                TileData.gameObject.GetComponent<HexagonUi>().activateBiodiversityOverlay(false);
                ActivateMapLayer(true);
                activeLayer = OverlayLayers.NONE;
            }
        }
    }

    public void enableProductivityLayer()
    {
        ActivateMapLayer(false);
        
        if (activeLayer != OverlayLayers.PRODUCTIVITY)
        {
            foreach (TileData TileData in worldData.tiles)
            {
                TileData.gameObject.GetComponent<HexagonUi>().activateBiodiversityOverlay(false);
                TileData.gameObject.GetComponent<HexagonUi>().activateProductivityOverlay(true);
                activeLayer = OverlayLayers.PRODUCTIVITY;
            }
        }
        else
        {
            foreach (TileData TileData in worldData.tiles)
            {
                TileData.gameObject.GetComponent<HexagonUi>().activateProductivityOverlay(false);
                ActivateMapLayer(true);
                activeLayer = OverlayLayers.NONE;
            }
        }
    }
    
    private void ActivateMapLayer(bool b)
    {
        foreach (TileData TileData in worldData.tiles)
        {
            var currentInstance = TileData.gameObject.GetComponent<TileMeshUpdater>().currentInstance;
            currentInstance.SetActive(b);
        }
    }
}

public enum OverlayLayers
{
    NONE,
    BIODIVERSITY,
    PRODUCTIVITY
}
