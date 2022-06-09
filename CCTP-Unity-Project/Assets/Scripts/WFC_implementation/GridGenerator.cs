using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WaveFunctionCollapse;
using Helpers;

public class GridGenerator : MonoBehaviour
{
    [SerializeField] string mapName = "===== MAP =====";

    [Range(0, 10)] [SerializeField] private int gridWidth;
    [Range(0, 10)] [SerializeField] private int gridHeight;

    [SerializeField] private float tileOffset = 5f;

    [SerializeField] private List<GameObject> tilePrefabs;
    public List<GameObject> tilesInSet = new List<GameObject>();

    public GameObject spherePrefab;
    public GameObject[] spherePrefabs;

    public Dictionary<int, List<GameObject>> possibleTilesInCells = new Dictionary<int, List<GameObject>>();

    void Start()
    {
        
    }

    private int GenerateRandomTile(List<GameObject> tiles)
    {
        return UnityEngine.Random.Range(0, tiles.Count);
    }

    public void GenerateTiles()
    {
        if(GameObject.Find(mapName))
        {
            DestroyImmediate(GameObject.Find(mapName));
        }
        GameObject map = new GameObject(mapName);

        tilesInSet.Clear();
        possibleTilesInCells.Clear();
        spherePrefabs = new GameObject[gridWidth * gridHeight];

        for (int row = 0; row < gridWidth; row++)
        {
            for (int col = 0; col < gridHeight; col++)
            {
                int i = HelperFunctions.ConvertTo1dArray(row, col, gridWidth);

                Vector3 cellPositionOffset = new Vector3(-gridWidth * tileOffset / 2, 0, -gridHeight * tileOffset / 2) + new Vector3(tileOffset/2, 0, tileOffset/2);
                Vector3 cellPosition = new Vector3(row * tileOffset, 0, col * tileOffset);
                spherePrefabs[i] = Instantiate(spherePrefab, cellPositionOffset + cellPosition, Quaternion.identity);
                spherePrefabs[i].transform.parent = map.transform;
                possibleTilesInCells.Add(i, tilePrefabs);
            }
        }

        for (int i = 0; i < spherePrefabs.Length; i++)
        {
            List<GameObject> valuesInKey = possibleTilesInCells[i];
            //Debug.Log("values in tile 0: " + valuesInKey);

            int numberOfValuesInKey = valuesInKey.Count;
            //Debug.Log("number of values in tile 0: " + numberOfValuesInKey);

            for (int j = 0; j < numberOfValuesInKey; j++)
            {
                GameObject tile = valuesInKey[j];
                GameObject tileInstance = Instantiate(tile);
                tileInstance.transform.parent = spherePrefabs[i].transform;
                tileInstance.transform.localPosition = Vector3.zero;
            }
        }
    }

    public bool IsNeighbourValidInDirection(int tileIndex, Direction direction)
    {
        return tilesInSet[tileIndex].GetComponent<Tile>().sockets[(int)direction].value == GetNeinbourInDirection(tileIndex, direction).sockets[(int)DirectionHelper.GetOppositeDirection(direction)].value;
    }

    public Tile GetNeinbourInDirection(int index, Direction dir)
    {
        switch (dir)
        {
            case Direction.Up:
                return tilesInSet[index + 1].GetComponent<Tile>();
            case Direction.Down:
                return tilesInSet[index - 1].GetComponent<Tile>();
            case Direction.Left:
                return tilesInSet[index - gridHeight].GetComponent<Tile>();
            case Direction.Right:
                return tilesInSet[index + gridHeight].GetComponent<Tile>();
            default:
                Debug.LogWarning("No tile in given direction");
                return null;
        }
    }

    void Update()
    {
        
    }
}
