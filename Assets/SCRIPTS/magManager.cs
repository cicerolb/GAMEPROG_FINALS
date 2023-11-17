using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine("DeSpawn");
    }


  


    IEnumerator DeSpawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            Destroy(gameObject);
        }
    }
}
