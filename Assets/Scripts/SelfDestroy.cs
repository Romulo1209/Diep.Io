using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(SelfDestroyTime());
    }
    IEnumerator SelfDestroyTime()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
