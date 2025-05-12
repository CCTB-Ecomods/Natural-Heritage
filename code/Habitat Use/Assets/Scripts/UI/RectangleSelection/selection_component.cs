using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selection_component : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var hexUI = GetComponent<HexagonUi>();
        if (hexUI != null)
            hexUI.select(true);
    }

    private void OnDestroy()
    {
        var hexUI = GetComponent<HexagonUi>();
        if (hexUI != null)
            hexUI.select(false);
    }
}
