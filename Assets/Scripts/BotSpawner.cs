using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotSpawner : MonoBehaviour
{
    public List<WeaponCreator> weapons;
    public GameObject botPrefab;
    public List<GameObject> allBots;
    public Transform[] points;
    public GameObject botFather;
    public int maxBots;

    private void Start()
    {
        StartCoroutine(SpawnBots());
    }
    IEnumerator SpawnBots()
    { 
        foreach(GameObject bot in allBots) {
            if (bot == null)
            {
                allBots.Clear();
                break;
            }
        }
        yield return new WaitForSeconds(5);
        if(allBots.Count < maxBots)
        {
            Vector2 randomPosition = new Vector2(Random.Range(points[0].position.x, points[1].position.x), Random.Range(points[0].position.y, points[1].position.y));
            GameObject holder = Instantiate(botPrefab, randomPosition, Quaternion.identity, botFather.transform);
            holder.GetComponent<BotController>().Weapon = weapons[Random.Range(0, weapons.Count - 1)];
            allBots.Add(holder);
        }
        StartCoroutine(SpawnBots());
    }
}
