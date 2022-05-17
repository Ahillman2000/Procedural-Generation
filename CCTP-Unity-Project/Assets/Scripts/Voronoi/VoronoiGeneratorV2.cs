using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoronoiEdge
{
    public Vector3 v1;
    public Vector3 v2;

    public Vector3 sitePosition;

    public VoronoiEdge(Vector3 v1, Vector3 v2, Vector3 sitePositon)
    {
        this.v1 = v1;
        this.v2 = v2;
        this.sitePosition = sitePositon;
    }
}

public class VoronoiCell
{
    public Vector3 sitePosition;

    public List<VoronoiEdge> edges = new List<VoronoiEdge>();

    public VoronoiCell(Vector3 sitePositition)
    {
        this.sitePosition = sitePositition;
    }
}

public class Geometry
{
    public static Vector2 CalculateCircleCenter(Vector2 p1, Vector2 p2, Vector2 p3)
    {
        Vector2 center = new Vector2();

        float ma = (p2.y - p1.y) / (p2.x - p1.x);
        float mb = (p3.y - p2.y) / (p3.x - p2.x);

        center.x = (ma * mb * (p1.y - p3.y) + mb * (p1.x + p2.x) - ma * (p2.x + p3.x)) / (2 * (mb - ma));
        center.y = (-1 / ma) * (center.x - (p1.x + p2.x) / 2) + (p1.y + p2.y) / 2;

        return center;
    }
}

public class DelaunayToVoronoi
{
    public static List<VoronoiCell> GenerateVoronoiDiagram(List<Vector3> sites)
    {
        List<Triangle> triangles = Delaunay.TriangulateByFlippingEdges(sites);

        List<VoronoiEdge> voronoiEdges = new List<VoronoiEdge>();

        for (int i = 0; i < triangles.Count; i++)
        {
            Triangle t = triangles[i];

            HalfEdge e1 = t.halfEdge;
            HalfEdge e2 = e1.nextEdge;
            HalfEdge e3 = e2.nextEdge;

            /*Vector3 v1 = e1.vertex.position;
            Vector3 v2 = e2.vertex.position;
            Vector3 v3 = e3.vertex.position;*/

            Vector2 v1 = new Vector2(e1.vertex.position.x, e1.vertex.position.z);
            Vector2 v2 = new Vector2(e2.vertex.position.x, e2.vertex.position.z);
            Vector2 v3 = new Vector2(e3.vertex.position.x, e3.vertex.position.z);

            Vector2 center2D = Geometry.CalculateCircleCenter(v1, v2, v3);

            Vector3 voronoiVertex = new Vector3(center2D.x, 0f, center2D.y);

            TryAddVoronoiEdgeFromTriangleEdge(e1, voronoiVertex, voronoiEdges);
            TryAddVoronoiEdgeFromTriangleEdge(e2, voronoiVertex, voronoiEdges);
            TryAddVoronoiEdgeFromTriangleEdge(e3, voronoiVertex, voronoiEdges);
        }

        List<VoronoiCell> voronoiCells = new List<VoronoiCell>();

        for (int i = 0; i < voronoiEdges.Count; i++)
        {
            VoronoiEdge ve = voronoiEdges[i];

            int cellPos = TryFindCellPos(ve, voronoiCells);

            if (cellPos == -1)
            {
                VoronoiCell newCell = new VoronoiCell(ve.sitePosition);

                voronoiCells.Add(newCell);
                newCell.edges.Add(ve);
            }
            else
            {
                voronoiCells[cellPos].edges.Add(ve);
            }
        }

        return voronoiCells;
    }

