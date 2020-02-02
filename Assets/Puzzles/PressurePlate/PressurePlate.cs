using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : DoorLock {

    [SerializeField] Sprite depressed;
    [SerializeField] Sprite notDepressed;
    [SerializeField] float massRequirement = 1;

    [SerializeField] AudioClip changeAudio;
    AudioSource audioSource;
    bool previouslyUnlocked = false;

    SpriteRenderer sr;
    float currentMass;

    void playAudio(AudioClip audio) {
        audioSource.clip = audio;
        audioSource.Play();
    }

    void holdOpen() {
        if (currentMass >= massRequirement) {
            if (!previouslyUnlocked) playAudio(changeAudio);
            previouslyUnlocked = true;

            sr.sprite = depressed;
            isUnlocked = true;
        }
    }

    float findMassFromCollision(Collider2D collision) {
        GameObject curCheck = collision.gameObject;
        GameObject rootNode = curCheck.transform.root.gameObject;

        Rigidbody2D rb = curCheck.GetComponent<Rigidbody2D>();

        while (true) {
            if (rb) return rb.mass;
            if (curCheck == rootNode) break;

            curCheck = curCheck.transform.parent.gameObject;
            rb = curCheck.GetComponent<Rigidbody2D>();
        }

        return 0;
    }

    void OnTriggerEnter2D(Collider2D collision) {
        currentMass += findMassFromCollision(collision);
        
        if (currentMass >= massRequirement) {
            holdOpen();
        }
    }

    void OnTriggerExit2D(Collider2D collision) {
        currentMass -= findMassFromCollision(collision);

        if (currentMass < massRequirement) {
            sr.sprite = notDepressed;
            isUnlocked = false;

            if (previouslyUnlocked) playAudio(changeAudio);
            previouslyUnlocked = false;
        }
    }

    void Start() {
        sr = gameObject.GetComponent<SpriteRenderer>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    void Update() {
        holdOpen();
    }
}
