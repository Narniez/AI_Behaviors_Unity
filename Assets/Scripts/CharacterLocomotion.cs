using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLocomotion : MonoBehaviour
{
    public float turnSpeed = 15;
    public float moveSpeed = 1;

    Camera mainCamera;

    Animator animator;
    Vector2 input;
    void Start()
    {
        animator = GetComponent<Animator>();
        mainCamera = Camera.main;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");

        animator.SetFloat("InputX", input.x);
        animator.SetFloat("InputY", input.y);
    }

    private void FixedUpdate()
    {
        CharacterDirection();
    }

    private void CharacterDirection()
    {
        float yCamera = mainCamera.transform.rotation.eulerAngles.y;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yCamera, 0), turnSpeed * Time.fixedDeltaTime);

    }
}
