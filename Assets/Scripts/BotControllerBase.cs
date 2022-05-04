using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotControllerBase : MonoBehaviour
{
    public enum States
    {
        Patrol,
        Farming,
        Chase,
        Running
    }

    [Header("Scriptables")]
    public WeaponCreator Weapon;

    [Header("Bot Infos")]
    public BotInfos botInfos;
    public WeaponInfos weapon;
    public GameObject bullet;

    [Header("Behaviours")]
    public States State;
    public bool SeeTarget;
    public GameObject Target;
    public float targetDistance;

    public void SetupWeapon()
    {
        if (weapon.weapon != null)
            Destroy(weapon.weapon.gameObject);

        weapon.weapon = Instantiate(Weapon.weaponPrefab, transform.position, Quaternion.identity, transform);
        weapon.weaponName = "aaa";
        weapon.weaponDamage = Weapon.weaponDamageBase;
        weapon.ammo = Weapon.weaponAmmobase;
        weapon.shootSpeed = Weapon.bulletSpeed;
        weapon.shootBackPropulsion = Weapon.shootBackPropulsion;
        weapon.shootCooldownTime = Weapon.shootCooldownTime;
        foreach (Vector3 pos in Weapon.weaponShootPoints)
        {
            GameObject point = Instantiate(new GameObject(), new Vector3(transform.position.x + pos.x, transform.position.y + pos.y, 0), Quaternion.identity, transform);
            point.name = "FirePoint";
            weapon.bulletSpawnPoints.Add(point.transform);
        }
    }

    #region IA
    public float TargetDistance()
    {
        Vector2 targetVector = new Vector2(Target.transform.position.x, Target.transform.position.y);
        Vector2 botVector = new Vector2(transform.position.x, transform.position.y);
        return targetDistance = Vector2.Distance(botVector, targetVector);
    }

    public void BehaviorTreeCheckStates()
    {
        botInfos.LowLife();
        if (SeeTarget && !botInfos.lowLife && State == States.Patrol || SeeTarget && !botInfos.lowLife && State == States.Farming) {
            State = States.Chase;
        }
        else if(SeeTarget && botInfos.lowLife && State == States.Patrol || SeeTarget && botInfos.lowLife && State == States.Farming) {
            State = States.Running;
        }
    }
    public void BehaviourStatesDefinition()
    {
        switch (State) {
            case States.Patrol:
                Patrol();
                break;
            case States.Chase:
                Chase();
                break;
            case States.Running:
                Running();
                break;
        }
    }
    public void Patrol()
    {
        if(botInfos.Pos.x == 0 && botInfos.Pos.y == 0)
            botInfos.SetPosition();
        else if(transform.position.x == botInfos.Pos.x && transform.position.y == botInfos.Pos.y)
            botInfos.SetPosition();

        transform.position = Vector2.MoveTowards(transform.position, botInfos.Pos, botInfos.botMovementSpeed * Time.deltaTime);
    }
    
    public void Chase()
    {
        TargetDistance();
        LookTarget();
        if (targetDistance < 10)
            Shoot();

        if (targetDistance > botInfos.safeDistance)
            transform.position = Vector2.MoveTowards(this.transform.position, Target.transform.position, botInfos.botMovementSpeed * Time.deltaTime);
        else if(targetDistance < botInfos.safeDistance - 2)
            transform.position = Vector2.MoveTowards(this.transform.position, Target.transform.position, -botInfos.botMovementSpeed * Time.deltaTime);

        if(botInfos.lowLife)
            State = States.Running;
    }
    public void Running()
    {
        TargetDistance();
        transform.position = Vector2.MoveTowards(this.transform.position, Target.transform.position, -botInfos.botMovementSpeed * Time.deltaTime);

        if (botInfos.lowLife)
            State = States.Running;
    }
    private void OnTriggerStay2D(Collider2D collision) {
        if(collision.tag == "Player" || collision.tag == "Bot") {
            SeeTarget = true;
            Target = collision.gameObject;
        } 
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Player" || collision.tag == "Bot") {
            SeeTarget = false;
            State = States.Patrol;
            Target = null;
        }
    }
    #endregion
    #region Aim
    public void LookTarget()
    {
        Quaternion rotation = Quaternion.LookRotation(transform.position - Target.transform.position, Vector3.forward);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation.normalized, botInfos.botRotationSpeed);
        transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z);
    }
    void Shoot()
    {
        if (weapon.canShoot) {
            GetComponent<Animator>().SetTrigger("Shoot");
            botInfos.rb.AddForce(-weapon.bulletSpawnPoints[0].up * weapon.shootBackPropulsion, ForceMode2D.Impulse);

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
    #endregion
    #region Life
    public void CheckDeath()
    {
        if(botInfos.life <= 0)
        {
            GameUtils.DestroyObject(botInfos.deathParticle, transform, gameObject);
            GameUtils.AddScore(GameObject.Find("Player").GetComponent<PlayerController>(), 50, botInfos.Owner);
        }
            
    }
    public void TakeDamage(float damage)
    {
        botInfos.life -= damage;
        GetComponent<Animator>().SetTrigger("Hit");
        CheckDeath();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.collider.tag == "Damage") {
            float damage = col.gameObject.GetComponent<BulletController>().bulletDamage;
            botInfos.Owner = col.gameObject.GetComponent<BulletController>().owner;
            TakeDamage(damage);
        }
    }
    #endregion
}

[System.Serializable]
public class BotInfos
{
    [Header("Bot Parameters")]
    public float maxLife;
    public float life;
    public float botMovementSpeed;
    public float botRotationSpeed;
    public float propulsionGravity;
    public float safeDistance;

    [Header("Conditions")]
    public bool lowLife;
    public Vector2 Pos;

    [Space]
    public GameObject deathParticle;
    public GameObject Owner;

    public bool LowLife() {
        if (life < maxLife / 8)
            return lowLife = true;
        else
            return lowLife = false;
    }
    public Vector2 SetPosition() {
        float x = Random.Range(points[0].transform.position.x, points[1].transform.position.x);
        float y = Random.Range(points[0].transform.position.y, points[1].transform.position.y);

        Vector2 pos = new Vector2(x, y);
        return Pos = pos;
    }
    [SerializeField] public GameObject[] points;
    [SerializeField] public Rigidbody2D rb;
}
