using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    private EnemyTargeting enemyTargeting;
    public EnemyController enemyController;


    public GameObject bulletPrefab;

    public Transform bulletSpawn;

    private float coolDownShot = 0;

    public float bulletSpeed = 25;
    public float lifeTime = 1;

    public Transform cannonBarrel;
    public Transform cannonSphere;
    private Vector3 startPosCannonBarrel;
    private Vector3 startPosCannonSphere;
    public ParticleSystem muzzelFlash;


    // Start is called before the first frame update
    void Start()
    {
        enemyTargeting = GetComponentInParent<EnemyTargeting>();
        enemyController = GetComponentInParent<EnemyController>();

        startPosCannonBarrel = cannonBarrel.localPosition;
        startPosCannonSphere = cannonSphere.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        coolDownShot -= Time.deltaTime;
        //if (coolDownShot <= 0  && enemyTargeting.inSight) Fire();

        SlideCannonBarrelBack();


        switch (enemyController.state)
        {
            case EnemyController.States.Idle:
               
                break;
            case EnemyController.States.Attack:
                if (coolDownShot <= 0) Fire();
                break;
            case EnemyController.States.Death:

                break;
        }
    }
    
    private void SlideCannonBarrelBack()
    {
        cannonBarrel.localPosition = AnimMath.Slide(cannonBarrel.localPosition, startPosCannonBarrel, .01f);
        cannonSphere.localPosition = AnimMath.Slide(cannonSphere.localPosition, startPosCannonSphere, .01f);
    }

    public void Fire()
    {
        coolDownShot = .5f;

        GameObject bullet = Instantiate(bulletPrefab);
        Physics.IgnoreCollision(bullet.GetComponent<Collider>(), bulletSpawn.parent.GetComponent<Collider>());

        bullet.transform.position = bulletSpawn.position;
       
        Vector3 rotation = bullet.transform.rotation.eulerAngles;
        bullet.transform.rotation = Quaternion.Euler(rotation.x, transform.eulerAngles.y, rotation.z);

        bullet.GetComponent<Rigidbody>().AddForce(bulletSpawn.forward * bulletSpeed * 2, ForceMode.Impulse);

        if (cannonBarrel) Instantiate(muzzelFlash, bulletSpawn.position, bulletSpawn.rotation);
        cannonBarrel.localEulerAngles += new Vector3(-30, 0, 0);
        cannonBarrel.position += -cannonBarrel.forward * .1f;

        cannonSphere.localEulerAngles += new Vector3(-30, 0, 0);
        cannonSphere.position += -cannonSphere.forward * .1f;

        StartCoroutine(DestroyBulletAfterLifeTime(bullet, lifeTime));
    }

    private IEnumerator DestroyBulletAfterLifeTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);

    }
    
}
