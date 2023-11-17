using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    public float health;
    public Manager manager;
    public Animator animator;
    public bool alive = true;
    private BoxCollider hitbox;

    // Ammos
    public GameObject pistolPreFab;
    public GameObject arPreFab;
    public GameObject sniperPreFab;
    public GameObject trapPreFab;

    private void Start()
    {
        manager = GameObject.Find("GameManager").GetComponent<Manager>();
        animator = gameObject.GetComponentInChildren<Animator>();
        hitbox = GetComponent<BoxCollider>();
    }
    public virtual void TakeDamage(float dmg)
    {
        health -= dmg;

        transform.Translate(Vector3.back * Time.deltaTime * 70);

        Debug.Log(health);

        if (health <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        alive = false;
        manager.kills++;
        hitbox.enabled = false;
        Drops();
        DeathRandomizer();

        StartCoroutine(DeSpawn());

    }

    IEnumerator DeSpawn()
    {
        yield return new WaitForSeconds(4f);
        Destroy(gameObject);
    }

    public void Drops()
    {
        int randomDrop = Random.Range(1, 101);

        switch (randomDrop)
        {
            case int n when (n <= 39):
                Instantiate(pistolPreFab, transform.position - new Vector3(0f, 1f, 0f), Quaternion.identity);
                break;
            case int n when (n <= 64):

                break;
            case int n when (n <= 84):
                Instantiate(arPreFab, transform.position - new Vector3(0f, 1f, 0f), Quaternion.identity);
                break;
            case int n when (n <= 94):
                Instantiate(sniperPreFab, transform.position - new Vector3(0f, 1f, 0f), Quaternion.identity);
                break;

            default:
                Instantiate(trapPreFab, transform.position - new Vector3(0f, 1f, 0f), Quaternion.identity);
                break;

        }
        
    }

    public void DeathRandomizer()
    {
        int randomDeath = Random.Range(1, 4);

        switch (randomDeath)
        {
            case 1:
                animator.SetBool("Death 1", true);
                break;
            case 2:
                animator.SetBool("Death 2", true);
                break;
            case 3:
                animator.SetBool("Death 3", true);
                break;
        }
           
    }
}
