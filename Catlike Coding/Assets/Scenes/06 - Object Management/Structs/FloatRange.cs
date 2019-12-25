
using System;
using UnityEngine;

[System.Serializable]
public struct FloatRange
{
    public float min, max;

    public float RandomValueInRange
    {
        get
        {
            return UnityEngine.Random.Range(min, max);
        }
    }
}
