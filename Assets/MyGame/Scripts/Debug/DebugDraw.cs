using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugDraw : MonoBehaviour
{
    public static DebugDraw Instance { get; private set; }

    private void Start()
    {
        Instance = this;
    }

    private Vector3? lineStart, lineEnd;
    private Color lineColor;

    private Vector3? spherePosition;
    private float sphereRadius;
    private Color sphereColor;

    /// <summary> Vẽ 1 line duy nhất trong Scene View mỗi frame </summary>
    public static void DrawLine(Vector3 from, Vector3 to, Color color)
    {
        Instance.lineStart = from;
        Instance.lineEnd = to;
        Instance.lineColor = color;
    }

    /// <summary> Vẽ 1 wire sphere duy nhất mỗi frame </summary>
    public static void DrawSphere(Vector3 position, float radius, Color color)
    {
        Instance.spherePosition = position;
        Instance.sphereRadius = radius;
        Instance.sphereColor = color;
    }

    private void OnDrawGizmos()
    {
        if (lineStart.HasValue && lineEnd.HasValue)
        {
            Gizmos.color = lineColor;
            Gizmos.DrawLine(lineStart.Value, lineEnd.Value);
        }

        if (spherePosition.HasValue)
        {
            Gizmos.color = sphereColor;
            Gizmos.DrawWireSphere(spherePosition.Value, sphereRadius);
        }

        // Clear sau mỗi lần vẽ
        lineStart = null;
        lineEnd = null;
        spherePosition = null;
    }
}
