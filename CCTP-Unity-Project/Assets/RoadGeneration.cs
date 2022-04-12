using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class RoadGeneration : MonoBehaviour
{
    [SerializeField] private GameObject roadPoint1, roadPoint2;

    //[Range(0.5f, 1.5f)] private float spacing = 1.0f;
    [SerializeField] private float roadWidth = 1.0f;

    void Start()
    {
        Vector3[] _points = new Vector3[2];

        _points[0] = roadPoint1.transform.position;
        _points[1] = roadPoint2.transform.position;

        this.GetComponent<MeshFilter>().mesh = CreateRoadMesh(_points);
    }

    /*private void GenerateRoad()
    {
        // linearly interpret between two points
        // spawn vertex at that point
    }

    private Vector3 GenerateRoadPoint(Vector3 a, Vector3 b, float t)
    {
        return a + (b - a) * t;
    }*/

    private Mesh CreateRoadMesh(Vector3[] points)
    {
        Vector3[] vertices  = new Vector3[points.Length * 2];
        int[]     triangles = new int[(points.Length - 1) * 6];

        int vertexIndex   = 0;
        int triangleIndex = 0;

        for (int i = 0; i < points.Length; i++)
        {
            /*Vector3 forward = Vector3.zero;
            if (i < points.Length - 1)
            {
                forward += points[i - 1] - points[i];
            }
            if(i > 0)
            {
                forward += points[i] - points[i - 1];
            }
            forward.Normalize();*/

            Vector3 forward = Vector3.zero;

            vertices[vertexIndex]     = points[i] + 0.5f * roadWidth * forward;
            vertices[vertexIndex + 1] = points[i] - 0.5f * roadWidth * forward;

            if(i < points.Length - 1)
            {
                triangles[triangleIndex]     = vertexIndex;
                triangles[triangleIndex + 1] = vertexIndex + 2;
                triangles[triangleIndex + 2] = vertexIndex + 1;

                triangles[triangleIndex + 3] = vertexIndex + 1;
                triangles[triangleIndex + 4] = vertexIndex + 2;
                triangles[triangleIndex + 5] = vertexIndex + 3;
            }

            vertexIndex   += 2;
            triangleIndex += 6;
        }

        Mesh mesh      = new Mesh();
        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        return mesh;
    }

    void Update()
    {
        
    }
}
