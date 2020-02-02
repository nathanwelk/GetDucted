using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class Door : MonoBehaviour {
    [SerializeField] Sprite openDoor;
    [SerializeField] Sprite closedDoor;

    [SerializeField] DoorLock[] requiredLocks;
    [SerializeField] AudioClip changeSound;
    AudioSource audioSource;
    bool previouslyOpen = false;

    BoxCollider2D bc;
    SpriteRenderer sr;

    void playAudio() {
        audioSource.clip = changeSound;
        audioSource.Play();
    }

    private void close() {
        //if (previouslyOpen) playAudio();

        previouslyOpen = false;
        bc.enabled = true;
        sr.sprite = closedDoor;

    }

    private void open() {
        if (!previouslyOpen) playAudio();

        previouslyOpen = true;
        bc.enabled = false;
        sr.sprite = openDoor;
    }

    private void checkLockStatus() {
        foreach (DoorLock _lock in this.requiredLocks) {
            if (!_lock.isUnlocked) {
                close();

                return;
            }
        }

        open();
    }

    void Update() {
        checkLockStatus();
    }

    void Start() {
        audioSource = gameObject.GetComponent<AudioSource>();
        bc = gameObject.GetComponent<BoxCollider2D>();
        sr = gameObject.GetComponent<SpriteRenderer>();
    }
}