using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunData : MonoBehaviour
{

    public GameObject WeabonMuzzle;

    [Header("Weapon Data")]
    public float Damage = 15f;
    public float Distance = 100f;
    public float FireRate = 10f;
    public float NextTimeToFire = 0;
}
