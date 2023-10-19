using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class Gun : MonoBehaviour
{
    public enum GunType {Semi,Burst,Auto}
    public GunType gunType;
    public float fireRate;

    public Transform bulletSpawn;
    private LineRenderer tracer;
    public Transform shellSpawn;
    public Rigidbody shell;

    private float SecondsBetweenShots;
    private float nextPossibleShootTime;

    private void Start()
    {
        SecondsBetweenShots = 60 / fireRate;
        if (GetComponent<LineRenderer>())
        {
            tracer = GetComponent<LineRenderer>();
        }
    }

    public void Shoot()
    {
        if (CanShoot())
        {
            Ray ray = new Ray(bulletSpawn.position, bulletSpawn.forward);
            RaycastHit hit;

            float shotDistance = 20;

            if (Physics.Raycast(ray, out hit, shotDistance))
            {
                shotDistance = hit.distance;
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
            
        }



       
    }

    public void ShootContinous()
    {
        if (gunType == GunType.Auto)
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
