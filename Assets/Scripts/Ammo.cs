using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col) {
        if(col.tag == "Player")
            GameUtils.RecieveAmmo(col, gameObject);
    }
}
