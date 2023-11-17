using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LandMineScript : MonoBehaviour
{
    public GameObject explosionEffect; 
    public float explosionRadius = 5f; 
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zombie"))
        {
            Explode();
        }
    }

    private void Explode()
    {
     
        Instantiate(explosionEffect, transform.position,Quaternion.identity);

        
        // Detect nearby zombies within the explosion radius
        Collider[] zombies = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider zombie in zombies)
        {
            // Check if the collider has the specified tag
            if (zombie.CompareTag("Zombie"))
            {
                EnemyScript enemyScript = zombie.GetComponent<EnemyScript>();

                enemyScript.Die();
            }
        }

        
        Destroy(gameObject);
    }

}