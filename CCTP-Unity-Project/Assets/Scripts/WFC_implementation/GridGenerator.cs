using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WaveFunctionCollapse;

public class GridGenerator : MonoBehaviour
{
    [SerializeField] string mapName = "===== MAP =====";

    [Range(0, 10)] [SerializeField] private int gridWidth;
    [Range(0, 10)] [SerializeField] private int gridHeight;

    [SerializeField] private float tileOffset = 5f;

    [SerializeField] private GameObject[] tilePrefabs;
    public List<GameObject> tilesInSet = new List<GameObject>();

    public Vector3[,] gridPositions;
    public Vector3[,] possibleTilles;

    public GameObject spherePrefab;

    void Start()
    {
        
    }

    private int GenerateRandomNumber()
    {
        return UnityEngine.Random.Range(0, tilePrefabs.Length);
    }

    public void GenerateTiles()
    {
        if(GameObject.Find(mapName))
        {
            DestroyImmediate(GameObject.Find(mapName));
        }
        GameObject map = new GameObject(mapName);
        tilesInSet.Clear();

        gridPositions = new Vector3[gridWidth, gridHeight];

        /*for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                int randonNumber = Random.Range(0, tilePrefabs.Length);
                Vector3 tilePositionOffset = new Vector3(-gridWidth * tileOffset / 2, 0, -gridHeight * tileOffset / 2);
                Vector3 tilePosition = tilePositionOffset + new Vector3(x * tileOffset, 0, z * tileOffset) + new Vector3(tileOffset / 2, 0, tileOffset / 2);
                GameObject tile = Instantiate(tilePrefabs[randonNumber], tilePosition, Quaternion.identity);
                tile.transform.parent = map.transform;
                tilesInSet.Add(tile);

                gridPositions[x, z] = tilePosition;
            }
        }*/
        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            { 
                Vector3 tilePositionOffset = new Vector3(-gridWidth * tileOffset / 2, 0, -gridHeight * tileOffset / 2);
                Vector3 tilePosition = tilePositionOffset + new Vector3(x * tileOffset, 0, z * tileOffset) + new Vector3(tileOffset / 2, 0, tileOffset / 2);
                gridPositions[x, z] = tilePosition;

                GameObject debugSphere = Instantiate(spherePrefab, tilePosition, Quaternion.identity);
                debugSphere.transform.parent = map.transform;
            }
        }

        GameObject CenterTile = Instantiate(tilePrefabs[GenerateRandomNumber()], gridPositions[gridWidth / 2, gridHeight / 2], Quaternion.identity);
        CenterTile.transform.parent = map.transform;

        foreach(Direction direction in Enum.GetValues(typeof(Direction)))
        {
            Vector3 directionOffset = Vector3.zero;

            switch (direction)
            {
                case Direction.Up:
                    directionOffset = new Vector3(0,0,1);
                    break;
                case Direction.Down:
                    directionOffset = new Vector3(0, 0, -1);
                    break;
                case Direction.Left:
                    directionOffset = new Vector3(-1, 0, 0);
                    break;
                case Direction.Right:
                    directionOffset = new Vector3(1, 0, 0);
                    break;
                default:
                    break;
            }

            GameObject gen1Tile = Instantiate(tilePrefabs[GenerateRandomNumber()], CenterTile.transform.position + directionOffset * tileOffset, Quaternion.identity);
            gen1Tile.transform.parent = map.transform;

            while (CenterTile.GetComponent<Tile>().sockets[(int)direction].value != gen1Tile.GetComponent<Tile>().sockets[(int)DirectionHelper.GetOppositeDirection(direction)].value)
            {
                DestroyImmediate(gen1Tile);
                gen1Tile = Instantiate(tilePrefabs[GenerateRandomNumber()], CenterTile.transform.position + directionOffset * tileOffset, Quaternion.identity);
                gen1Tile.transform.parent = map.transform;
            }
        }

        /*
        int i = 0;
        Direction dir = Direction.Up;

        Debug.Log(GetNeinbourInDirection(i, dir).name);
        Debug.Log(gridPositions[0, 0]);
        Debug.Log(GetNeinbourInDirection(i, dir).sockets[(int)DirectionHelper.GetOppositeDirection(dir)].value);
        Debug.Log(IsNeighbourValidInDirection(i, dir));
        */
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
