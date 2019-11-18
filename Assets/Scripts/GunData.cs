using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunData : MonoBehaviour
{

    public GameObject WeabonMuzzle;
    public ParticleSystem MuzzleParticles;
    public GameObject BulletPrefab;


    [Header("Weapon Data")]
    public float ShootingDistance = 100f;
    public float FireRate = 10f;
    [HideInInspector]public float NextTimeToFire = 0;


}
