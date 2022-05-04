using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUtils : MonoBehaviour
{
    public static void RecieveAmmo(Collider2D col, GameObject ammoObject)
    {
        col.gameObject.GetComponent<PlayerController>().weapon.ammo += col.gameObject.GetComponent<PlayerController>().Weapon.weaponAmmobase;
        Destroy(ammoObject);
    }
    public static void DestroyObject(GameObject destroyParticle, Transform pos, GameObject destroyGameObject)
    {
        Instantiate(destroyParticle, pos.position, Quaternion.identity);
        Destroy(destroyGameObject);
    }
    public static void AddScore(PlayerController player, int Points, GameObject owner = null)
    {
        if(owner != null)
            if(owner.name == "Player")
                player.player.points += Points;
    }
}
