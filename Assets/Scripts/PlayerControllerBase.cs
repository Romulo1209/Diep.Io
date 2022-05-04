using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControllerBase : MonoBehaviour
{
    [Header("Scriptables")]
    public List<WeaponCreator> allWeapons;
    public WeaponCreator Weapon;
    public ExpTableCreate expTable;

    [Header("Weapon Infos")]
    public WeaponInfos weapon;

    [Header("Player Infos")]
    public PlayerInfos player;

    [Header("Objects")]
    public GameObject bullet;

    [Header("HUDS")]
    [SerializeField] TMP_Text ammoText;
    [SerializeField] TMP_Text expText;
    [SerializeField] TMP_Text healthText;
    [SerializeField] Slider healthSlider;
    [SerializeField] Slider expSlider;
    [SerializeField] GameObject GameOver;

    public void SetupPlayer() {
        player.level = 1;
        player.exp = 0;
        player.expMax = expTable.ExpNeed(player.level);
    }
    public void SetupWeapon() {
        if (weapon.weapon != null)
            Destroy(weapon.weapon.gameObject);

        weapon.weapon = Instantiate(Weapon.weaponPrefab, transform.position, Quaternion.identity, transform);
        weapon.weaponName = Weapon.name;
        weapon.weaponDamage = Weapon.weaponDamageBase;
        weapon.ammo = Weapon.weaponAmmobase;
        weapon.shootSpeed = Weapon.bulletSpeed;
        weapon.shootBackPropulsion = Weapon.shootBackPropulsion;
        weapon.shootCooldownTime = Weapon.shootCooldownTime;
        foreach(Vector3 pos in Weapon.weaponShootPoints) {
            GameObject point = Instantiate(new GameObject(), new Vector3(transform.position.x + pos.x, transform.position.y + pos.y, 0), Quaternion.identity, transform);
            point.name = "FirePoint";
            weapon.bulletSpawnPoints.Add(point.transform);
        }
    }
    public void HudInfos() {
        ammoText.text = weapon.ammo.ToString();
        //HEALTH
        healthText.text = player.life.ToString() + " LP";
        healthSlider.maxValue = player.maxLife;
        healthSlider.value = player.life;
        //EXP
        expText.text = "Points " + player.points.ToString();
    }
    public void ExpController()
    {
        if (player.exp >= player.expMax) {
            player.exp = 0;
            player.level++;
            player.expMax = expTable.ExpNeed(player.level);
        }
    }
    #region Life
    public void CheckDeath()
    {
        if (player.life <= 0) {
            GameUtils.DestroyObject(player.deathParticle, transform, gameObject);
            GameObject.Find("GameController").GetComponent<SceneController>().SetScore(player.points);
            GameOver.SetActive(true);
        }  
    }
    public void TakeDamage(float damage, Collision2D col, bool back = true)
    {
        GetComponent<Animator>().SetTrigger("Hit");
        player.life -= damage;
        if(back)
            player.rb.AddForce(-col.contacts[0].normal * player.knockbackPropulsion, ForceMode2D.Impulse);
        CheckDeath();
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.tag == "Damage")
        {
            float damage = col.gameObject.GetComponent<BulletController>().bulletDamage;
            TakeDamage(damage, col, false);
        }
    }
    #endregion
}
[System.Serializable]
public class WeaponInfos
{
    public GameObject weapon;
    public string weaponName;
    public float weaponDamage;
    public int ammo;
    public float shootSpeed;
    public float shootBackPropulsion;
    public float shootCooldownTime;
    [HideInInspector] public bool canShoot = true;
    public List<Transform> bulletSpawnPoints;
}
[System.Serializable]
public class PlayerInfos
{
    [Header("Level")]
    public int level;
    public int exp;
    public int expMax;
    public int points;
    [Header("Life")]
    public float life;
    public float maxLife;
    [Header("Misc")]
    public float propulsionGravity;
    public float knockbackPropulsion;
    public float movementVelocity;
    public float rotationSpeed = 0.4f;
    public GameObject deathParticle;

    [HideInInspector] public Rigidbody2D rb;
}
