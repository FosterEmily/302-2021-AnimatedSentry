using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPointAt : MonoBehaviour
{
    private EnemyController enemyController;
    // public Transform target;
    private EnemyTargeting enemy;

    private Quaternion startingRotation;

    public bool lockEnemyRotationX;
    public bool lockEnemyRotationY;
    public bool lockEnemyRotationZ;

    void Start()
    {
        startingRotation = transform.localRotation;
        enemyController = GetComponentInParent<EnemyController>();
        enemy = GetComponentInParent<EnemyTargeting>();
    }


    void Update()
    {
        TurnTowardsTheTarget();
    }

    private void TurnTowardsTheTarget()
    {

        if (enemyController.myTarget != null)
        {
            Vector3 disToTarget = enemyController.myTarget.position - transform.position;

            Quaternion targetRotation = Quaternion.LookRotation(disToTarget, Vector3.up);

            Vector3 euler1 = transform.localEulerAngles;
            Quaternion prevRot = transform.rotation;
            transform.rotation = targetRotation;
            Vector3 euler2 = transform.localEulerAngles;

            if (lockEnemyRotationX) euler2.x = euler1.x;
            if (lockEnemyRotationY) euler2.y = euler1.y;
            if (lockEnemyRotationZ) euler2.z = euler1.z;

            transform.rotation = prevRot;
            transform.localRotation = AnimMath.Slide(transform.localRotation, Quaternion.Euler(euler2), .01f);
        }
        else
        {
            transform.localRotation = AnimMath.Slide(transform.localRotation, startingRotation, .05f);
        }
    }
}