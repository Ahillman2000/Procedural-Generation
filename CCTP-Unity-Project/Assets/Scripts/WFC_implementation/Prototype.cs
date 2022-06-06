using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Prototype
{
    public string name;
    public Mesh mesh;
    public Quaternion rotation;
    //public List<Socket> neighbour_list;

    [Header("SOCKETS")]
    [Space(2)]

    public Socket Up;
    public Socket Down;
    public Socket Left;
    public Socket Right;
    [Space(10)]

    public bool symmetric;
    public bool flipped;
}
