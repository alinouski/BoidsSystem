using UnityEngine;
using System.Collections;

[System.Serializable]
public struct BoidsBehaviourPreset 
{
    [Range(0,1)]
    public float aligment;
    [Range(0, 1)]
    public float cohesion;
    [Range(0, 1)]
    public float separation;
}
