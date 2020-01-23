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
                incrementWeaponNumber(ref weaponNumber);
            }
            else if (scrollwheelInput < 0)
            {
                decrementWeaponNumber(ref weaponNumber);
            }


            // if weapon changed Publish to server
            if (weaponNumber != selectedWeaponNum)
            {
                selectedWeaponNum = weaponNumber;
                CmdSelectetWeapon(weaponNumber);
            }
        }
    }


    // increments the weaponnumber and check if the player has the weapon
    private void incrementWeaponNumber( ref int weaponNumber)
    {
        //incrementWeaponNumber weapon in circle
        weaponNumber++;
        if (weaponNumber == Weaponholder.transform.childCount)
        {
            weaponNumber = 0;
        }

        //check if play has the weapon
        if (!this.hasWeapon(weaponNumber))
        {
            incrementWeaponNumber(ref weaponNumber);
        }
    }

    // decrements the weaponnumber and check if the player has the weapon
    private void decrementWeaponNumber( ref int weaponNumber)
    {
        //decrementWeaponNumber weapon in circle
        weaponNumber--;
        if (weaponNumber == -1)
        {
            weaponNumber = Weaponholder.transform.childCount - 1;
        }

        //check if play has the weapon
        if (!this.hasWeapon(weaponNumber))
        {
            decrementWeaponNumber(ref weaponNumber);
        }
    }



    // returns true if the player has the weapon with number
    bool hasWeapon(int weaponNumber)
    {
        int count = 0;
        foreach (Transform weapon in Weaponholder.transform)
        {
            if (count++ == weaponNumber)
            {
                if (weapon.gameObject.activeInHierarchy)
                {
                    //Debug.Log("Has weapon");
                    return true;
                }
                else
                {
                    //Debug.Log("Has not weapon");
                    return false;
                }
            }
        }
        throw new System.ArgumentException("Some Error in Weaponswitching occours!!", "Weapon");
    }



    void SelectWeapon()
    {
        int count = 0;
        foreach (Transform weapon in Weaponholder.transform)
        {
            if (count++ == selectedWeaponNum)
            {
                weapon.GetComponent<GunData>().Weapon.SetActive(true);
                SelectedWeapon = weapon.gameObject;
            }
            else
            {
                weapon.GetComponent<GunData>().Weapon.SetActive(false);
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
