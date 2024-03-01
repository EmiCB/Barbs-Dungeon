using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to make pools of commonly reused objects. Reduces the memory usage and
/// risk of memory leaks.
/// </summary>
public class ObjectPooler : MonoBehaviour {
    public GameObject pooledObject;

    public int pooledAmount;

    List<GameObject> pooledObjects;

    void Start() {
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < pooledAmount; i++) {
            GameObject obj = (GameObject)Instantiate(pooledObject, transform);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }
    
    public GameObject GetPooledObject() {
        for (int i = 0; i < pooledObjects.Count; i++) {
            if (!pooledObjects[i].activeInHierarchy) {
                return pooledObjects[i];
            }
        }
        GameObject obj = (GameObject)Instantiate(pooledObject, transform);
        obj.SetActive(false);
        pooledObjects.Add(obj);
        return obj;
    }
}