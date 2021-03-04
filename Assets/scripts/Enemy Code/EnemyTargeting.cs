using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTargeting : MonoBehaviour
{
    public Transform target;

    public float visionDis = 3;
    public float visionAngle = 5;

    public bool inSight = false;

    float coolDownScan = 0;
    float coolDownTarget = 0;
   // float coolDownShoot = 0; // 1 divide round per second

    public float roundsPerSecond = 10;

    private List<EnemyTargets> potentialTargets = new List<EnemyTargets>();

    private void Update()
    {
        if (target && !CanSeeThing(target)) target = null;
        //if (!inSight) target = null;

        coolDownScan -= Time.deltaTime;
        if (coolDownScan <= 0 ) ScanForTargets();
        coolDownTarget -= Time.deltaTime;
        if (coolDownTarget <= 0) PickATarget();


        if (target && !CanSeeThing(target)) target = null;


    }

    private void ScanForTargets()
    {

        coolDownScan = .5f;

       potentialTargets.Clear();
        inSight = false;

        EnemyTargets[] things = GameObject.FindObjectsOfType<EnemyTargets>();

        foreach (EnemyTargets thing in things)
        {
            if (CanSeeThing(thing.transform))
            {
                potentialTargets.Add(thing);
                
                print("Can see player");

                inSight = true;

               
            }
        }
    }
    private bool CanSeeThing(Transform thing)
    {
        if (!thing) return false;

        Vector3 vToThing = thing.position - transform.position;

        if (vToThing.sqrMagnitude > visionDis * visionDis) return false; //to far away to see

        if (Vector3.Angle(transform.forward, vToThing) > visionAngle) return false; // out of the cone of vision

        return true;
       

    }
    void PickATarget()
    {
        coolDownTarget = .25f;
        //if (target) return;// we already have a target

        target = null;

        float closetDisSoFar = 0;

        foreach (EnemyTargets pt in potentialTargets)
        {

            float dd = (pt.transform.position - transform.position).sqrMagnitude;


            if (dd < closetDisSoFar || target == null)
            {
                target = pt.transform;
                closetDisSoFar = dd;
            }


        }

    }
}
