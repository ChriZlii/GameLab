using UnityEngine;


[RequireComponent(typeof(WeaponholderScript))]
public class ShootingScript : MonoBehaviour
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
        gunData = weaponholder.SelectedWeapon.GetComponent<GunData>();

        if (controls.Player.Shoot.ReadValue<float>() == 1 && Time.time >= gunData.NextTimeToFire)
        {
            gunData.NextTimeToFire = Time.time + 1f / gunData.FireRate;
            Shoot();
        }
    }





    void Shoot()
    {
        RaycastHit rayHit;
        if (Physics.Raycast(gunData.WeabonMuzzle.transform.position, gunData.WeabonMuzzle.transform.forward, out rayHit, gunData.Distance))
        {
            // hit something
            Debug.Log(rayHit.collider.name);
        }
    }
}




