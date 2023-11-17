
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public Gun currentGun;
    public PlayerMovement playerMovement;
    public ZombieScript zombieScript;
    public AudioSource damageSound;
    public AudioSource zombieEating;
    public enemySpawnScript enemySpawn;
    public AmmoManager ammoManager;

    public float enemySpawned = 0;
    public float maxEnemy;
    public float kills = 0;

    public float magazineValue;
    public float magsLeft;
    public bool isReloading;
    public float currentHP;
    public bool isDamaged = false;
    public bool roundStarted = false;
    public int roundNumber;
    public bool roundFinished = false;

    public GameObject[] zombies;


    //MAIN MENU
    public GameObject startMenu;
    public GameObject menu;
    public GameObject back;
    public GameObject creator;

    //IN-GAME
    public TextMeshProUGUI ammoTMP;
    public TextMeshProUGUI ammoDisplay;
    public TextMeshProUGUI magsLabel;
    public TextMeshProUGUI healthDisplay;
    public GameObject damageScreen;
    public GameObject gameOver;
    public GameObject ammo;
    public GameObject hP;
    public TextMeshProUGUI round;
    public TextMeshProUGUI iceMineLabel;
    public TextMeshProUGUI mineLabel;


    public bool intermission = false;




    void Start()
    {
        playerMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        damageSound = GetComponent<AudioSource>();
        zombieEating = GameObject.Find("ZombieEating").GetComponent<AudioSource>();
        ammoManager = GameObject.Find("player").GetComponent<AmmoManager>();


        magazineValue = 10;
        magsLeft = 1;


        Time.timeScale = 0f;
        back.SetActive(false);
        creator.SetActive(false);
        startMenu.SetActive(true);
        gameOver.SetActive(false);

        roundNumber = 1;

    }

    // Update is called once per frame
    public void Update()
    {

        

        zombies = GameObject.FindGameObjectsWithTag("Zombie");

        if (zombies.Length == 0 && roundStarted == true)
        {
            intermission = true;
            roundStarted = false;
            StartCoroutine(IntermissionDelay());
        }

        iceMineLabel.text = "" + ammoManager.freezeMine;
        mineLabel.text = "" + ammoManager.landMine;

       

        if (isReloading == false)
        {
            ammoDisplay.text = "Ammo:   " + magazineValue;
            magsLabel.text = "MAGS:   " + magsLeft;
        }
        else
        {

            ammoDisplay.text = "Ammo: Reloading";
        }

        healthDisplay.text = "HP: " + playerMovement.playerHP;

        if (intermission == false)
        {
            round.text = "Round: " + roundNumber;
        }
        else
        {
            round.text = "Round End";
        }
       

        if (isDamaged == true)
        {
            damageSound.Play();
            StartCoroutine(isDamagedReset());
            damageScreen.SetActive(true);

        }
        else
        {
            damageScreen.SetActive(false);
        }

        if (playerMovement.avatarChanged == true)
        {
            hP.SetActive(false);
            ammo.SetActive(false);
            gameOver.SetActive(true);
            Debug.Log("Zombie Eating Play");
            zombieEating.Play();
        }

    }

    IEnumerator isDamagedReset()
    {
        yield return new WaitForSeconds(0.2f);
        isDamaged = false;
    }
   

    public void Play()
    {
        Time.timeScale = 1f;
        startMenu.SetActive(false);
    }

    public void Credits()
    {
        menu.SetActive(false);
        back.SetActive(true);
        creator.SetActive(true);
    }

    public void Back()
    {
        back.SetActive(false);
        creator.SetActive(false);
        menu.SetActive(true);
    }


    IEnumerator IntermissionDelay()
    {
        yield return new WaitForSeconds(8f);
        intermission = false; 
        roundFinished = true;
        roundNumber++;
    }
  
}
