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

    void Start()
    {

    }

    public void GenerateTiles()
    {
        if(GameObject.Find(mapName))
        {
            DestroyImmediate(GameObject.Find(mapName));
        }
        GameObject map = new GameObject(mapName);
        //map.transform.position = new Vector3(gridWidth/2, 0, gridHeight/2);
        tilesInSet.Clear();

        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                int randonNumber = Random.Range(0, tilePrefabs.Length);
                GameObject tile = Instantiate(tilePrefabs[randonNumber], new Vector3(x * tileOffset, 0, z * tileOffset), Quaternion.identity);
                tile.transform.parent = map.transform;
                tilesInSet.Add(tile);
            }
        }

        GetNeinbourInDirection(4, Direction.Left);
    }

    public void GetNeinbourInDirection(int index, Direction dir)
    {
        switch (dir)
        {
            case Direction.Up:
                Debug.Log("up: " + tilesInSet[index + 1]);
                Debug.Log(tilesInSet[index + 1].GetComponent<Module>().sockets[(int)dir.GetOppositeDirection()].Value);
                break;
            case Direction.Down:
                Debug.Log("down: " + tilesInSet[index - 1]);
                Debug.Log(tilesInSet[index - 1].GetComponent<Module>().sockets[(int)dir.GetOppositeDirection()].Value);
                break;
            case Direction.Left:
                Debug.Log("Left: " + tilesInSet[index - gridHeight]);
                Debug.Log(tilesInSet[index - gridHeight].GetComponent<Module>().sockets[(int)dir.GetOppositeDirection()].Value);
                break;
            case Direction.Right:
                Debug.Log("Right: " + tilesInSet[index + gridHeight]);
                Debug.Log(tilesInSet[index + gridHeight].GetComponent<Module>().sockets[(int)dir.GetOppositeDirection()].Value);
                break;
            default:
                break;
        }
    }

    void Update()
    {
        
    }
}
