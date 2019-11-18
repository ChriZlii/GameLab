using Mirror;
using UnityEngine;


[RequireComponent(typeof(WeaponholderScript))]
public class ShootingScript : NetworkBehaviour
{
    //Public Variables

    // Private Variables
    private InputController controls = null;
    private WeaponholderScript weaponholder = null;
    private GunData gunData = null;



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


        gunData = weaponholder.SelectedWeapon.GetComponent<GunData>();

        if (controls.Player.Shoot.ReadValue<float>() == 1 && Time.time >= gunData.NextTimeToFire)
        {
            gunData.NextTimeToFire = Time.time + 1f / gunData.FireRate;
            Cmd_Shoot();
        }
    }




    [Command]
    void Cmd_Shoot()
    {
        GameObject Bullet = Instantiate(gunData.BulletPrefab, gunData.WeabonMuzzle.transform.position, gunData.WeabonMuzzle.transform.rotation);

        BulletScript BullData = Bullet.GetComponent<BulletScript>();
        //BullData.MaxFlightDistance = gunData.ShootingDistance;
        BullData.PLAYERID = netId;

        NetworkServer.Spawn(Bullet);

        Rpc_ShootDebug();
    }

    [ClientRpc]
    void Rpc_ShootDebug()
    {
        Debug.Log("Shooting " + netId);
    }
}




