using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public static float health { get; set; }
    public float healthMax = 100;

    public enum States
    {
        Idle,
        Attack,
        Death
    }

    public States state { get; private set; }

    //Enemy Detection 
    public Transform headTransform;

    public float headDetectRaduis = 8;
    public float headRangeRaduis = 3;
    private RaycastHit hitTarget;

    //Layer Masks
    public LayerMask playerLayer;
    public LayerMask sightLayer;
    private Transform myTransform;
    public Transform myTarget;

    private float canSeeTimer = 3;
    private float destroyDelay = 3;

    public EnemyWeapon enemyWeapon;


    public Transform cannonBarrel;
    public Transform cannonSphere;
    private Vector3 startPosCannonBarrel;
    private Vector3 startPosCannonSphere;

    // Start is called before the first frame update
    void Start()
    {
        health = healthMax;
        state = States.Idle;
        myTransform = transform;
        startPosCannonBarrel = cannonBarrel.localPosition;
        startPosCannonSphere = cannonSphere.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        CarryOutDetection();
        if (health <= 0) state = States.Death;
        if (headTransform == null) headTransform = myTransform;
        if (state == States.Death) destroyDelay -= Time.deltaTime;
        if (canSeeTimer >= 0) canSeeTimer -= Time.deltaTime;
        if (canSeeTimer >= 2) canSeeTimer = 2;
        if (canSeeTimer <= 0)
        {
            myTarget = null;
            state = States.Idle;
        }
        print(state);
        //print(destroyDelay);
        print(health);

        switch (state)
        {
            case States.Idle:

                break;
            case States.Attack:
                if (canSeeTimer <= 0) state = States.Idle;
                if (myTarget == null) state = States.Idle;
                break;
            case States.Death:
                SlideCannonBarrelBack();
                Die();
                break;
        }

    }

    public bool CanSeeTarget(Transform potentialTarget)
    {
        //print("HELP");
        if (Physics.Linecast(headTransform.position, potentialTarget.position, out hitTarget, sightLayer))
        {

            if (hitTarget.transform == potentialTarget)
            {
                canSeeTimer = 10;
                //print("I SEE");
                myTarget = potentialTarget;
                return true;
            }
            else
            {
                print("hello");
                myTarget = null;
                state = States.Idle;
                return false;
            }
        }
        else
        {
            print("World");
            myTarget = null;
            state = States.Idle;
            return false;
        }
    }

    public void CarryOutDetection()
    {
        //print("Hello?");
        Collider[] colliders = Physics.OverlapSphere(myTransform.position, headDetectRaduis, playerLayer);
        Collider[] inRange = Physics.OverlapSphere(myTransform.position, headRangeRaduis, playerLayer);
        if (colliders.Length > 0)
        {
            foreach (Collider potentialTargetCollider in inRange)
            {
                if (CanSeeTarget(potentialTargetCollider.transform))
                {

                    //print("I WANNA ATTACK");
                    state = States.Attack;
                    break;
                }
            }

        }
      
    }

    public void TakeDamage(int amt)
    {
        if (amt <= 0) return;
        health -= amt;

        if (health <= 0)
        {
            Die();
        }
    }

    private void SlideCannonBarrelBack()
    {
        cannonBarrel.localPosition = AnimMath.Slide(cannonBarrel.localPosition, startPosCannonBarrel, .25f);
        cannonSphere.localPosition = AnimMath.Slide(cannonSphere.localPosition, startPosCannonSphere, .25f);
    }

    public void Die()
    {
        print("DEAD");
        state = States.Death;
        cannonBarrel.localEulerAngles += new Vector3(.55f, 0, 0);
        ///cannonBarrel.position += -cannonBarrel.forward * .01f;

        cannonSphere.localEulerAngles += new Vector3(.55f, 0, 0);
        //cannonSphere.position += -cannonSphere.forward * .01f;
       
        if (destroyDelay <= 0)
        {
            Destroy(gameObject);
        }

    }
}
