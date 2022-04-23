using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delaunay
{
    public static bool IsTriangleOrientedClockwise(Vector2 p1, Vector2 p2, Vector2 p3)
    {
        bool isClockWise = true;

        float determinant = p1.x * p2.y + p3.x * p1.y + p2.x * p3.y - p1.x * p3.x - p3.x * p2.y - p2.x * p1.y;

        if (determinant > 0f) isClockWise = false;

        return isClockWise;
    }

    public static float PointPositonRelativeToCircle(Vector2 aVec, Vector2 bVec, Vector2 cVec, Vector2 dVec)
    {
        float a = aVec.x - dVec.x;
        float d = bVec.x - dVec.x;
        float g = cVec.x - dVec.x;

        float b = aVec.y - dVec.y;
        float e = bVec.y - dVec.y;
        float h = cVec.y - dVec.y;

        float c = a * a + b * b;
        float f = d * d + e * e;
        float i = g * g + h * h;

        float determinant = (a * e * i) + (b * f * g) + (c * d * h) - (g * e * c) - (h * f * a) - (i * d * b);

        return determinant;
    }

    public static void OrientTrianglesClockwise(List<Triangle> triangles)
    {
        foreach (var triangle in triangles)
        {
            Triangle tri = triangle;

            Vector2 v1 = new Vector2(tri.v1.position.x, tri.v1.position.z);
            Vector2 v2 = new Vector2(tri.v2.position.x, tri.v2.position.z);
            Vector2 v3 = new Vector2(tri.v3.position.x, tri.v3.position.z);

            if (!IsTriangleOrientedClockwise(v1, v2, v3)) tri.ChangeOrientation();
        }
    }

    public static List<HalfEdge> TransformFromTriangleToHalfEdge(List<Triangle> triangles)
    {
        OrientTrianglesClockwise(triangles);

        List<HalfEdge> halfEdges = new List<HalfEdge>(triangles.Count * 3);

        foreach (var triangle in triangles)
        {
            HalfEdge he1 = new HalfEdge(triangle.v1);
            HalfEdge he2 = new HalfEdge(triangle.v2);
            HalfEdge he3 = new HalfEdge(triangle.v3);

            he1.nextEdge = he2;
            he2.nextEdge = he3;
            he3.nextEdge = he1;

            he1.previousEdge = he3;
            he2.previousEdge = he1;
            he3.previousEdge = he2;

            he1.vertex.halfEdge = he2;
            he2.vertex.halfEdge = he3;
            he3.vertex.halfEdge = he1;

            triangle.halfEdge = he1;

            he1.triangle = triangle;
            he2.triangle = triangle;
            he3.triangle = triangle;

            halfEdges.Add(he1);
            halfEdges.Add(he2);
            halfEdges.Add(he3);
        }

        for (int i = 0; i < halfEdges.Count; i++)
        {
            HalfEdge he = halfEdges[i];

            Vertex goingToVertex = he.vertex;
            Vertex goingFromVertex = he.previousEdge.vertex;

            for (int j = 0; j < halfEdges.Count; j++)
            {
                if (j == i) continue;

                HalfEdge heOpposite = halfEdges[j];

                if(goingFromVertex.position == heOpposite.vertex.position && goingToVertex.position == heOpposite.previousEdge.vertex.position)
                {
                    he.opositeEdge = heOpposite;
                    break;
                }
            }
        }
        return halfEdges;
    }

    public static List<Triangle> TriangulateByFlippingEdges(List<Vector3> sites)
    {
        List<Vertex> vertices = new List<Vertex>();

        foreach (var site in sites)
        {
            vertices.Add(new Vertex(site));
        }

        List<Triangle> triangles = Triangulation.IncrementalPointsTriangulation(vertices);

        List<HalfEdge> halfEdges = TransformFromTriangleToHalfEdge(triangles);

        int safety = 0;

        int flippedEdges = 0;

        while(true)
        {
            safety++;

            if(safety > 100000)
            {
                Debug.Log("AHHHHH");
                break;
            }

            bool hasFlippedEdge = false;

            for (int i = 0; i < halfEdges.Count; i++)
            {
                HalfEdge thisEdge = halfEdges[i];

                if (thisEdge.opositeEdge == null) continue;

                Vertex a = thisEdge.vertex;
                Vertex b = thisEdge.nextEdge.vertex;
                Vertex c = thisEdge.previousEdge.vertex;
                Vertex d = thisEdge.opositeEdge.vertex;

                Vector2 aPos = a.GetPos2D_XZ();
                Vector2 bPos = b.GetPos2D_XZ();
                Vector2 cPos = c.GetPos2D_XZ();
                Vector2 dPos = d.GetPos2D_XZ();

                if (PointPositonRelativeToCircle(bPos, cPos, dPos, aPos) < 0f) continue;

                flippedEdges++;

                hasFlippedEdge = true;

                Flip
            }
        }
    }
}
