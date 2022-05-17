using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
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
}