using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargeting : MonoBehaviour
{


    public Transform target;

    public bool wantsToTarget = false;
    public float visionDis = 10;
    public float visionAngle = 45;

    private List<ShootAbleThing> potentialTargets = new List<ShootAbleThing>();

    float coolDownScan = 0;
    float coolDownTarget = 0;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        wantsToTarget = Input.GetButton("Fire2");

        if (!wantsToTarget) target = null;

        coolDownScan -= Time.deltaTime;
        if (coolDownScan <= 0 || (target == null && wantsToTarget)) ScanForTargets();
        coolDownTarget -= Time.deltaTime;
        if (coolDownTarget <= 0) PickATarget();

        if (target && !CanSeeThing(target)) target = null;
       
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
