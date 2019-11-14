using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{

    public GameObject WeabonMuzzle;
    public Camera FPSCamera;


    public float Damage = 15f;
    public float Distance = 100f;
    public float FireRate = 10f;


    // Private Variables
    private InputController controls = null;

    private float nextTimeToFire = 0;



    private void Awake()
    {
        controls = new InputController();
    }
    private void OnEnable() => controls.Player.Enable();
    private void OnDisable() => controls.Player.Disable();



    // Update is called once per frame
    void Update()
    {
        if (controls.Player.Shoot.ReadValue<float>() == 1 && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / FireRate;
            Shoot();
        }
    }





    void Shoot()
    {
        RaycastHit rayHit;
        if (Physics.Raycast(WeabonMuzzle.transform.position, FPSCamera.transform.forward, out rayHit, Distance))
        {
            // hit something
            Debug.Log(rayHit.collider.name);
        }
    }
}




