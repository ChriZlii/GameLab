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
            //Shoutinganimation
            //Shoutingsound
            Cmd_Shoot(netId);
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
                //Shoutinganimation
                //Shoutingsound without bullet
            }
        }
    }


    public void Reaload()
    {

        if (!isLocalPlayer) return;

        GunData gunData = weaponholder.SelectedWeapon.GetComponent<GunData>();

        if (gunData.AmmoCount <= 0) return;

        //Relaod animation

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


    [Command]
    public void Cmd_Shoot(uint PlayerNetID)
    {
        GameObject _player = NetworkIdentity.spawned[PlayerNetID].gameObject;
        GameObject _SelectedWeapon = _player.GetComponent<WeaponholderScript>().SelectedWeapon;
        GunData _gundata = _SelectedWeapon.GetComponent<GunData>();

        GameObject _Bullet = Instantiate(_gundata.BulletPrefab, _gundata.WeaponMuzzle.transform.position, _gundata.WeaponMuzzle.transform.rotation);

        BulletScript _BullData = _Bullet.GetComponent<BulletScript>();
        // Bug Workaround ClickUp ID #2jj8b0
        //_BullData.BulletDamage = _gundata.Damage;
        //_BullData.MaxFlightDistance = _gundata.ShootingDistance;
        //_BullData.PLAYERID = PlayerNetID;

        NetworkServer.Spawn(_Bullet);

        //Rpc_ShootDebug(PlayerNetID);
    }

    [ClientRpc]
    public void Rpc_ShootDebug(uint PlayerNetID)
    {
        Debug.Log("Shooting PlayerID:" + PlayerNetID);
    }
}




