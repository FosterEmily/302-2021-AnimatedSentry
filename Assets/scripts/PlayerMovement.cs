using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController pawn;

    public float walkSpeed = 4;

    private Camera cam;

    
    void Start()
    {

        cam = Camera.main;
        pawn = GetComponent<CharacterController>();
    }

   
    void Update()
    {

       float h = Input.GetAxis("Horizontal");
       float v = Input.GetAxis("Vertical");

        bool isTryingToMove = (h != 0 || v != 0);

        if(isTryingToMove)
        {
           float camYaw = cam.transform.eulerAngles.y;
            transform.rotation = AnimMath.Slide(transform.rotation, Quaternion.Euler(0, camYaw, 0), .02f);
        }

     

       Vector3 inputDirection = transform.forward * v +transform.right * h;

       pawn.SimpleMove(inputDirection * walkSpeed);
    }
}
