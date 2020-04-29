using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponData : MonoBehaviour
{
    public GameObject Weapon;
    public GameObject WeaponMuzzle;
    public ParticleSystem MuzzleParticles;
    public GameObject BulletImpact;

    [Header("Weapon Data")]
    public Transform RightHandGrip;
    public Transform LeftHandGrip;

    [Header("Weapon Data")]
    public WeaponTypes weaponType;
    public float ShootingDistance = 100f;
    public float Damage = 10f;
    public float FireRate = 10f;


    [Header("Mag Data")]
    public int MagSize = 15;
    public int AmmoLoaded = 0;
    public int AmmoCount = 1000;

    [HideInInspector]public float NextTimeToFire = 0;

    //Sturcturen--------------------------------------------------------------
    
}

public enum WeaponTypes
{
    PISTOL,
    RIFEL,
    HEAVY,
    SNIPER
}
