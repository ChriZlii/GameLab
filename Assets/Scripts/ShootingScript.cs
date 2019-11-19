using Mirror;
using UnityEngine;

[RequireComponent(typeof(NetworkIdentity))]
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
            Cmd_Shoot(netId);
        }
    }




    [Command]
    public void Cmd_Shoot(uint PlayerNetID)
    {
        GameObject _player = NetworkIdentity.spawned[PlayerNetID].gameObject;
        GameObject _SelectedWeapon = _player.GetComponent<WeaponholderScript>().SelectedWeapon;
        GunData _gundata = _SelectedWeapon.GetComponent<GunData>();

        GameObject _Bullet = Instantiate(_gundata.BulletPrefab, _gundata.WeabonMuzzle.transform.position, _gundata.WeabonMuzzle.transform.rotation);

        BulletScript _BullData = _Bullet.GetComponent<BulletScript>();
        //_BullData.MaxFlightDistance = _gunData.ShootingDistance;
        _BullData.PLAYERID = PlayerNetID;

        NetworkServer.Spawn(_Bullet);

        Rpc_ShootDebug(PlayerNetID);
    }

    [ClientRpc]
    public void Rpc_ShootDebug(uint PlayerNetID)
    {
        Debug.Log("Shooting PlayerID:" + PlayerNetID);
    }
}




