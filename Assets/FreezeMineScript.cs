using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeMineScript : MonoBehaviour
{
    public GameObject iceExplosionEffect;
    public float explosionRadius = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zombie"))
        {
            Explode();
        }
    }

    private void Explode()
    {
        Instantiate(iceExplosionEffect, transform.position, Quaternion.identity);

        Collider[] zombies = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider zombie in zombies)
        {
            // Check if the collider has the specified tag
            if (zombie.CompareTag("Zombie"))
            {
                ZombieScript zombieScript = zombie.GetComponent<ZombieScript>();

                zombieScript.agent.speed = 1;
            }
        }
        Destroy(gameObject);
    }
}
