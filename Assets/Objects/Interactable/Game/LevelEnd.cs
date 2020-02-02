using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    [SerializeField] private string nextLevel = "";
    [SerializeField] DoorLock[] requiredLocks;

    DialogueContainer dc;
    float messageSentTime = 0;
    float delay = 1;

    bool checkLocks() {
        foreach(DoorLock _lock in requiredLocks) {
            if (!_lock.isUnlocked) return false;
        }

        return true;
    }
    
    private void OnTriggerEnter2D(Collider2D collision) {
        if (!collision.gameObject.CompareTag("Player"))
            return;
        if (nextLevel == "")
            Debug.LogError("No level input was added");

        if (checkLocks()) {
            SceneManager.LoadScene(nextLevel);
        }
    }


    private void Update() {
        if(messageSentTime == 0 && checkLocks()) {
            dc.isProximityTrigger = true;
            messageSentTime = Time.time;
        } else if(messageSentTime > 0) {
            if(Time.time - messageSentTime > delay) {
                dc.isProximityTrigger = false;
            }
        }
    }

    private void Start() {
        dc = gameObject.GetComponent<DialogueContainer>();
    }
}