    private static void TryAddVoronoiEdgeFromTriangleEdge(HalfEdge he, Vector3 voronoiVertex, List<VoronoiEdge> alledges)
    {
        if (he.opositeEdge == null) return;

        HalfEdge heNeighbour = he.opositeEdge;

        /*Vector3 v1 = heNeighbour.vertex.position;
        Vector3 v2 = heNeighbour.nextEdge.vertex.position;
        Vector3 v3 = heNeighbour.nextEdge.nextEdge.vertex.position;*/

        Vector2 v1 = new Vector2(heNeighbour.vertex.position.x, heNeighbour.vertex.position.z);
        Vector2 v2 = new Vector2(heNeighbour.nextEdge.vertex.position.x, heNeighbour.nextEdge.vertex.position.z);
        Vector2 v3 = new Vector2(heNeighbour.nextEdge.nextEdge.vertex.position.x, heNeighbour.nextEdge.nextEdge.vertex.position.z);

        Vector2 center2D = Geometry.CalculateCircleCenter(v1, v2, v3);

        Vector3 voronoiVertexNeighbour = new Vector3(center2D.x, 0f, center2D.y);

        VoronoiEdge vEdge = new VoronoiEdge(voronoiVertex, voronoiVertexNeighbour, he.previousEdge.vertex.position);

        alledges.Add(vEdge);
    }

    private static int TryFindCellPos(VoronoiEdge ve, List<VoronoiCell> voronoicells)
    {
        for (int i = 0; i < voronoicells.Count; i++)
        {
            if (ve.sitePosition == voronoicells[i].sitePosition)
            {
                return i;
            }
        }
        return -1;
    }
}

public class VoronoiGeneratorV2 : MonoBehaviour
{
    [SerializeField] int seed = 0;

    [SerializeField] float halfMapSize = 10f;

    [SerializeField] int numberOfPoints = 20;

    [SerializeField] float siteRadius = 1f;

    private void OnDrawGizmos()
    {
        List<Vector3> randomSites = new List<Vector3>();

        Random.InitState(seed);

        float min = -halfMapSize;
        float max = halfMapSize;

        float bigSize = halfMapSize * 5f;

        randomSites.Add(new Vector3(0, 0, bigSize));
        randomSites.Add(new Vector3(0, 0, -bigSize));
        randomSites.Add(new Vector3(bigSize, 0, 0));
        randomSites.Add(new Vector3(-bigSize, 0, 0));

        for (int i = 0; i < numberOfPoints; i++)
        {
            float randomX = Random.Range(min, max);
            float randomZ = Random.Range(min, max);

            randomSites.Add(new Vector3(randomX, 0f, randomZ));
        }

        /*randomSites.Add(new Vector3(-bigSize, 0, -bigSize));
        randomSites.Add(new Vector3(2 * bigSize, -bigSize));
        randomSites.Add(new Vector3(2 * bigSize, 2 * bigSize));
        randomSites.Add(new Vector3(-bigSize, -bigSize));*/

        List<VoronoiCell> cells = DelaunayToVoronoi.GenerateVoronoiDiagram(randomSites);

        DisplayVoronoiCells(cells);

        Gizmos.color = Color.white;

        // display sites
        for (int i = 0; i < randomSites.Count; i++)
        {
            Gizmos.DrawSphere(randomSites[i], siteRadius);
        }
    }

    // Display coloured mesh
    private void DisplayVoronoiCells(List<VoronoiCell> cells)
    {
        Random.InitState(seed);

        for (int i = 0; i < cells.Count; i++)
        {
            VoronoiCell c = cells[i];

            Vector3 p1 = c.sitePosition;

            Gizmos.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);

            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();

            vertices.Add(p1);

            for (int j = 0; j < c.edges.Count; j++)
            {
                Vector3 p2 = c.edges[j].v2;
                Vector3 p3 = c.edges[j].v1;

                vertices.Add(p2);
                vertices.Add(p3);

                triangles.Add(0);
                triangles.Add(vertices.Count - 2);
                triangles.Add(vertices.Count - 1);
            }

            Mesh triangleMesh = new Mesh();
            /*triangleMesh.vertices = vertices.ToArray();
            triangleMesh.triangles = triangles.ToArray();*/
            triangleMesh.SetVertices(vertices);
            triangleMesh.SetTriangles(triangles, 0);

            triangleMesh.RecalculateNormals();
            Gizmos.DrawMesh(triangleMesh);
        }
    }
}