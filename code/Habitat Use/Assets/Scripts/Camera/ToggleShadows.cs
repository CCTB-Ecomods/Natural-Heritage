using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleShadows : MonoBehaviour
{

    static private Light _light;
    static private LightShadows _orig;
    
    // Start is called before the first frame update
    void Start()
    {
        _light = GetComponent<Light>();
        _orig = _light.shadows;
    }

    public static bool togglePerformance()
    {
        if (_light.shadows == LightShadows.None)
            _light.shadows = _orig;
        else
            _light.shadows = LightShadows.None;

        return _light.shadows == LightShadows.None;
    }
}
