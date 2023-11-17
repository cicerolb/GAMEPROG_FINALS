using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoManager : MonoBehaviour
{


    public float mmMag = 5;
    public float mmMagSize = 5;
    public float mmMagsLeft = 1;

    public float arMag = 45;
    public float arMagSize = 45;
    public float arMagsLeft = 2;

    public float pistolMag = 10;
    public float pistolMagSize = 10;
    public float pistolMagsLeft = 1;

    public float weapon;

    public PlayerMovement playerMovement;

    public bool isShooting;

    public float landMine = 5;
    public float freezeMine = 0;

 



    // Start is called before the first frame update
    public void Start()
    {
        playerMovement = GameObject.Find("player").GetComponent<PlayerMovement>();



        pistolMag = 10;
        pistolMagSize = 10;
        pistolMagsLeft = 1;

        mmMag = 3;
        mmMagSize = 3;
        mmMagsLeft = 1;

        arMag = 45;
        arMagSize = 45;
        arMagsLeft = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (pistolMagsLeft < 0)
        {
            pistolMagsLeft = 0;
        }

        if (mmMagsLeft < 0)
        {
            mmMagsLeft = 0;
        }

        if (arMagsLeft < 0)
        {
            arMagsLeft = 0;
        }
    }
}
