using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController pawn;
    private Camera cam;

    public float walkSpeed = 4;

    public Transform leg1; //left leg
    public Transform leg2; // right leg

    private Vector3 inputDirection = new Vector3();

    private float verticalVelocity = 0;


    void Start()
    {

        cam = Camera.main;
        pawn = GetComponent<CharacterController>();
    }

   
    void Update()
    {
        MovePlayer();
        WiggleLegs();
    }

    private void WiggleLegs()
    {
        float degrees = 45;
        float speed = 10;


       Vector3 inputDirLocal = transform.InverseTransformDirection(inputDirection);
        Vector3 axis = Vector3.Cross(inputDirLocal, Vector3.up);

        //check the alignment of inputDirLocal againt vector
        float alignment = Vector3.Dot(inputDirLocal, Vector3.forward);

        //if (alignment < 0) alignment *= -1;
        alignment = Mathf.Abs(alignment);

        degrees *= AnimMath.Lerp(.25f, 1, alignment); //decrease degrees when strafing
        
        float wave = Mathf.Sin(Time.time * speed) * degrees; //outputs valuse between (-1 to 1)


        leg1.localRotation = AnimMath.Slide(leg1.localRotation,Quaternion.AngleAxis(wave, axis), .001f);
        leg2.localRotation = AnimMath.Slide(leg2.localRotation,Quaternion.AngleAxis(-wave, axis), .001f);
    }

    private void MovePlayer()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        bool isTryingToMove = (h != 0 || v != 0);

        if (isTryingToMove)
        {
            float camYaw = cam.transform.eulerAngles.y;
            transform.rotation = AnimMath.Slide(transform.rotation, Quaternion.Euler(0, camYaw, 0), .02f);
        }

        inputDirection = transform.forward * v + transform.right * h;
        if (inputDirection.sqrMagnitude > 1) inputDirection.Normalize();

        verticalVelocity += 10 * Time.deltaTime;

        Vector3 moveDelta = inputDirection * walkSpeed + verticalVelocity * Vector3.down;

        CollisionFlags flags =  pawn.Move(moveDelta * Time.deltaTime);

        if(pawn.isGrounded)
        {
            verticalVelocity = 0;// on ground, zero-out vertical-velocity
        }
    }
}
