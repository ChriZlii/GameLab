using Mirror;
using UnityEngine;

[RequireComponent(typeof(NetworkIdentity))]
[RequireComponent(typeof(WeaponholderScript))]
public class ShootingScript : NetworkBehaviour
{
    //Public Variables
    public bool AutoReload = true;

    // Private Variables
    private InputController controls = null;
    private WeaponholderScript weaponholder = null;
    //private GunData gunData = null;



    private void Awake()
    {
        weaponholder = GetComponent<WeaponholderScript>();
        controls = new InputController();
    }
    private void OnEnable() => controls.Player.Enable();
    private void OnDisable() => controls.Player.Disable();



    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) return;


        GunData gunData = weaponholder.SelectedWeapon.GetComponent<GunData>();

        // if nex time to fire is reached
        if (controls.Player.Shoot.ReadValue<float>() == 1 && Time.time >= gunData.NextTimeToFire)
        {
            // calc new time to fire
            gunData.NextTimeToFire = Time.time + 1f / gunData.FireRate;

            //shoot
            Shoot(gunData);
        }


        if (controls.Player.Reload.ReadValue<float>() == 1)
        {
            Reaload();
        }

    }


    private void Shoot(GunData gunData)
    {

        if (gunData.AmmoLoaded > 0)
        {
            //Shouting Animation
            //Shouting Sound
            ShootRayCast();
            gunData.AmmoLoaded--;
        }
        else
        {
            if (AutoReload)
            {
                Reaload();
            }
            else
            {
                //Shouting Animation
                //Shouting Sound without bullet
            }
        }
    }


    public void Reaload()
    {

        if (!isLocalPlayer) return;

        GunData gunData = weaponholder.SelectedWeapon.GetComponent<GunData>();

        if (gunData.AmmoCount <= 0) return;

        //Relaod Animation

        int loadamount = gunData.MagSize - gunData.AmmoLoaded;

        if (gunData.AmmoCount > loadamount)
        {
            gunData.AmmoLoaded = gunData.MagSize;
            gunData.AmmoCount -= loadamount;
        }
        else
        {
            gunData.AmmoLoaded = gunData.AmmoCount;
            gunData.AmmoCount = 0;
        }
    }



    
    public void ShootRayCast()
    {
        GameObject player = gameObject;
        GameObject SelectedWeapon = weaponholder.SelectedWeapon;
        GunData gundata = SelectedWeapon.GetComponent<GunData>();

        //TODO here shooting RayCast!
        Debug.LogWarning("implementing RayCast missing");

        //Rpc_ShootDebug(PlayerNetID);
    }

    [ClientRpc]
    public void Rpc_ShootDebug(uint PlayerNetID)
    {
        Debug.Log("Shooting PlayerID:" + PlayerNetID);
    }
}




