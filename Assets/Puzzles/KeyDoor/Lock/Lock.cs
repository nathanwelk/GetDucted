using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock : DoorLock {

    [SerializeField] public string keyValue;
    [SerializeField] float proximity = 2;
    [SerializeField] KeyCode interactButton = KeyCode.E;

    GameObject player;


    void Start() {
        player = GameObject.Find("Player");

    }

    void interact(KeyInventory inventory) {
        if (inventory.checkLock(this)) {
            inventory.removeKey(keyValue);
            isUnlocked = true;

        } else {
            // fail noise?
        }
    }

    void checkPlayerProximity() {
        if ((player.transform.position - transform.position).magnitude < proximity) {
            if (Input.GetKeyDown(interactButton)) {
                KeyInventory inv = player.GetComponent<KeyInventory>();

                interact(inv);
            }
        }
    }

    void Update() {
        if(!isUnlocked) {
            checkPlayerProximity();
        }
    }

}
