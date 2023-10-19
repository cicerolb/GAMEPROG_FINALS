using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellManager : MonoBehaviour
{

    private float lifeTime = 5;

    private Material mat;
    private Color originalCol;
    private float fadePercent;
    private float deathTime;
    private bool fading;


    // Start is called before the first frame update
    void Start()
    {
        //Renderer renderer = GetComponent<Renderer>();
        //originalCol = mat.color;
        //mat = renderer.material;
        //deathTime = Time.time + lifeTime;
        //StartCoroutine("Fade");
        StartCoroutine("DeSpawn");
    }


    //IEnumerator Fade()
    //{
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(.2f);
    //        if (fading)
    //        {
    //            fadePercent += Time.deltaTime;
    //            mat.color = Color.Lerp(originalCol, Color.clear, fadePercent);

    //            if (fadePercent >= 1)
    //            {
    //                Destroy(gameObject);
    //            }
    //        }

    //        else if (Time.time > deathTime)
    //        {
    //            fading = true;
    //        }
    //    }
    //}


    IEnumerator DeSpawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground")
        {
            Rigidbody rigidBody = GetComponent<Rigidbody>();
            rigidBody.Sleep();

            DeSpawn();

        }
    }
}
