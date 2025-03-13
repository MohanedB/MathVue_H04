using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldLimits : MonoBehaviour
{
    public float xMin = -5f;
    public float xMax = 5f;
    public float zMin = -3f;
    public float zMax = 3f;

    void Start()
    {
        // Debugging: Show the limits in the console
        Debug.Log($"Field Limits: xMin={xMin}, xMax={xMax}, zMin={zMin}, zMax={zMax}");
    }
}
