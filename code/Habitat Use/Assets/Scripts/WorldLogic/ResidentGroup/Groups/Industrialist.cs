using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Industrialist : ResidentGroup
{
    private LandUseValue _idealLandUse;

    public Industrialist(string name, LandUseValue idealLandUse) : base(name)
    {
        _idealLandUse = idealLandUse;
    }

    protected override LandUseValue GetIdealLandscapeValues()
    {
        return _idealLandUse;
    }
}

