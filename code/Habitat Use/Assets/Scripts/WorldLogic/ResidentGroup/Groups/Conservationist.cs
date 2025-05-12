using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conservationist : ResidentGroup
{
    private LandUseValue _idealLandUse;

    public Conservationist(string name, LandUseValue idealLandUse) : base (name) 
    {
        _idealLandUse = idealLandUse;
    }

    protected override LandUseValue GetIdealLandscapeValues()
    {
        return _idealLandUse;
    }
}
