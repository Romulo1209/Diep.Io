using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PlayerControllerBase
{
    Vector3 mousePos;
    void Start() {
        player.rb = GetComponent<Rigidbody2D>();
        Weapon = allWeapons[Random.Range(0, allWeapons.Count)];
        SetupWeapon();
        SetupPlayer();
    }
    void FixedUpdate() {
        HudInfos();
        ExpController();
        Moviment();
    }
    void Moviment()
    {
        float movX = Input.GetAxis("Horizontal");
        float movY = Input.GetAxis("Vertical");

        transform.position = Vector2.Lerp(new Vector2(transform.position.x, transform.position.y), new Vector2(transform.position.x + movX, transform.position.y + movY), player.movementVelocity * Time.deltaTime);
        Vector2 velocity = Vector2.Lerp(new Vector2(player.rb.velocity.x, player.rb.velocity.y), new Vector2(0,0), player.propulsionGravity * Time.deltaTime);

        player.rb.velocity = new Vector2(velocity.x, velocity.y);
        Look();
    }
    void Look() {
        mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        Quaternion rotation = Quaternion.LookRotation(transform.position - mousePos, Vector3.forward);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation.normalized, player.rotationSpeed);
        transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);

        Shoot();
    }
    void Shoot() {
        float shoot = Input.GetAxisRaw("Fire1");
        if(shoot != 0 && weapon.canShoot && weapon.ammo > 0) {
            weapon.ammo -= 1;
            GetComponent<Animator>().SetTrigger("Shoot");
            player.rb.AddForce(-weapon.bulletSpawnPoints[0].up * weapon.shootBackPropulsion, ForceMode2D.Impulse);
           
            StartCoroutine(InstantiateShoot());
            StartCoroutine(ShootCooldown());
        }
    }
    IEnumerator InstantiateShoot()
    {
        List<GameObject> lastBullets = new List<GameObject>();
        foreach (Transform pos in weapon.bulletSpawnPoints)
        {
            lastBullets.Add(Instantiate(bullet, pos.position, transform.rotation));
            lastBullets[lastBullets.Count - 1].GetComponent<BulletController>().FirePoint = pos;
            lastBullets[lastBullets.Count - 1].GetComponent<BulletController>().owner = gameObject;
            lastBullets[lastBullets.Count - 1].GetComponent<BulletController>().bulletSpeed = weapon.shootSpeed;
            lastBullets[lastBullets.Count - 1].GetComponent<BulletController>().bulletDamage = weapon.weaponDamage;
            yield return new WaitForSeconds(0.05f);
        }
    }
    IEnumerator ShootCooldown()
    {
        weapon.canShoot = false;
        yield return new WaitForSeconds(weapon.shootCooldownTime);
        weapon.canShoot = true;
    }
}
