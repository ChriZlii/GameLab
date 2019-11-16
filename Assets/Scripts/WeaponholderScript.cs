using Mirror;
using UnityEngine;

public class WeaponholderScript : NetworkBehaviour
{

    // Publics
    public GameObject Weaponholder = null;

    [SyncVar] public int selectedWeapon = 0;
    public bool EnableManulaSelection = false;

    // Privates
    private InputController controls = null;
    private int selectedWeaponPrevious = 0;


    private void Awake() => controls = new InputController();

    private void OnEnable() => controls.Player.Enable();
    private void OnDisable() => controls.Player.Disable();


    void Start()
    {
        SelectWeapon();
    }

    
    void Update()
    {
        if (EnableManulaSelection)
        {
            // every weaponholder--------------------------------------------------------------------------------------------
            // is weapon changed, change the wepon in hand
            if (selectedWeapon != selectedWeaponPrevious)
            {
                SelectWeapon();
                selectedWeaponPrevious = selectedWeapon;
            }



            if (!isLocalPlayer) return;
            // only weaponholder----------------------------------------------------------------------------------------------



            int weaponNumber = selectedWeapon;
            float scrollwheelInput = controls.Player.SwitchWeapons.ReadValue<Vector2>().y;

            if (scrollwheelInput > 0)
            {
                weaponNumber++;
                if (weaponNumber == transform.childCount)
                {
                    weaponNumber = 0;
                }
            }
            else if (scrollwheelInput < 0)
            {
                weaponNumber--;
                if (weaponNumber == -1)
                {
                    weaponNumber = transform.childCount - 1;
                }
            }


            // if weapon changed Publish to server
            if (weaponNumber != selectedWeapon)
            {
                selectedWeapon = weaponNumber;
                CmdSelectetWeapon(weaponNumber);
            }


            
        }
    }


    void SelectWeapon()
    {
        int count = 0;
        foreach (Transform weapon in Weaponholder.transform)
        {
            if (count++ == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
        }

    }



    // publishes the weapon change to the server. The server Syncs it to the rest of holders
    [Command]
    public void CmdSelectetWeapon(int seletion)
    {
        this.selectedWeapon = seletion;
    }

}
