using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    public Transform rotationPivot;
    private Vector3 moveDirection = Vector3.zero;
    CharacterController controller;

    private void Start()
    {
         controller = GetComponent<CharacterController>();
    }
    void Update()
    {
        //Rotate the player based on direction input
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            //Face player towards where the rotationPivot is facing 
            transform.rotation = Quaternion.Euler(0f, rotationPivot.rotation.eulerAngles.y, 0f);
        }
        if (controller.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
            if (Input.GetKeyDown(KeyCode.Space))
                moveDirection.y = jumpSpeed;

        }
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }
}
