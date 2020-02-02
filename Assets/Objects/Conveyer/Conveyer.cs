using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyer : MonoBehaviour {

    [SerializeField] float force;

    Rigidbody2D findFirstRB(Collider2D collision) {
        GameObject curCheck = collision.gameObject;
        GameObject rootNode = curCheck.transform.root.gameObject;

        Rigidbody2D rb = curCheck.GetComponent<Rigidbody2D>();

        while (true) {
            if (rb) return rb;
            if (curCheck == rootNode) break;

            curCheck = curCheck.transform.parent.gameObject;
            rb = curCheck.GetComponent<Rigidbody2D>();
        }

        return null;
    }

    void OnTriggerStay2D(Collider2D other) {
        Rigidbody2D otherRb = findFirstRB(other);

        Debug.Log(otherRb);

        if (otherRb) {
            otherRb.AddForceAtPosition(transform.up * force, other.transform.position);
        }
	}

    void Start() {
        
    }

    void Update() {
        
    }
}
