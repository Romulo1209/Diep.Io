using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedController : MonoBehaviour
{
    public List<ObjectCreate> Objects;
    public List<GameObject> ObjectsSpawnedList;
    [Range(1, 500)] public int MaxObjectsSpawned = 1;

    public Transform[] points;
    public Transform objectsFather;

    bool spawning;
    void Update()
    {
        CheckAllObjects();
    }
    void CheckAllObjects()
    {
        int i = 0;
        foreach (GameObject Object in ObjectsSpawnedList) {
            if (Object == null) {
                ObjectsSpawnedList.RemoveAt(i);
                break;
            }
            i++;
        }

        if (ObjectsSpawnedList.Count - 1 < MaxObjectsSpawned && !spawning) {
            StartCoroutine(SpawnObject());
        }
    }
    IEnumerator SpawnObject()
    {
        spawning = true;
        yield return new WaitForSeconds(Random.Range(0.5f, 2f));
        SpawnObject(Objects[Random.Range(0, Objects.Count)]);
        spawning = false;
    }
    void SpawnObject(ObjectCreate Object)
    {
        GameObject newObject = Instantiate(Object.ObjectBase, RandomLocation(), Quaternion.identity, objectsFather);
        newObject.GetComponent<Objects>().objectScriptable = Object;
        ObjectsSpawnedList.Add(newObject);
    }

    Vector2 RandomLocation()
    {
        float valueX = Random.Range(points[0].position.x, points[1].position.x);
        float valueY = Random.Range(points[0].position.y, points[1].position.y);
        Vector2 location = new Vector2(valueX, valueY);
        return location;
    }
}
