using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Vector3 StartPosition;
    void Start()
    {
        transform.position = StartPosition; 
    }
}
