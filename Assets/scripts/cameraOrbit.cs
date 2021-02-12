using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraOrbit : MonoBehaviour
{
    public PlayerMovement target;

    private float yaw = 0;
    private float pitch = 0;

    public float cameraSenX = 10;
    public float cameraSenY = 10;

    void Start()
    {


        
    }

    
    void Update()
    {
        RotateCamera();

        transform.position = target.transform.position;





    }

    private void RotateCamera()
    {
        float mx = Input.GetAxisRaw("Mouse X");
        float my = Input.GetAxisRaw("Mouse Y");

        yaw += mx * cameraSenX;
        pitch += my * cameraSenY;

        pitch = Mathf.Clamp(pitch, -89, 89);
        transform.rotation = Quaternion.Euler(pitch, yaw, 0);
    }
}
