using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*[RequireComponent(typeof(SpriteRenderer))]
public class VoronoiGenerator : MonoBehaviour
{
    [SerializeField] Vector2Int imageDimension;
    [SerializeField] int regionAmount;

    void Start()
    {

    }

    [ContextMenu("GenerateDiagram")]
    void GenerateDiagram()
    {
        this.GetComponent<SpriteRenderer>().sprite = Sprite.Create(GetDiagram(), new Rect(0, 0, imageDimension.x, imageDimension.y), Vector2.one * 0.5f);
    }

    Texture2D GetDiagram()
    {
        // points to divide areas with
        Vector2Int[] centroids = new Vector2Int[regionAmount];

        Color[] regions = new Color[regionAmount];

        for (int i = 0; i < regionAmount; i++)
        {
            centroids[i] = new Vector2Int(Random.Range(0, imageDimension.x + 1), Random.Range(0, imageDimension.y + 1));
            regions[i] = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1.0f);
        }

        Color[] pixelColours = new Color[imageDimension.x * imageDimension.y];
        for (int x = 0; x < imageDimension.x; x++)
        {
            for (int y = 0; y < imageDimension.y; y++)
            {
                int index = x * imageDimension.x + y;
                pixelColours[index] = regions[GetClosetsCentroidIndex(new Vector2Int(x, y), centroids)];
            }
        }
        return GetImageFromColourArray(pixelColours);
    }

    int GetClosetsCentroidIndex(Vector2Int pixelPos, Vector2Int[] centroids)
    {
        float smallestDistance = float.MaxValue;
        int index = 0;

        for (int i = 0; i < centroids.Length; i++)
        {
            if (Vector2.Distance(pixelPos, centroids[i]) < smallestDistance)
            {
                smallestDistance = Vector2.Distance(pixelPos, centroids[i]);
                index = i;
            }
        }
        return index;
    }

    Texture2D GetImageFromColourArray(Color[] pixelColors)
    {
        Texture2D texture = new Texture2D(imageDimension.x, imageDimension.y);
        texture.filterMode = FilterMode.Point;
        texture.SetPixels(pixelColors);
        texture.Apply();
        return texture;
    }

    void Update()
    {

    }
}*/

public class VoronoiEdge
{
    public Vector3 v1;
    public Vector3 v2;

    public Vector3 sitePosition;

    public VoronoiEdge(Vector3 v1,Vector3 v2, Vector3 sitePositon)
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

public class DelaunayToVoronoi
{
    public static List<VoronoiCell> GenerateVoronoiDiagram(List<Vector3> sites)
    {
        List<Triangle> triangles = Delaunay.TriangulateByFlippingEdges(sites);

        return null;
    }
}

public class VoronoiGenerator : MonoBehaviour
{
    [SerializeField] int seed = 0;

    [SerializeField] float halfMapSize = 10f;

    [SerializeField] int numberOfPoints = 20;

    private void OnDrawGizmos()
    {
        List<Vector3> randomSites = new List<Vector3>();

        Random.InitState(seed);

        float min = -halfMapSize;
        float max =  halfMapSize;

        for (int i = 0; i < numberOfPoints; i++)
        {
            float randomX = Random.Range(min, max);
            float randomZ = Random.Range(min, max);

            randomSites.Add( new Vector3(randomX, 0, randomZ));
        }

        float bigSize = halfMapSize * 5f;

        randomSites.Add( new Vector3(0, 0, bigSize));
        randomSites.Add(new Vector3(0, 0, -bigSize));
        randomSites.Add(new Vector3(bigSize, 0, 0));
        randomSites.Add(new Vector3(-bigSize, 0, 0));

        //List<VoronoiCell> cells = Dela
    }
}