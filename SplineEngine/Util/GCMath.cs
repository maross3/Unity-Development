using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GCMath 
{
    // todo, implement this in a KD Tree
    public static int FindClosestPoint(Vector3[] points, Vector3 target)
    {
        if (points.Length == 0)
        {
            return -1;
        }

        var closestIndex = 0;
        var closestDistance = Vector3.Distance(points[0], target);

        for (var i = 1; i < points.Length; i++)
        {
            var distance = Vector3.Distance(points[i], target);
            if (!(distance < closestDistance)) continue;
            
            closestIndex = i;
            closestDistance = distance;
        }

        return closestIndex;
    }
    
    public static Vector3 Flatten(Vector3 v)
    {
        return new Vector3(v.x, 0f, v.z);
    }
}
