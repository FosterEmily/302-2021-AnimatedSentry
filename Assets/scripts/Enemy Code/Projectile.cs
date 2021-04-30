using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float lifespan = .4f;
    private float age = 0;
    void Update()
    {
        age += Time.deltaTime;
        if (age > lifespan)
        {
            Destroy(gameObject);
        }
    }
        private void OnTriggerEnter(Collider other)
    {

       PlayerMovement player = other.GetComponent<PlayerMovement>();

        if (player)
        {
            PlayerMovement.health -= 10;

            Destroy(gameObject);
        }

    }
}
