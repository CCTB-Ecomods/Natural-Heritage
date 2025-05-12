using UnityEngine;
using Random = UnityEngine.Random;

public class TileMeshUpdater : MonoBehaviour
{
    public GameObject defaultMesh;

    private TileData _td;
    public GameObject currentInstance { get; private set; }
    private Intensity _previousIntensity;
    private TileType _previousType;

    public void UpdateMesh()
    {
        bool updateMesh = false;
        if (_td == null || currentInstance == null) //first iteration, always update Mesh
        {
            _td = gameObject.GetComponent<TileData>();
            currentInstance = defaultMesh;
            updateMesh = true;
        }

        GameObject[] meshes = _td.type.GetMeshes();
        if (meshes.Length == 0) return;
        GameObject chosenMesh = meshes[Random.Range(0, meshes.Length)];

        if (_previousIntensity != _td.intensity || _previousType != _td.type.Type) //are there any changes?
            updateMesh = true;

        if (updateMesh) {
            _previousIntensity = _td.intensity;
            _previousType = _td.type.Type;

            Destroy(currentInstance);
            currentInstance = Instantiate(chosenMesh, transform);
            if(!((_td.type.Type == TileType.FIELD && (_td.intensity == Intensity.MEDIUM || _td.intensity == Intensity.HIGH))||(_td.type.Type == TileType.RIVER) || (_td.type.Type == TileType.CITY)))
                currentInstance.transform.Rotate(Vector3.up, 60 * Random.Range(0,6));
        }
    }
}