using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyScript : MonoBehaviour
{
    public AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        StartCoroutine(DestroyDelay());
        audio.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DestroyDelay()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
