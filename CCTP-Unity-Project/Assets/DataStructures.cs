using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfEdge
{
    public Vertex vertex;

    public Triangle triangle;

    public HalfEdge previousEdge;
    public HalfEdge nextEdge;
    public HalfEdge opositeEdge;

    public HalfEdge(Vertex v)
    {
        this.vertex = v;
    }
}

public class Edge
{
    public Vertex v1;
    public Vertex v2;

    public bool isIntersecting = false;

    public Edge(Vertex v1, Vertex v2)
    {
        this.v1 = v1;
        this.v2 = v2;
    }

    public Edge(Vector3 v1, Vector3 v2)
    {
        this.v1 = new Vertex(v1);
        this.v2 = new Vertex(v2);
    }

    public Vector2 GetVertex2D(Vertex v)
    {
        return new Vector2(v.position.x, v.position.z);
    }

    public void FlipEdge()
    {
        Vertex tmp = v1;

        v1 = v2;

        v2 = tmp;
    }
}

public class Vertex
{
    public Vector3 position;

    public HalfEdge halfEdge;

    public Triangle triangle;

    public Vertex previousVertex;
    public Vertex nextVertex;

    public bool isReflex;
    public bool isConvex;
    public bool isEar;

    public Vertex(Vector3 position)
    {
        this.position = position;
    }

    public Vector2 GetPos2D_XZ()
    {
        Vector2 pos_2d_xz = new Vector2(position.x, position.z);

        return pos_2d_xz;
    }
}

public class Triangle
{
    public Vertex v1;
    public Vertex v2;
    public Vertex v3;

    public HalfEdge halfEdge;

    public Triangle(Vertex v1, Vertex v2, Vertex v3)
    {
        this.v1 = v1;
        this.v2 = v2;
        this.v3 = v3;
    }

    public Triangle(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        this.v1 = new Vertex(v1);
        this.v2 = new Vertex(v2);
        this.v3 = new Vertex(v3);
    }

    public Triangle(HalfEdge halfEdge)
    {
        this.halfEdge = halfEdge;
    }

    public void ChangeOrientation()
    {
        Vertex tmp = this.v1;

        this.v1 = this.v2;

        this.v2 = tmp;
    }
}
