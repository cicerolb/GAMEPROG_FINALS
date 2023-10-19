using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    // Handling
    public float rotationSpeed = 450;
    public float walkSpeed = 5;
    public float runSpeed = 10;
    public float acceleration = 5;

    public bool isShooting = false;

    // Sytem
    private Quaternion targetRotation;
    private Vector3 currentVelocityMod;

    // Components
    public Gun gun;
    private CharacterController characterController;
    private Camera cam;


    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        MouseControl();
        //WASDControl();

        if (Input.GetButtonDown("Shoot"))
        {
            gun.Shoot();
            isShooting = !isShooting;

        }

        else if (Input.GetButton("Shoot"))
        {
            gun.ShootContinous();



        }
        else if (Input.GetButtonUp("Shoot"))
        {
            isShooting = !isShooting;
        }

        if (isShooting)
        {
            runSpeed = 2;
            walkSpeed = 2;
        }

        else
        {
            runSpeed = 10;
            walkSpeed = 5;
        }
    }


    

    void MouseControl()
    {

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
}