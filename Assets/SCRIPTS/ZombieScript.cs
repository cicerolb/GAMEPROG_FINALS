using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ZombieScript : MonoBehaviour
{
    [SerializeField] private Transform target;
    float dist;
    public Animator animator;
    public PlayerMovement playerMovement;
    public Manager manager;
    public EnemyScript enemyScript;

    public bool canDamage = true;
    public float damageCooldown = 2.0f;



    //public float speed = 2f;
    //public float seperationDistance = 1.0f;

    public NavMeshAgent agent = null;




    // Start is called before the first frame update
    void Start()
    {

        
        GetReferences();


        
        //transform.LookAt(target.transform);

    }

    // Update is called once per frame
    public void Update()
    {
        if (enemyScript.alive == false)
        {
            canDamage = false;
            agent.velocity = Vector3.zero;
            agent.isStopped = true;
        }


        MoveToTarget();
        RotateTarget();

        dist = Vector3.Distance(transform.position, target.transform.position);


        if (dist <= 2 && canDamage && playerMovement.playerHP > 0)
        {
            StartCoroutine(DamageToPlayer());
        }

        if (dist <= 2 && playerMovement.playerHP > 0)
        {
            animator.SetBool("Attack", true);
        }
        else
        {
            animator.SetBool("Attack", false);

        }

        if (playerMovement.avatarChanged == true)
        {
            StartCoroutine(EatDelay());
        }

        

    }

    IEnumerator EatDelay()
    {
        yield return new WaitForSeconds(1f);
        animator.SetBool("Eating", true);
        yield return new WaitForSeconds(5f);
        animator.SetBool("Eating", false);
        animator.SetBool("Dancing", true);
    }

    
    private void GetReferences()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindWithTag("Player").transform;
        animator = GetComponentInChildren<Animator>();
        playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        manager = GameObject.Find("GameManager").GetComponent<Manager>();
        enemyScript = GetComponent<EnemyScript>();
    }

    private void MoveToTarget()
    {
        if (enemyScript.alive == true)
        agent.SetDestination(target.position);
    }

    private void RotateTarget()
    {
        if (enemyScript.alive == true)
        {
            Vector3 targetPosition = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);

            transform.LookAt(targetPosition);
        }
        
    }

    public IEnumerator DamageToPlayer()
    {

        if (canDamage)
        {
            canDamage = false;
            

            playerMovement.playerHP -= 1;
            manager.isDamaged = true;


            yield return new WaitForSeconds(damageCooldown);
            canDamage = true;
        }


    }

  
}





    //private void OldMovementCode()
    //{
    //    GameObject[] zombies = GameObject.FindGameObjectsWithTag("Zombie");

    //    foreach (GameObject zombie in zombies)
    //    {


    //        if (zombie != gameObject)
    //        {
    //            Vector3 direction = transform.position - zombie.transform.position;

    //            if (direction.magnitude < seperationDistance
    //                )
    //            {

    //                Vector3 moveDirection = direction.normalized;
    //                transform.position += moveDirection * Time.deltaTime * 0.1f;
    //            }
    //            else
    //            {
    //                speed = 1f;
    //            }
    //        }


    //        float zombieDist = Vector3.Distance(transform.position, zombie.transform.position);



    //        dist = Vector3.Distance(transform.position, target.transform.position);
    //        transform.LookAt(target.transform);


    //        if (dist >= 2)
    //        {
    //            transform.Translate(Vector3.forward * Time.deltaTime * speed);

    //        }
    //    }
    //}

