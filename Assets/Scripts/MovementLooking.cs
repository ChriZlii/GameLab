using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementLooking : MonoBehaviour
{
    // Public editable Variables
    public GameObject head;
    public CharacterController controller;


    // Private editable Variables
    [SerializeField] float MouseSensitivity = 100f;
    [SerializeField] float movementSpeed = 5f;
    [SerializeField] float jumpHeight = 1f;
    [SerializeField] float GravityMulti = 1f;


    // Private Variables
    private InputController controls = null;
    private float xRotation = 90f;
    private bool isGrounded = false;
    public  Vector3 velocity;




    private void Awake()
    {
        controls = new InputController();
        Cursor.lockState = CursorLockMode.Locked;

        velocity = Vector3.zero;
    }


    private void OnEnable() => controls.Player.Enable();
    private void OnDisable() => controls.Player.Disable();
    

    private void Update()
    {
        Gravity();
        Move();
        Look();
    }

    private void Look()
    {
        //Body Rotation LEFT/RIGHT
        Vector2 deltaMouse = controls.Player.Look.ReadValue<Vector2>() * MouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * deltaMouse.x);

        // Camera Rotation UP/DOWN
        this.xRotation -= deltaMouse.y;
        this.xRotation = Mathf.Clamp(this.xRotation, -90f, 90f);
        head.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }

    public void Jump()
    {
        if (isGrounded)
        {
            velocity.y += Mathf.Sqrt(jumpHeight * Physics.gravity.y * -1f);
        }
    }

    private void Gravity()
    {
        isGrounded = Physics.Raycast(controller.transform.position, -Vector3.up, 1.1f * controller.transform.localScale.y);
        velocity.y += Physics.gravity.y * GravityMulti * Time.deltaTime;

        if (isGrounded && velocity.y < 0)
        {
            velocity = new Vector3(0, -1f, 0);
        }
    }

    private void Move()
    {
        if (isGrounded)
        {
            Vector2 movementInput = controls.Player.Movement.ReadValue<Vector2>();
            float movementRun = controls.Player.Run.ReadValue<float>();

            Vector3 movement = (transform.right * movementInput.x + transform.forward * movementInput.y).normalized * movementSpeed * (1 + movementRun * 1.0f);
            velocity.x = movement.x;
            velocity.z = movement.z;
        }

        controller.Move(velocity * Time.deltaTime);
    }


    
}
