using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Vector3 cameraRotation;
    private Vector3 cameraTarget;
    private Transform target;
    public PlayerMovement playerMovement;

    public float zoomedInFOV = 50.0f; // Field of View when zoomed in
    public float normalFOV = 60.0f;  // Default Field of View
    public float zoomSpeed = 2.0f;



    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
       
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();



    }

    // Update is called once per frame
    void Update()
    {

        cameraTarget = new Vector3(target.position.x, transform.position.y, target.position.z);
        transform.position = Vector3.Lerp(transform.position, cameraTarget, Time.deltaTime * 1);



        //cameraTarget = new Vector3(target.position.x -2, transform.position.y, target.position.z-2);

        //if (playerMovement.avatarChanged == true)
        //{
        //    cameraTarget = new Vector3(target.position.x, transform.position.y, target.position.z);

        //}
        // transform.position = Vector3.Lerp(transform.position, cameraTarget, Time.deltaTime * 8);

        //if (playerMovement.avatarChanged)
        //{
        //    SmoothZoom(zoomedInFOV);
        //}
        //else
        //{
        //    SmoothZoom(normalFOV);
        //}



    }
    void SmoothZoom(float targetFOV)
    {
        //Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, targetFOV, Time.deltaTime * zoomSpeed);
        // You might want to also adjust other camera settings like position, rotation, etc.
    }


    void ZoomIn()
    {
        //Camera.main.fieldOfView = zoomedInFOV;
        // You might want to also adjust other camera settings like position, rotation, etc.
    }

    void ZoomOut()
    {
        //Camera.main.fieldOfView = normalFOV;
        // Reset any other camera settings you changed in ZoomIn()
    }

}
  