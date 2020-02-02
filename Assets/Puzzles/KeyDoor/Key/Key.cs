using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour {

    [SerializeField] public string keyValue;
    [SerializeField] float proximity = 1;

    GameObject player;

    void pickupKey(KeyInventory inv) {
        inv.addKey(this.keyValue);
        Destroy(gameObject);
    }

    void checkPlayerProximity() {
        Vector2 playerpos = player.transform.position;
        Vector2 objPos = transform.position;

        if ((playerpos - objPos).magnitude < proximity) {
            KeyInventory inv = this.player.GetComponent<KeyInventory>();
            this.pickupKey(inv);
        }
    }

    void Start() {
        this.player = GameObject.Find("Player");
    } 

    void Update() {
        this.checkPlayerProximity();
    }
}
