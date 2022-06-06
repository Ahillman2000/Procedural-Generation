using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Socket[] sockets = new Socket[4];
     
    private void Start()
    {
        foreach (var socket in sockets)
        {
            Debug.Log("direction: " + socket.direction + ", value: " + socket.Value + ", value Neighbours: " + socket.validNeighbours[0]);
        }
    }
}
