using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponholderScript : MonoBehaviour
{

    // Publics
    public int selectedWeapon = 0;
    public bool EnableManulaSelection = false;

    // PRivates
    private InputController controls = null;


    private void Awake() => controls = new InputController();

    private void OnEnable() => controls.Player.Enable();
    private void OnDisable() => controls.Player.Disable();


    void Start()
    {
        SelectWeapon(selectedWeapon);
    }

    
    void Update()
    {
        if (EnableManulaSelection)
        {
            float scrollwheelInput = controls.Player.SwitchWeapons.ReadValue<Vector2>().y;

            if (scrollwheelInput > 0)
            {
                selectedWeapon++;
                if (selectedWeapon == transform.childCount)
                {
                    selectedWeapon = 0;
                }
            }
            else if (scrollwheelInput < 0)
            {
                selectedWeapon--;
                if (selectedWeapon == -1)
                {
                    selectedWeapon = transform.childCount - 1;
                }
            }

            SelectWeapon(selectedWeapon);
        }
    }


    void SelectWeapon(int weaponIndex)
    {
        int count = 0;
        foreach (Transform weapon in transform)
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
}
