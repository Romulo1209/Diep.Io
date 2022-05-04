using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Object", menuName = "Monomyto/ObjectCreate", order = 3)]
public class ObjectCreate : ScriptableObject
{
    [Header("Object Infos")]
    public GameObject ObjectBase;
    public string ObjectName;
    public Sprite ObjectSprite;
    public int ObjectPoints;
    public float ObjectLife;
    public float ObjectTouchDamage;
    [Header("Object Drops")]
    [Range(0,100)] public float DropChance;
    public List<GameObject> Drops;
}
