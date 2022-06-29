using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHeight : MonoBehaviour
{
    [SerializeField] GridGenerator gridGenerator;
    public void SetCameraHeight()
    {
        this.transform.position = new Vector3(0f, gridGenerator.gridDimension * 30, 0f);
    }
}
