using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public GameObject owner;
    public Transform FirePoint;
    public float bulletDamage;
    public float bulletSpeed;
    public float bulletGravity;
    public GameObject destroyParticle;

    Rigidbody2D rb;
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(FirePoint.up * bulletSpeed, ForceMode2D.Impulse);
        StartCoroutine(SelfDestroy());
    }
    private void Update()
    {
        Vector2 velocity = Vector2.Lerp(new Vector2(rb.velocity.x, rb.velocity.y), new Vector2(0, 0), bulletGravity * Time.deltaTime);
        rb.velocity = new Vector2(velocity.x, velocity.y);
    }
    IEnumerator SelfDestroy()
    {
        yield return new WaitForSeconds(3);
        Instantiate(destroyParticle, transform);
        Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        GameUtils.DestroyObject(destroyParticle, transform, gameObject);
    }
}
