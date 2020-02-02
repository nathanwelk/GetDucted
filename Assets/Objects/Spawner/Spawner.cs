using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    [SerializeField] Transform spawnItem;
    [SerializeField] int spawnCap;

    [SerializeField] float spawnInterval;
    [SerializeField] DoorLock[] requiredLocks;

    List<Transform> spawnedItems = new List<Transform>();
    int currentlySpawned;
    float lastSpawn = 0;

    void pruneSpawns() {
        int len = spawnedItems.Count;

        for(int i = len - 1; i >= 0; i--) {
            if(spawnedItems[i] == null) {
                currentlySpawned--;
                spawnedItems.RemoveAt(i);

            }
        }
    }

    bool checkLocks() {
        foreach(DoorLock _lock in requiredLocks) {
            if (!_lock.isUnlocked) return false;
        }

        return true;
    }

    void checkSpawn() {
        if (checkLocks()) {
            if (currentlySpawned < spawnCap && Time.time - lastSpawn > spawnInterval) {
                lastSpawn = Time.time;

                Vector3 spawnPos = transform.position + transform.up;
                Transform obj = Instantiate(spawnItem, spawnPos, Quaternion.identity);
                obj.transform.localPosition = obj.transform.localPosition - new Vector3(0,0,0.001f);
                spawnedItems.Add(obj);
                currentlySpawned++;
            }
        }
    }

    void Start() {
        
    }

    void Update() {
        pruneSpawns();

        checkSpawn();
    }
}
