using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BoidHelper
{
    const int numViewDirections = 300;
    public static readonly Vector3[] directions;

    static BoidHelper()
    {
        directions = new Vector3[numViewDirections];
        
        //distance between each point 
        float goldenRatio = (1 + Mathf.Sqrt(5)) / 2;
        float angleIncrement = Mathf.PI * 2 * goldenRatio;

        for (int i = 0; i < numViewDirections; i++)
        {
            float t = (float)i / numViewDirections;
            float inclination = Mathf.Acos(1 - 2 * t);
            float angle = angleIncrement * i;
            
            //calculate sphere points for ray cast
            float x = Mathf.Sin(inclination) * Mathf.Cos(angle);
            float y = Mathf.Sin(inclination) * Mathf.Sin(angle);
            float z = Mathf.Cos(inclination);
            directions[i] = new Vector3(x, y, z);
        }
    }
}