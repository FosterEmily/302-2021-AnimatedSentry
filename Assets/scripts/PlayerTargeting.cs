using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargeting : MonoBehaviour
{
    

    public Transform target;

    public bool wantsToTarget = false;
    public bool wantsToAttack = false;
    public float visionDis = 10;
    public float visionAngle = 45;

    private List<ShootAbleThing> potentialTargets = new List<ShootAbleThing>();

    float coolDownScan = 0;
    float coolDownTarget = 0;
    float coolDownShoot = 0; // 1 divide round per second

    public float roundsPerSecond = 10;

    public Transform armL;
    public Transform armR;

    private Vector3 startPosArmL;
    private Vector3 startPosArmR;

    public ParticleSystem prefabMuzzelFlash;
    public Transform handR;
    public Transform handL;

    cameraOrbit camOrbit;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        startPosArmL = armL.localPosition;
        startPosArmR = armR.localPosition;

        //camOrbit = Camera.main.GetComponentInParent<>();

    }

    // Update is called once per frame
    void Update()
    {
        wantsToTarget = Input.GetButton("Fire1");
        wantsToAttack = Input.GetButton("Fire2");

        if (!wantsToTarget) target = null;

        coolDownScan -= Time.deltaTime;
        if (coolDownScan <= 0 || (target == null && wantsToTarget)) ScanForTargets();
        coolDownTarget -= Time.deltaTime;
        if (coolDownTarget <= 0) PickATarget();

        if (coolDownShoot > 0) coolDownShoot -= Time.deltaTime;

        if (target && !CanSeeThing(target)) target = null;

        SlideArmsHome();

        DoAttack();
       
    }

    private void SlideArmsHome()
    {

        armL.localPosition = AnimMath.Slide(armL.localPosition, startPosArmL, .01f);
        armR.localPosition = AnimMath.Slide(armR.localPosition, startPosArmR, .01f);


    }
    private void DoAttack()
    {
        if (coolDownShoot > 0) return;
        if (!wantsToTarget) return;
        if (!wantsToAttack) return;
        if (target == null) return;
        if (!CanSeeThing(target)) return;

        print("Pew Pew");
        HealthSystem targetHealth = target.GetComponent<HealthSystem>();
        if (targetHealth)
        {
            targetHealth.TakeDamage(20);
        }

        coolDownShoot = 1 / roundsPerSecond;

        //attack!!!


        camOrbit.Shake(1);

        if (handL) Instantiate(prefabMuzzelFlash, handL.position, handL.rotation);
        if (handR) Instantiate(prefabMuzzelFlash, handR.position, handR.rotation);


        //trigger arm animation

        //roates arms up
        armL.localEulerAngles += new Vector3(-20, 0, 0);
        armR.localEulerAngles += new Vector3(-20, 0, 0);
        //move the arms backwards
        armL.position += -armL.forward * .1f;
        armR.position += -armR.forward * .1f;

    }

    private bool CanSeeThing(Transform thing)
    {
        if (!thing) return false;

        Vector3 vToThing = thing.position - transform.position;

        if (vToThing.sqrMagnitude > visionDis*visionDis) return false; //to far away to see

        if (Vector3.Angle(transform.forward, vToThing) > visionAngle) return false; // out of the cone of vision

        return true;


    }
    private void ScanForTargets()
    {

        coolDownScan = 1;

        potentialTargets.Clear();

        ShootAbleThing[] things = GameObject.FindObjectsOfType<ShootAbleThing>();

        foreach(ShootAbleThing thing in things)
        {
            if (CanSeeThing(thing.transform))
            { 
             potentialTargets.Add(thing);
            }
        }
    }



    void PickATarget()
    {
        coolDownTarget = .25f;
        //if (target) return;// we already have a target

        target = null;

        float closetDisSoFar = 0;

        foreach(ShootAbleThing pt in potentialTargets)
        {

           float dd = (pt.transform.position - transform.position).sqrMagnitude;

            if( dd < closetDisSoFar || target == null)
            {
                target = pt.transform;
                closetDisSoFar = dd;
            }


        }

    }

}
