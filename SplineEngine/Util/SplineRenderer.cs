using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SplineRenderer
{
    
    public static LineRenderer CreateSplineRenderer(this GameObject obj, Vector3[] points)
    {
        var lineRenderer = obj.AddComponent<LineRenderer>();

        lineRenderer.positionCount = points.Length;
        lineRenderer.SetPositions(points);

        // Customize other properties of the Line Renderer as desired
        lineRenderer.startColor = Color.magenta;
        lineRenderer.endColor = Color.magenta;
        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.2f;
        return lineRenderer;
    }

    public static void UpdateSplineRenderer(this LineRenderer lineRenderer, Vector3[] points)
    {
        lineRenderer.positionCount = points.Length;
        lineRenderer.SetPositions(points);
    }
}
