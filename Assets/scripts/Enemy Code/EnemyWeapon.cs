using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    private EnemyTargeting enemyTargeting;

    public GameObject bulletPrefab;

    public Transform bulletSpawn;

    private float coolDownShot = 0;

    public float bulletSpeed = 25;
    public float lifeTime = 1;


    // Start is called before the first frame update
    void Start()
    {
        enemyTargeting = GetComponentInParent<EnemyTargeting>();
    }

    // Update is called once per frame
    void Update()
    {
        coolDownShot -= Time.deltaTime;
        if (coolDownShot <= 0  && enemyTargeting.inSight) Fire();
    

    }

    private void Fire()
    {
        coolDownShot = 2.5f;

        GameObject bullet = Instantiate(bulletPrefab);
        Physics.IgnoreCollision(bullet.GetComponent<Collider>(), bulletSpawn.parent.GetComponent<Collider>());

        bullet.transform.position = bulletSpawn.position;
       
        Vector3 rotation = bullet.transform.rotation.eulerAngles;
        bullet.transform.rotation = Quaternion.Euler(rotation.x, transform.eulerAngles.y, rotation.z);

        bullet.GetComponent<Rigidbody>().AddForce(bulletSpawn.forward * bulletSpeed * 2, ForceMode.Impulse);
        

        StartCoroutine(DestroyBulletAfterLifeTime(bullet, lifeTime));
    }

    private IEnumerator DestroyBulletAfterLifeTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);

    }
}
