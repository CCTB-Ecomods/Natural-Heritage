using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandUseValue : IEnumerable<float>
{    
    private float[] values = new float[3];

    public float NatureValue 
    { 
        get { return values[0]; }
        set { values[0] = value; } 
    }
    public float TouristValue
    {
        get { return values[1]; }
        set { values[1] = value; }
    }
    public float IndustryValue
    {
        get { return values[2]; }
        set { values[2] = value; }
    }

    public LandUseValue(float nature, float tourist, float industry)
    {
        NatureValue = nature;
        TouristValue = tourist;
        IndustryValue = industry;
    }

    public float GetSum()
    {
        float sum = 0;

        foreach(var value in values)
        {
            sum += value;
        }

        return sum;
    }

    public IEnumerator<float> GetEnumerator()
    {
        foreach (float f in values)
        {
            yield return f;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
