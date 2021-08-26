using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathRenderer : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lineRenderer;

    public void ShowPath(List<Vector3> pathPoints)
    {
        var allPoints = GeneratePoints(pathPoints);
        
        lineRenderer.positionCount = allPoints.Length;
        lineRenderer.SetPositions(allPoints);
    }

    private Vector3[] GeneratePoints(List<Vector3> keyPoints, int segments = 5)
    {
        var points = new Vector3[(keyPoints.Count - 1) * segments + keyPoints.Count];

        for(var i = 1; i < keyPoints.Count; i++)
        {
            points[(i - 1) * segments + i - 1] = new Vector3(keyPoints[i-1].x, 0, keyPoints[i-1].z);

            for (int j = 1; j <= segments; j++)
            {
                var x = keyPoints[i - 1].x;
                var z = keyPoints [i - 1].z;
                var dx = (keyPoints[i].x - keyPoints[i - 1].x) / segments;
                var dz = (keyPoints[i].z - keyPoints[i - 1].z) / segments;

                points[(i - 1) * segments + j + i - 1] = new Vector3 (x + dx * j, 0, z + dz * j);
            }
        }

        points[(keyPoints.Count - 1) * segments + keyPoints.Count - 1] =
            new Vector3(keyPoints [keyPoints.Count-1].x, 0, keyPoints [keyPoints.Count-1].z);

        return points;
    }

    public void Clear()
    {
        lineRenderer.positionCount = 0;
    }
}
