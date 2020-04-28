using Mirror;
using UnityEngine;
using UnityEngine.InputSystem;
using static InputController;

public class MovementLooking : NetworkBehaviour, IPlayerActions
{
    // Public editable Variables
    public GameObject head;
    public CharacterController controller;
    public HumanoidAnimationCntrlScript humanoidControl;


    // Private editable Variables
    [SerializeField] float MouseSensitivity = 100f;
    [SerializeField] float movementSpeed = 1f;      // m/s
    [SerializeField] float runningSpeed = 3f;      // m/s
    [SerializeField] float jumpHeight = 1f;         // m
    [SerializeField] float GravityMulti = 1f;


    // Private Variables
    private InputController inputControls = null;
    private float xRotation = 0;
    private bool isGrounded = false;
    public Vector3 velocity;




    private void Awake()
    {
        inputControls = new InputController();
        inputControls.Player.SetCallbacks(this);
        velocity = Vector3.zero;
    }


    private void OnEnable() => inputControls.Player.Enable();
    private void OnDisable() => inputControls.Player.Disable();


    private void LateUpdate()
    {
        if (isLocalPlayer)
        {
            Gravity();
            Move();
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
            Vector2 movementInput = inputControls.Player.Movement.ReadValue<Vector2>();

            Vector3 movement;
            if (inputControls.Player.Run.ReadValue<float>() == 0) // normal walk
            {
                movement = (transform.right * movementInput.x + transform.forward * movementInput.y).normalized * movementSpeed;
                humanoidControl.MoveBodyForward(movementInput.y*5);
                humanoidControl.MoveBodySideward(movementInput.x * 5);
            }
            else // running
            {
                movement = (transform.right * movementInput.x + transform.forward * movementInput.y).normalized * runningSpeed;
                humanoidControl.MoveBodyForward(movementInput.y * 10);
                humanoidControl.MoveBodySideward(movementInput.x * 5);
            }
            
            velocity.x = movement.x;
            velocity.z = movement.z;
        }
        controller.Move(velocity * Time.deltaTime);
    }




    // InputSystem Event
    public void OnJump(InputAction.CallbackContext context)
    {
        if (isLocalPlayer)
        {
            if (context.phase == InputActionPhase.Started)
            {
                if (isGrounded)
                {
                    velocity.y += Mathf.Sqrt(jumpHeight * Physics.gravity.y * -1f);
                }
            }
        }
    }






    // InputSystem Event
    public void OnLook(InputAction.CallbackContext context)
    {
        if (isLocalPlayer)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                //Body Rotation LEFT/RIGHT
                Vector2 deltaMouse = inputControls.Player.Look.ReadValue<Vector2>() * MouseSensitivity * Time.deltaTime;
                transform.Rotate(Vector3.up * deltaMouse.x);

                // Camera Rotation UP/DOWN
                this.xRotation -= deltaMouse.y;
                this.xRotation = Mathf.Clamp(this.xRotation, -40f, 70f);
                head.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
            }
        }
    }






    #region Unused InputSystemCallbacks in this File
    // InputSystem Event
    public void OnMovement(InputAction.CallbackContext context) { }

    public void OnRun(InputAction.CallbackContext context) { }

    public void OnShoot(InputAction.CallbackContext context) { }

    public void OnScope(InputAction.CallbackContext context) { }

    public void OnSwitchWeapons(InputAction.CallbackContext context) { }

    public void OnReload(InputAction.CallbackContext context) { }

    public void OnInteract(InputAction.CallbackContext context) { }

    #endregion
}
