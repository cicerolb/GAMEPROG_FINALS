using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{


    // Handling
    public float rotationSpeed = 450;
    public float walkSpeed = 5;
    public float runSpeed = 6;
    public float acceleration = 5;
    public float playerHP = 20;

    public bool isShooting = false;
    public bool ammoPickUp = false;
    public bool arIsShooting = false;
    

    // Sytem
    private Quaternion targetRotation;
    private Vector3 currentVelocityMod;

    //public Gun[] guns;


    // Components
    public Transform handHold;
    public Transform handHold2;
    public Gun[] guns;
    public Gun currentGun;
    private CharacterController characterController;
    private Camera cam;

    public AmmoManager ammoManager;
    public Manager manager;

    public int gunEquipped;

    public avatarController avatarController;
    public avatarController[] avatars;
    public Transform playerPosition;

    public Animator anim;

    public bool dead = false;
    public bool died = false;
    public bool avatarChanged = false;

    public Vector3 mousepos;

    public GameObject landMinePreFab;
    public GameObject freezeMinePreFab;



    // Start is called before the first frame update
    void Start()
    {



        characterController = GetComponent<CharacterController>();
        cam = Camera.main;
        EquipGun(0);
        ChangeAvatar(0);
        gunEquipped = 1;
        currentGun = GameObject.FindGameObjectWithTag("Gun").GetComponent<Gun>();
        ammoManager = GameObject.FindGameObjectWithTag("Player").GetComponent<AmmoManager>();
        manager = GameObject.Find("GameManager").GetComponent<Manager>();
        SetupAnimator();


    }

    // Update is called once per frame
    void Update()
    {

        DropTrap();
        //// Get input for movement
        //float horizontal = Input.GetAxis("Horizontal");
        //float vertical = Input.GetAxis("Vertical");

        //// Get the camera's forward and right vectors
        //Vector3 camForward = Camera.main.transform.forward;
        //Vector3 camRight = Camera.main.transform.right;

        //// Flatten the vectors (ignore the y component)
        //camForward.y = 0;
        //camRight.y = 0;

        //// Normalize the vectors to maintain direction
        //camForward.Normalize();
        //camRight.Normalize();

        //// Combine inputs into a single vector based on camera orientation
        //Vector3 desiredMoveDirection = camForward * vertical + camRight * horizontal;

        //// Move the player
        //transform.Translate(desiredMoveDirection * moveSpeed * Time.deltaTime, Space.World);




        if (playerHP == 0 && !dead && !avatarChanged)
        {
            dead = true;
            ChangeAvatar(2);
            avatarChanged = true; // Set the flag to true to prevent further avatar changes
            
            Destroy(currentGun.gameObject);

        }



        if (manager.isReloading == true)
        {
            walkSpeed = 3;
            runSpeed = 3;
        }
        else
        {
            walkSpeed = 5;
            runSpeed = 6;
        }


        MouseControl();
        //WASDControl();
        if (currentGun)
        {
            Combat();
        }

        if (manager.isReloading == false)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                EquipGun(0);
                gunEquipped = 1;
                ChangeAvatar(0);




            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                EquipGun(1);
                gunEquipped = 2;
                ChangeAvatar(1);


            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                EquipGun(2);
                gunEquipped = 3;
                ChangeAvatar(1);


            }


        }





    }

    private void FixedUpdate()
    {
        if (dead == false)
        {
            anim = GameObject.FindGameObjectWithTag("avatar").GetComponent<Animator>();
            anim.SetFloat("Forward", Input.GetAxisRaw("Vertical"));
            anim.SetFloat("Turn", Input.GetAxisRaw("Horizontal"));

        }

    }

    void SetupAnimator()
    {
        anim = GameObject.FindGameObjectWithTag("avatar").GetComponent<Animator>();

    }



    void EquipGun(int i)
    {
        if (currentGun)
        {
            Destroy(currentGun.gameObject);
        }

        if (i == 0)
        {
            currentGun = Instantiate(guns[i], handHold2.position, handHold2.rotation) as Gun;
            currentGun.transform.parent = handHold2;
        }



        if (i != 0)
        {
            currentGun = Instantiate(guns[i], handHold.position, handHold.rotation) as Gun;
            currentGun.transform.parent = handHold;
        }








        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            i = 0;
        }
    }

    void DropTrap()
    {
        if (ammoManager.landMine > 0)
        {
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                Instantiate(landMinePreFab, transform.position - new Vector3(0f, 0.7f, 0f), Quaternion.identity);
                ammoManager.landMine--;
            }
        }

        if (ammoManager.freezeMine > 0)
        {
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                Instantiate(freezeMinePreFab, transform.position - new Vector3(0f, 0.7f, 0f), Quaternion.identity);
                ammoManager.freezeMine--;
            }
        }
           
        
    }

    void ChangeAvatar(int l)
    {
        if (avatarController)
        {
            Destroy(avatarController.gameObject);
        }

        avatarController = Instantiate(avatars[l], playerPosition.position,
        playerPosition.rotation) as avatarController;
        avatarController.transform.parent = playerPosition;

    }






    void MouseControl()
    {
        if (dead == false)
        {

            //Vector3 mousePos = Input.mousePosition;
            //Vector3 screenPos = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.transform.position.y - transform.position.y));

            //// Adjust for isometric view
            //Vector3 isometricOffset = new Vector3(0, -45f, 0); // Adjust Y value as needed
            //Vector3 targetPosition = screenPos + isometricOffset;

            //Vector3 lookDirection = (targetPosition - transform.position).normalized;
            //Quaternion targetRotation = Quaternion.LookRotation(lookDirection, Vector3.up);

            //transform.rotation = Quaternion.Euler(0, targetRotation.eulerAngles.y, 0);



            Vector3 mousePos = Input.mousePosition;
            mousePos = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.transform.position.y - transform.position.y));
            targetRotation = Quaternion.LookRotation(mousePos - new Vector3(transform.position.x, 0, transform.position.z));
            transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);

            Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            Vector3 motion = input;
            motion *= (Mathf.Abs(input.x) == 1 && Mathf.Abs(input.z) == 1) ? .7f : 1;
            motion *= (Input.GetButton("Run")) ? runSpeed : walkSpeed;
            motion += Vector3.up * -8;

            characterController.Move(motion * Time.deltaTime);
        }


    }

    void WASDControl()
    {
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        if (input != Vector3.zero)
        {
            targetRotation = Quaternion.LookRotation(input);
            transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);
        }


        currentVelocityMod = Vector3.MoveTowards(currentVelocityMod, input, acceleration * Time.deltaTime);
        Vector3 motion = input;
        motion *= (Mathf.Abs(input.x) == 1 && Mathf.Abs(input.z) == 1) ? .7f : 1;
        motion *= (Input.GetButton("Run")) ? runSpeed : walkSpeed;
        motion += Vector3.up * -8;

        characterController.Move(motion * Time.deltaTime);
    }

    public void Combat()
    {
        if (Input.GetButtonDown("Shoot"))
        {
            currentGun.Shoot();
            isShooting = !isShooting;

        }

        else if (Input.GetButton("Shoot"))
        {
            currentGun.ShootContinous();



        }
        else if (Input.GetButtonUp("Shoot"))
        {
            isShooting = !isShooting;
        }

        if (Input.GetButtonDown("Shoot") && currentGun.gunType == Gun.GunType.AR)
        {
            arIsShooting = !arIsShooting;

        }

        else if (Input.GetButtonUp("Shoot") && currentGun.gunType == Gun.GunType.AR)
        {
            arIsShooting = !arIsShooting;
        }

        if (arIsShooting && currentGun.gunType == Gun.GunType.AR && currentGun.magazine > 0)
        {
            runSpeed = 2;
            walkSpeed = 2;
        }

        else
        {
            runSpeed = 6;
            walkSpeed = 5;
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("pistolAmmo"))
        {
            ammoPickUp = true;
            ammoManager.pistolMagsLeft++;

            Destroy(other.gameObject);
            StartCoroutine(ResetAmmoPickUp());
        }

        if (other.CompareTag("arAmmo"))
        {
            ammoPickUp = true;
            ammoManager.arMagsLeft++;


            Destroy(other.gameObject);
            StartCoroutine(ResetAmmoPickUp());
        }

        if (other.CompareTag("sniperAmmo"))
        {
            ammoPickUp = true;
            ammoManager.mmMagsLeft++;

            Destroy(other.gameObject);
            StartCoroutine(ResetAmmoPickUp());
        }

        if (other.CompareTag("trapBox"))
        {
            ammoPickUp = true;
            int randomTrap = Random.Range(1, 3);

            switch (randomTrap)
            {
                case 1:
                    ammoManager.landMine++;
                    break;
                case 2:
                    ammoManager.freezeMine++;
                    break;
            }

            Destroy(other.gameObject);
            StartCoroutine(ResetAmmoPickUp());
        }
    }

    public IEnumerator ResetAmmoPickUp()
    {
        yield return new WaitForSeconds(0.1f);
        ammoPickUp = false;
    }
}