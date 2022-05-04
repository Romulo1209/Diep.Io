using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectsBase : MonoBehaviour
{
    public ObjectCreate objectScriptable;

    public ObjectInfos objectInfos;

    public Canvas canvas;
    public Slider lifeHud;
    public void SetupObject()
    {
        GetComponent<SpriteRenderer>().sprite = objectScriptable.ObjectSprite;
        objectInfos.ObjectName = objectScriptable.ObjectName;
        objectInfos.ObjectMaxLife = objectScriptable.ObjectLife;
        objectInfos.ObjectLife = objectInfos.ObjectMaxLife;
        objectInfos.Drops = objectScriptable.Drops;
        objectInfos.DropChance = objectScriptable.DropChance;
        objectInfos.ObjectTouchDamage = objectScriptable.ObjectTouchDamage;
        objectInfos.ObjectPoints = objectScriptable.ObjectPoints;

        objectInfos.rb.transform.Rotate(new Vector3(transform.rotation.x, transform.rotation.y, Random.Range(0, 360)));
        objectInfos.rb.AddForce(new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.5f, 0.5f)), ForceMode2D.Impulse);
        objectInfos.randomRotation = Random.Range(-0.01f, 0.01f);

        SaveLastPositions();
    }
    public void Bounce(Collision2D col)
    {
        objectInfos.rb.AddForce(col.contacts[0].normal * 0.1f, ForceMode2D.Impulse);
        SaveLastPositions();
    }
    public void Hud() {
        canvas.transform.rotation = Quaternion.Euler(0, 0, canvas.transform.rotation.z + transform.rotation.z);
    }
    public void RotationAndVelocity()
    {
        objectInfos.ActualVel(objectInfos.rb.velocity.x, objectInfos.rb.velocity.y);
        objectInfos.rb.velocity = Vector2.Lerp(objectInfos.actualVel, objectInfos.lastVel, objectInfos.ObjectGravity);

        transform.Rotate(new Vector3(0, 0, objectInfos.randomRotation));
    }
    public void CheckObjectLife()
    {
        lifeHud.value = objectInfos.ObjectLife;
        lifeHud.maxValue = objectInfos.ObjectMaxLife;
        if (objectInfos.ObjectLife <= 0)
            Death();
    }
    void Death()
    {
        float chance = Random.Range(0, 100);
        if (objectInfos.DropChance <= chance)
            foreach (GameObject drop in objectInfos.Drops)
                Instantiate(drop, transform.position, Quaternion.identity);
        GameUtils.AddScore(GameObject.Find("Player").GetComponent<PlayerController>(), objectInfos.ObjectPoints, objectInfos.Owner);
        GameUtils.DestroyObject(objectInfos.deathParticle, transform, gameObject);
    }
    public void TakeDamage(float damage, GameObject owner)
    {
        objectInfos.ObjectLife -= damage;
        GetComponent<Animator>().SetTrigger("Hit");
        objectInfos.Owner = owner;
    }

    void SaveLastPositions() {
        objectInfos.LastVel(objectInfos.rb.velocity.x, objectInfos.rb.velocity.y);
    }
}
[System.Serializable]
public class ObjectInfos
{
    [Header("Objects Infos")]
    public string ObjectName;
    public float ObjectLife;
    public float ObjectMaxLife;
    public float ObjectTouchDamage;
    public float ObjectGravity;
    public int ObjectPoints;

    [Header("Objects Infos")]
    [HideInInspector] public float DropChance;
    public List<GameObject> Drops;

    [HideInInspector] public Vector2 lastVel;
    [HideInInspector] public Vector2 actualVel;

    [HideInInspector] public float randomRotation;
    public GameObject Owner;

    public GameObject deathParticle;

    public Vector2 LastVel(float x, float y) {
        return lastVel = new Vector2(x, y);
    }
    public Vector2 ActualVel(float x, float y) {
        return actualVel = new Vector2(x, y);
    }

    [HideInInspector] public Rigidbody2D rb;
}
