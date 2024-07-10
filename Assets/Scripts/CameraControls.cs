using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;
    public Vector3 offset;
    public float rotateSpeed;

    public Transform rotationPivot;

    void Start()
    {
        offset = target.position - transform.position;

        //Move the pivot wherever the player is r 
        rotationPivot.position = target.position;
        rotationPivot.transform.parent = null;

    }

    // Update is called once per frame
    void Update()
    {
        //Pivot point always moves with the player 
        rotationPivot.transform.position = target.transform.position;

        //Get the X position of the mouse and rotate the rotationPivot 
        float horizontal = Input.GetAxis("Mouse X") * rotateSpeed;
        rotationPivot.Rotate(0, horizontal, 0);

        //Get the Y position of the mouse and rotate the rotationPivot 
        float vertical = Input.GetAxis("Mouse Y") * rotateSpeed;
        rotationPivot.Rotate(-vertical, 0, 0);

        //Limit the up and down camera rotaion
        if (rotationPivot.rotation.eulerAngles.x > 45f && rotationPivot.rotation.eulerAngles.x < 180)
        {
            rotationPivot.rotation = Quaternion.Euler(45f, 0, 0);
        }

        //Move the camera based on the current rotation of the target and the original offset     
        Quaternion rotation = Quaternion.Euler(rotationPivot.eulerAngles.x, rotationPivot.eulerAngles.y, 0);
        transform.position = target.position - (rotation * offset);

        // Makes the camera unable to go below the player and the world
        if (transform.position.y < target.position.y)
        {
            transform.position = new Vector3(transform.position.x, target.position.y - .5f, transform.position.z);
        }

        transform.LookAt(target);
    }
}
