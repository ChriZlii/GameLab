using Mirror;
using System.Collections;
using System.Collections.Generic;
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
            //MuzzleFlash

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


        Ray ray = new Ray(gundata.WeaponMuzzle.transform.position, gundata.WeaponMuzzle.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, gundata.ShootingDistance))
        {
            //spawn/despawn impact
            //GameObject bulletImpact = Instantiate(gundata.BulletImpact, position: hit.point, Quaternion.LookRotation(hit.normal));
            Quaternion quant = Quaternion.LookRotation(hit.normal);
            CmdSpawnImpact(netId, hit.point.x, hit.point.y, hit.point.z, quant.eulerAngles.x, quant.eulerAngles.y, quant.eulerAngles.z);

            //notify hitted component to be hitted (only items with networkidentity have syncroniced Health, so only they can be hitten)
            NetworkIdentity identityPlayerHit = hit.collider.GetComponent<NetworkIdentity>();
            if (identityPlayerHit != null)
            {
                Cmd_Hit(this.netId, identityPlayerHit.netId, gundata.Damage);
            }

            Debug.DrawRay(gundata.WeaponMuzzle.transform.position, gundata.WeaponMuzzle.transform.forward * hit.distance, Color.blue, 2f);
        }
        else
        {
            Debug.DrawRay(gundata.WeaponMuzzle.transform.position, gundata.WeaponMuzzle.transform.forward * gundata.ShootingDistance, Color.red, 1f);
        }
    }





    [Command]
    private void Cmd_Hit(uint netIDFrom, uint netIDTo, float damage)
    {
        List<object> messageData = new List<object>();
        messageData.Add(netIDFrom);
        messageData.Add(damage);
        NetworkIdentity.spawned[netIDTo].gameObject.SendMessage("Msg_HIT", messageData, SendMessageOptions.RequireReceiver);
    }


    IEnumerator DespawnAfter1s(uint netID)
    {
        yield return new WaitForSeconds(1f);
        CmdDespawn(netID);
    }


    [Command]
    private void CmdSpawnImpact(uint netID, float x, float y, float z, float eulerx, float eulery, float eulerz)
    {
        GameObject player = NetworkIdentity.spawned[netID].gameObject;
        GameObject SelectedWeapon = weaponholder.SelectedWeapon;
        GunData gundata = SelectedWeapon.GetComponent<GunData>();

        GameObject impact = Instantiate(gundata.BulletImpact, new Vector3(x, y, z), Quaternion.Euler(eulerx, eulery, eulerz));

        NetworkServer.Spawn(impact);

        StartCoroutine("DespawnAfter1s", impact.GetComponent<NetworkIdentity>().netId);
    }

    [Command]
    private void CmdDespawn(uint netID)
    {
        GameObject gm = NetworkIdentity.spawned[netID].gameObject;
        NetworkServer.Destroy(gm);
    }






}




