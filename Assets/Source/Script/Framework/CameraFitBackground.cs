using System;
using UnityEngine;


public class CameraFitBackground : MonoBehaviour
{
    public SpriteRenderer risk;

    private void Start()
    {
        float orthoSize = risk.bounds.size.y / 2f;
        Camera.main.orthographicSize = orthoSize;
    }
}
