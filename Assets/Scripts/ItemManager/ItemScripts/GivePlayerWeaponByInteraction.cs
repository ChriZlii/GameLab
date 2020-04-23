using Mirror;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static InputController;

public class GivePlayerWeaponByInteraction : ItemBehaviour, IPlayerActions
{
    //Public-----------------------------------------------------------------------------------

    public WeaponTypes weaponType;

    //Private----------------------------------------------------------------------------------
    private InputController inputControls;

    private bool isInRange = false;
    private GameObject PlayerInRange;


    private void Awake()
    {
        inputControls = new InputController();
        inputControls.Player.SetCallbacks(this);
    }

    private void OnEnable() => inputControls.Player.Enable();
    private void OnDisable() => inputControls.Player.Disable();




    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameObject player = other.gameObject;
            NetworkIdentity identity = player.GetComponent<NetworkIdentity>();

            if (identity.isLocalPlayer)
            {
                isInRange = true;
                PlayerInRange = player;

                // debug
                Renderer renderer = GetComponent<Renderer>();
                renderer.material.SetColor("_Color", Color.cyan);

            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameObject player = other.gameObject;
            NetworkIdentity identity = player.GetComponent<NetworkIdentity>();

            if (identity.isLocalPlayer)
            {
                isInRange = false;
                PlayerInRange = null;

                // debug
                Renderer renderer = GetComponent<Renderer>();
                renderer.material.SetColor("_Color", Color.green);
            }
        }
    }


    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (isInRange && PlayerInRange != null)
            {
                Activate(PlayerInRange);
            }
        }
    }



    // gives the player the weapon
    private void Activate(GameObject player)
    {
        List<object> messageData = new List<object>
                {
                    weaponType,         // type of weapon
                    true                // true = give / false = take ... weapon
                };
        if (destroyOnUse) messageData.Add(netId); // netid of the Item

        player.SendMessage("Msg_GiveWeapon", messageData, SendMessageOptions.RequireReceiver);
    }

    #region unused PlayerInput Events
    public void OnJump(InputAction.CallbackContext context) { }

    public void OnMovement(InputAction.CallbackContext context) { }

    public void OnRun(InputAction.CallbackContext context) { }

    public void OnLook(InputAction.CallbackContext context) { }

    public void OnShoot(InputAction.CallbackContext context) { }

    public void OnScope(InputAction.CallbackContext context) { }

    public void OnSwitchWeapons(InputAction.CallbackContext context) { }

    public void OnReload(InputAction.CallbackContext context) { }

    #endregion
}

