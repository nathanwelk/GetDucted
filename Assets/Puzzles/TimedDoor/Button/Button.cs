using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : DoorLock {

    [SerializeField] float proximity = 1.5f;
    [SerializeField] KeyCode interactButton = KeyCode.E;

    [SerializeField] float openDuration = 5;

    GameObject player;
    float unlockedTime;


    void checkCloseDoor() {
        if (Time.time - unlockedTime >= openDuration) {
            isUnlocked = false;

            unlockedTime = 0;
        }
    }

    void interact() {
        isUnlocked = true;

        if (openDuration != -1) unlockedTime = Time.time;
    }

    void checkInput() {
        if (Input.GetKeyDown(interactButton)) {
            interact();
        }
    }

    void checkPlayerProximity() {
        if ((player.transform.position - transform.position).magnitude < proximity) {
            checkInput();
        }
    }

    void Start() {
        player = GameObject.FindWithTag("Player");
    }

    void Update() {
        checkPlayerProximity();
        if(unlockedTime > 0) {
            checkCloseDoor();
        }
    }
}
