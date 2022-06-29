using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PossibleTileInstance : MonoBehaviour
{
    public GameObject prefab;

    public PossibleTileInstance(GameObject prefab)
    {
        this.prefab = prefab;
    }
}
