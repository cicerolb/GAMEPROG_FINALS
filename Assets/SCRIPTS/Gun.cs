using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class Gun : MonoBehaviour
{
    public enum GunType {Pistol,AR, Marksman}
    public GunType gunType;
    public float fireRate;
    public float magazine;
    public float damage;

    public bool isShooting;



    public AmmoManager ammoManager;
    public Manager manager;
    public PlayerMovement playerMovement;



    public LayerMask collisionMask;
    public Transform bulletSpawn;
    private LineRenderer tracer;

    public Transform shellSpawn;
    public Rigidbody shell;

    public Transform magSpawn;
    public Rigidbody magPreFab;

    private float SecondsBetweenShots;
    private float nextPossibleShootTime;

    public float reloadTime;

    float pistolMag;
    float pistolMagSize;
    public float pistolLeft;

    public float arMag;
    public float arMagSize;
    public float arLeft;

    public float mmMag;
    public float mmMagSize;
    public float mmLeft;

    public float magsLeft;

    public void Start()
    {
        ammoManager = GameObject.Find("player").GetComponent<AmmoManager>();
        manager = GameObject.Find("GameManager").GetComponent<Manager>();
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();

        SecondsBetweenShots = 60 / fireRate;
        if (GetComponent<LineRenderer>())
        {
            tracer = GetComponent<LineRenderer>();
        }

        pistolMag = ammoManager.pistolMag;
        pistolMagSize = ammoManager.pistolMagSize;
        pistolLeft = ammoManager.pistolMagsLeft;

        arMag = ammoManager.arMag;
        arMagSize = ammoManager.arMagSize;
        arLeft = ammoManager.arMagsLeft;

        mmMag = ammoManager.mmMag;
        mmMagSize = ammoManager.mmMagSize;
        mmLeft = ammoManager.mmMagsLeft;



        if (gunType == GunType.Pistol)
        {
            magazine = pistolMag;
            magsLeft = pistolLeft;
        }

        if (gunType == GunType.AR)
        {
            magazine = arMag;
            magsLeft = arLeft;
        }

        if (gunType == GunType.Marksman)
        {
            magazine = mmMag;
            magsLeft = mmLeft;
        }
 

    }


    public void Update()
    {
        if (manager.isReloading == false)
        {
            manager.magazineValue = magazine;

            if (playerMovement.ammoPickUp == true)
            {
                pistolLeft = ammoManager.pistolMagsLeft;
                arLeft = ammoManager.arMagsLeft;
                mmLeft = ammoManager.mmMagsLeft;


                if (gunType == GunType.Pistol)
                {
                    magsLeft = pistolLeft;
                    manager.magsLeft = magsLeft;
                }

                if (gunType == GunType.AR)
                {
                    magsLeft = arLeft;
                    manager.magsLeft = magsLeft;
                }

                if (gunType == GunType.Marksman)
                {
                    magsLeft = mmLeft;
                    manager.magsLeft = magsLeft;
                }
            }
        }
        


        


        if (Input.GetKeyDown(KeyCode.R) && gunType == GunType.Pistol && magazine < pistolMagSize
            && ammoManager.pistolMagsLeft >= 1)
        {
            

            
            StartCoroutine(PistolReloading());
            Rigidbody newMag = Instantiate(magPreFab, magSpawn.position, Quaternion.identity) as Rigidbody;

 
            
        }

        
        if (Input.GetKeyDown(KeyCode.R) && gunType == GunType.AR && magazine < arMagSize 
            && ammoManager.arMagsLeft >= 1)
        {

            StartCoroutine(ArReloading());
            Rigidbody newMag = Instantiate(magPreFab, magSpawn.position, Quaternion.identity) as Rigidbody;
        }

        if (Input.GetKeyDown(KeyCode.R) && gunType == GunType.Marksman
            && ammoManager.mmMagsLeft >=1)
        {
            StartCoroutine(MarksmanReloading());
            Rigidbody newMag = Instantiate(magPreFab, magSpawn.position, Quaternion.identity) as Rigidbody;
        }

        manager.magsLeft = magsLeft;
    }




    public void Shoot()
    {
        if (CanShoot())
        {
            Ray ray = new Ray(bulletSpawn.position, bulletSpawn.forward);
            RaycastHit hit;

            float shotDistance = 20;

            if (Physics.Raycast(ray, out hit, shotDistance, collisionMask))
            {
                shotDistance = hit.distance;

                if (hit.collider.GetComponent<EnemyScript>())
                {
                    hit.collider.GetComponent<EnemyScript>().TakeDamage(damage);
                }
            }
            Debug.DrawRay(ray.origin, ray.direction * shotDistance, Color.red, 1);

            nextPossibleShootTime = Time.time + SecondsBetweenShots;

            AudioSource audio = GetComponent<AudioSource>();
            audio.Play();

            if (tracer)
            {
                StartCoroutine("RendererTracer", ray.direction * shotDistance);
            }

            Rigidbody newShell = Instantiate(shell, shellSpawn.position,Quaternion.identity) as Rigidbody;
            newShell.AddForce(shellSpawn.forward * Random.Range(150f, 200f) + bulletSpawn.forward * Random.Range(-10f, 10f));

            magazine -= 1;

            if (gunType == GunType.Pistol)
            {
                ammoManager.pistolMag = magazine;
                manager.magazineValue = magazine;
                //manager.magsLeft = magsLeft;
            }

            if (gunType == GunType.AR)
            {
                ammoManager.arMag = magazine;
                manager.magazineValue = magazine;
                //manager.magsLeft = magsLeft;
            } 
            
            if (gunType == GunType.Marksman)
            {
                ammoManager.mmMag = magazine;
                manager.magazineValue = magazine;
                //manager.magsLeft = magsLeft;
            }



            


        }



       


    }

    public IEnumerator MarksmanReloading()
    {
        manager.isReloading = true;
        yield return new WaitForSeconds(7.0f);

        magazine += (mmMagSize - magazine);
        magsLeft--;
        ammoManager.mmMag = magazine;
        manager.magazineValue = magazine;
        mmLeft--;
        ammoManager.mmMagsLeft = magsLeft;
        manager.isReloading = false;
    }

    public IEnumerator PistolReloading()
    {
        manager.isReloading = true;
        yield return new WaitForSeconds(2.0f);

        magazine += (pistolMagSize - magazine);
        magsLeft--;
        ammoManager.pistolMag = magazine;
        manager.magazineValue = magazine;
        pistolLeft--;
        ammoManager.pistolMagsLeft = magsLeft;
        manager.isReloading = false;
    }

    public IEnumerator ArReloading()
    {
        manager.isReloading = true;
        yield return new WaitForSeconds(4.0f);
        magazine += (arMagSize - magazine);
        magsLeft--;

        ammoManager.arMag = magazine;
        manager.magazineValue = magazine;
        arLeft--;

        ammoManager.arMagsLeft = magsLeft;
        manager.isReloading = false;
    }

    public void ShootContinous()
    {
        if (gunType == GunType.AR)
        {
            Shoot();
        }
    }
  
    private bool CanShoot()
    {
        bool canShoot = true;

        if (Time.time < nextPossibleShootTime)
        {
            canShoot = false;
        }

        if (magazine <= 0 || Time.timeScale == 0f || manager.isReloading == true)
        {
            canShoot = false;
        }

        return canShoot;
    }

    IEnumerator RendererTracer(Vector3 hitPoint)
    {
        tracer.enabled = true;
        tracer.SetPosition(0, bulletSpawn.position);
        tracer.SetPosition(1, bulletSpawn.position + hitPoint);
        yield return null;
        tracer.enabled = false;
    }

    


}
