using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    CharacterController characterController;
    private Camera cam;
    Rigidbody rigidBody;
    public Animator anim;

    public float rotationSpeed = 1000;
    public float speed = 4;
    public float runSpeed = 6;
    public float walkSpeed = 5;

    public avatarChanger avatarChanger;
    public avatarChanger[] avatars;

    public Transform playerPosition;



    private Quaternion targetRotation;
    // Start is called before the first frame update
    void Start()
    {
        ChangeAvatar(0);
        characterController = GetComponent<CharacterController>();
        rigidBody = GetComponent<Rigidbody>();
        cam = Camera.main;
        SetupAnimator();
        
    }

    // Update is called once per frame
    void Update()
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


        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            
            ChangeAvatar(0);
            
            
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeAvatar(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeAvatar(2);
        }

       

    }

    private void FixedUpdate()
    {
        anim = GameObject.FindGameObjectWithTag("avatar").GetComponent<Animator>();
        anim.SetFloat("Forward", Input.GetAxisRaw("Vertical"));
        anim.SetFloat("Turn", Input.GetAxisRaw("Horizontal"));
    }

    void SetupAnimator()
    {
        anim = GameObject.FindGameObjectWithTag("avatar").GetComponent<Animator>();

        foreach(var childAnimator in GetComponentsInChildren<Animator>())
        {
            if (childAnimator != anim)
            {
                anim.avatar = childAnimator.avatar;
                Destroy(childAnimator);
                break;
            }
        }
     
    }

    void ChangeAvatar(int i)
    {
        if (avatarChanger)
        {
            Destroy(avatarChanger.gameObject);
        }

        avatarChanger = Instantiate(avatars[i], playerPosition.position,
            playerPosition.rotation) as avatarChanger;
        avatarChanger.transform.parent = playerPosition;
    }
}
