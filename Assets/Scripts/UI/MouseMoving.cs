using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MouseMoving : MonoBehaviour
{
    public Camera cam;
    private Vector2 prevPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            // 使用 GetAxis("Mouse X") 获取平滑的增量值
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            float moveSpeed = 0.2f;
            cam.transform.position -= cam.transform.right * mouseX * moveSpeed;

        }
    }
    
    
}
