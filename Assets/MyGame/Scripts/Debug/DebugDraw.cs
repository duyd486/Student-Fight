using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugDraw : MonoBehaviour
{
    public static DebugDraw Instance { get; private set; }

    public List<Sphere> SphereList;
    public List<Line> LineList;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        SphereList = new List<Sphere>();
        LineList = new List<Line>();
    }

    public void DrawLine(Vector3 from, Vector3 to, Color color)
    {
        Line line = new Line();
        line.lineStart = from;
        line.lineColor = color;
        line.lineEnd = to;
        LineList.Add(line);
    }

    public void DrawSphere(Vector3 position, float radius, Color color)
    {
        Sphere sphere = new Sphere();
        sphere.spherePosition = position;
        sphere.sphereRadius = radius;
        sphere.sphereColor = color;
        SphereList.Add(sphere);
    }

    private void OnDrawGizmos()
    {
        if (SphereList?.Count > 0)
        {
            foreach (Sphere sphere in SphereList)
            {
                Gizmos.color = sphere.sphereColor;
                Gizmos.DrawWireSphere(sphere.spherePosition.Value, sphere.sphereRadius);
            }
        }
        if (LineList?.Count > 0)
        {
            foreach(Line line in LineList)
            {
                Gizmos.color = line.lineColor;
                Gizmos.DrawLine(line.lineStart.Value, line.lineEnd.Value);
            }
        }
        SphereList?.Clear();
        LineList?.Clear();
    }
}
public class Sphere
{
    public Vector3? spherePosition;
    public float sphereRadius;
    public Color sphereColor;
}
public class Line
{
    public Vector3? lineStart, lineEnd;
    public Color lineColor;
}