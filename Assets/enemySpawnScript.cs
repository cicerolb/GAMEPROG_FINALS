using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class enemySpawnScript : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public Manager manager;
    public EnemyScript enemyScript;


    public GameObject zombie;
    public int zombiesToSpawn;
    public float spawnDelay = 1f;
    public float initialDelay = 2f;

    public int zombiesSpawned = 0;
    private float timer = 0f;
    public bool spawning = false;

    public float niggas;


    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        manager = GameObject.Find("GameManager").GetComponent<Manager>();

        StartSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        if (manager.roundFinished)
        {
            spawning = true;
        }
        ZombieModifier();
        if (spawning)
        {
            
            timer += Time.deltaTime;
            if (timer >= spawnDelay && zombiesSpawned < zombiesToSpawn)
            {
                SpawnZombie();
                timer = 0f;
                manager.roundStarted = true;
                manager.roundFinished = false;
            }
        }

        
        
    }

    public void StartSpawn()
    {
        spawning = true;
    }

    void SpawnZombie()
    {
        Instantiate(zombie, transform.position, Quaternion.identity);
        zombiesSpawned++;

        if (zombiesSpawned >= zombiesToSpawn)
        {
            spawning = false;
        }
    }






    public void ZombieModifier()
    {
        switch (manager.roundNumber)
        {
            case 1:
                zombiesToSpawn = 2;
                enemyScript.health = 1;
                break;
            case 2:
                zombiesToSpawn = 4 ;
                enemyScript.health = 1;
                break;
            case 3:
                zombiesToSpawn = 6;
                enemyScript.health = 3;
                break;
            case 4:
                zombiesToSpawn = 9;
                enemyScript.health = 5;
                break;
            case 5:
                zombiesToSpawn = 12;
                enemyScript.health = 7;
                break;
            case 6:
                zombiesToSpawn = 16;
                enemyScript.health = 9;
                break;
            case 7:
                zombiesToSpawn = 20;
                enemyScript.health = 12;
                break;

        }

    }
}
