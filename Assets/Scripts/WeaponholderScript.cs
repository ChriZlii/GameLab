using Mirror;
using UnityEngine;

public class WeaponholderScript : NetworkBehaviour
{

    // Publics
    public GameObject Weaponholder = null;

    public bool EnableManualSwitching = true;


    [HideInInspector] public GameObject SelectedWeapon = null;

    [SyncVar] 
    public int selectedWeaponNum = 0;

    
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
        if (EnableManualSwitching)
        {
            // every weaponholder--------------------------------------------------------------------------------------------
            // is weapon changed, change the wepon in hand
            if (selectedWeaponNum != selectedWeaponPrevious)
            {
                SelectWeapon();
                selectedWeaponPrevious = selectedWeaponNum;
            }



            if (!isLocalPlayer) return;
            // only weaponholder----------------------------------------------------------------------------------------------



            int weaponNumber = selectedWeaponNum;
            float scrollwheelInput = controls.Player.SwitchWeapons.ReadValue<Vector2>().y;

            if (scrollwheelInput > 0)
            {
                weaponNumber++;
                if (weaponNumber == Weaponholder.transform.childCount)
                {
                    weaponNumber = 0;
                }
            }
            else if (scrollwheelInput < 0)
            {
                weaponNumber--;
                if (weaponNumber == -1)
                {
                    weaponNumber = Weaponholder.transform.childCount - 1;
                }
            }


            // if weapon changed Publish to server
            if (weaponNumber != selectedWeaponNum)
            {
                selectedWeaponNum = weaponNumber;
                CmdSelectetWeapon(weaponNumber);
            }


            
        }
    }


    void SelectWeapon()
    {
        int count = 0;
        foreach (Transform weapon in Weaponholder.transform)
        {
            if (count++ == selectedWeaponNum)
            {
                weapon.gameObject.SetActive(true);
                SelectedWeapon = weapon.gameObject;
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
        this.selectedWeaponNum = seletion;
    }

}
