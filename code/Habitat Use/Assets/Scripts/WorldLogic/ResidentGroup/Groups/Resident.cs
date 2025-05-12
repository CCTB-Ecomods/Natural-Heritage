using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resident : ResidentGroup
{
    private LandUseValue _idealLandUse;

    public Resident(string name, LandUseValue idealLandUse) : base(name)
    {
        _idealLandUse = idealLandUse;
    }

    protected override LandUseValue GetIdealLandscapeValues()
    {
        return _idealLandUse;
    }
}
