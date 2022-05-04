using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objects : ObjectsBase 
{
    void Start()
    {
        objectInfos.rb = GetComponent<Rigidbody2D>();
        SetupObject();
    }
    void Update()
    {
        RotationAndVelocity();
        CheckObjectLife();
        Hud();
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.tag == "Damage") {
            float damage = col.gameObject.GetComponent<BulletController>().bulletDamage;
            TakeDamage(damage, col.gameObject.GetComponent<BulletController>().owner);
        }
        else if(col.collider.tag == "Player") {
            col.collider.GetComponent<PlayerController>().TakeDamage(objectInfos.ObjectTouchDamage, col);
        }
        else   
            Bounce(col);
    }
}
