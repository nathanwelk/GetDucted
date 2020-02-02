using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInventory : MonoBehaviour {

    List<string> inventory = new List<string>();

    public void addKey(string _key) {
        inventory.Add(_key);
    }

    public void removeKey(string _key) {
        int inventoryLen = this.inventory.Count;

        for(int index = 0; index < inventoryLen; index++) {
            if(inventory[index] == _key) {
                this.inventory.RemoveAt(index);
                return;
            }
        }
    }

    public bool checkLock(Lock _lock) {
        int inventoryLen = this.inventory.Count;

        for (int index = 0; index < inventoryLen; index++) {
            if(inventory[index] == _lock.keyValue) {
                return true;
            }
        }

        return false;
    }

    void Start() {
        
    }

    void Update() {
        
    }
}
