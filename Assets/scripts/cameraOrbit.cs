using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class cameraOrbit : MonoBehaviour
{
    public PlayerMovement moveScript;

    private PlayerTargeting targetScript;

    private Camera cam;

    private float yaw = 0;
    private float pitch = 0;

    public float cameraSenX = 10;
    public float cameraSenY = 10;

    public float shakeIntesity = 0;

    void Start()
    {
        targetScript = moveScript.GetComponent<PlayerTargeting>();
        cam = GetComponentInChildren<Camera>();
        
    }

    
    void Update()
    {
        PlayerOrbitCamera();

        transform.position = moveScript.transform.position;

        //if aiming, set camera rotation to look at target
        RotateCamToLookAtTarget();
        //"zoom" in the camera
        ZoomCamera();
        ShackCamera();

    }


    public void Shake(float intesity = 1)
    { 
            shakeIntesity = intesity;
    }
    private void ShackCamera()
    {
        if (shakeIntesity < 0) shakeIntesity = 0;

        if (shakeIntesity > 0) shakeIntesity -= Time.deltaTime;
        else return;// sake is 0 so do nothing


        Quaternion targetRot = AnimMath.Lerp(Random.rotation, Quaternion.identity, .999f);

        //cam.transform.localRotation *= targetRot ;
        cam.transform.localRotation = AnimMath.Lerp(cam.transform.localRotation, cam.transform.localRotation * targetRot, shakeIntesity * shakeIntesity);
    }

    private void ZoomCamera()
    {
        float dis = 10;
        if (IsTargeting()) dis = 3;

        cam.transform.localPosition = AnimMath.Slide(cam.transform.localPosition, new Vector3(0, 0, -dis), .001f);
    }

    private bool IsTargeting()
    {
        return (targetScript && targetScript.target != null && targetScript.wantsToTarget) ;
    }
    private void RotateCamToLookAtTarget()
    {

        if(targetScript && targetScript.target != null)
        {
            //if targeting set otation to look at target

            Vector3 vToTarget = targetScript.target.position - cam.transform.position;

            Quaternion targetRot = Quaternion.LookRotation(vToTarget, Vector3.up);


            cam.transform.rotation = AnimMath.Slide(cam.transform.rotation, targetRot, .001f);
        }
        else
        {
            // if not targeting reset rotation
            cam.transform.localRotation = AnimMath.Slide(cam.transform.localRotation, Quaternion.identity, .001f); //no rotation

        }



    }

    private void PlayerOrbitCamera()
    {
        float mx = Input.GetAxisRaw("Mouse X");
        float my = Input.GetAxisRaw("Mouse Y");

        yaw += mx * cameraSenX;
        pitch += my * cameraSenY;
         if (IsTargeting())
        {
            float playerYaw = moveScript.transform.eulerAngles.y;
            pitch = Mathf.Clamp(pitch, 0, 35);
            yaw = Mathf.Clamp(yaw, playerYaw - 45, playerYaw + 45);
        }
        else
        {
        pitch = Mathf.Clamp(pitch, 0, 89);
        }
        transform.rotation = AnimMath.Slide(transform.rotation, Quaternion.Euler(pitch, yaw, 0), .001f);
    }
}
