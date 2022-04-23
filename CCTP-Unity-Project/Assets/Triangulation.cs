using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class Triangulation
{
    public static bool AreEdgesIntersecting(Edge edge1, Edge edge2)
    {
        Vector2 line1_point1 = new Vector2(edge1.v1.position.x, edge1.v1.position.z);
        Vector2 line1_point2 = new Vector2(edge1.v2.position.x, edge1.v2.position.z);

        Vector2 line2_point1 = new Vector2(edge2.v1.position.x, edge2.v1.position.z);
        Vector2 line2_point2 = new Vector2(edge2.v2.position.x, edge2.v2.position.z);

        bool isIntersecting = LinearAlgebra.AreLinesIntersecting(line1_point1, line1_point2, line2_point1, line2_point2, true);

        return isIntersecting;
    }

    public static List<Triangle> IncrementalPointsTriangulation(List<Vertex> points)
    {
        List<Triangle> triangles = new List<Triangle>();

        points = points.OrderBy(n => n.position.x).ToList();

        Triangle newTriangle = new Triangle(points[0].position, points[1].position, points[2].position);

        triangles.Add(newTriangle);

        List<Edge> edges = new List<Edge>();


        edges.Add(new Edge(newTriangle.v1, newTriangle.v2));
        edges.Add(new Edge(newTriangle.v2, newTriangle.v3));
        edges.Add(new Edge(newTriangle.v3, newTriangle.v1));

        for(int i = 3; i < points.Count; i++)
        {
            Vector3 currentPoint = points[i].position;

            List<Edge> newEdges = new List<Edge>();

            for (int j = 0; j < edges.Count; j++)
            {
                Edge currentEdge = edges[j];

                Vector3 midPoint = (currentEdge.v1.position + currentEdge.v2.position) / 2f;

                Edge edgeToMidpoint = new Edge(currentPoint, midPoint);

                bool canSeeEdge = true;

                for (int k = 0; k < edges.Count; k++)
                {
                    if (k == j) continue;

                    if(AreEdgesIntersecting(edgeToMidpoint, edges[k]))
                    {
                        canSeeEdge = false;
                        break;
                    }
                }

                if(canSeeEdge)
                {
                    Edge edgeToPoint1 = new Edge(currentEdge.v1, new Vertex(currentPoint));
                    Edge edgeToPoint2 = new Edge(currentEdge.v2, new Vertex(currentPoint));

                    newEdges.Add(edgeToPoint1);
                    newEdges.Add(edgeToPoint2);

                    Triangle newTri = new Triangle(edgeToPoint1.v1, edgeToPoint1.v2, edgeToPoint2.v1);

                    triangles.Add(newTri);
                }
            }
        }
        return triangles;
    }
}
