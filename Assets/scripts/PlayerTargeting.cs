using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargeting : MonoBehaviour
{


    public Transform target;

    public bool wantsToTarget = false;

    private List<ShootAbleThing> potentialTargets = new List<ShootAbleThing>();

    float coolDownScan = 0;
    float coolDownTarget = 0;

    public float visionDis = 10;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        wantsToTarget = Input.GetButton("Fire2");

        coolDownScan -= Time.deltaTime;
        if (coolDownScan <= 0) ScanForTargets();
        coolDownTarget -= Time.deltaTime;
        if (coolDownTarget <= 0) PickATargety();

    }

    private void ScanForTargets()
    {

        coolDownScan = 1;

        potentialTargets.Clear();

        ShootAbleThing[] things = GameObject.FindObjectsOfType<ShootAbleThing>();

        foreach(ShootAbleThing thing in things)
        {

           Vector3 disToThing = thing.transform.position - transform.position;

           if( disToThing.sqrMagnitude < visionDis * visionDis)
            {

                if (Vector3.Angle(transform.forward, disToThing) < 45)
                {

                    potentialTargets.Add(thing);

                }

            }

        }

    }

    void PickATargety()
    {
        coolDownTarget = 5;
        if (target) return;// we already have a target

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
