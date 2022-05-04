using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Monomyto/WeaponCreator", order = 1)]
public class WeaponCreator : ScriptableObject
{
    public string weaponName;
    public GameObject weaponPrefab;
    public Vector2[] weaponShootPoints;
    public float weaponDamageBase;
    public int weaponAmmobase;
    public float shootBackPropulsion;
    public float shootCooldownTime;
    public float bulletSpeed;
    public float bulletGravity;
}
