using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHeight : MonoBehaviour
{
    public void SetCameraHeight()
    {
        this.transform.position = new Vector3(0f, GridGenerator.Instance.gridDimension * 30, 0f);
    }
}
