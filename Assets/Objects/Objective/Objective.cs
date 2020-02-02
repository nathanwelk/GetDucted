using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective : MonoBehaviour
{
    [SerializeField] float interactDuration = 5;
    [SerializeField] KeyCode interactKey = KeyCode.E;
    [SerializeField] float proximity = 2;
    [SerializeField] Sprite repairedSprite;

    [SerializeField] AudioClip completionSound;
    AudioSource audioSource;

    SpriteRenderer sr;
    DialogueContainer dc;

    GameObject player;
    ObjectiveManager objectiveManager;

    public bool isComplete = false;
    float interactStartTime;


    void interact() {
        if(Time.time - interactStartTime > interactDuration) {
            isComplete = true;

            audioSource.clip = completionSound;
            audioSource.Play();

            sr.sprite = repairedSprite;
            dc.dialogues[0].dialogueText = "Already Repaired";
            objectiveManager.setProgressBarValue(0);
            interactStartTime = 0;
        }
    }

    void checkPlayerProximity() {
        if ((player.transform.position - transform.position).magnitude < proximity && Input.GetKey(interactKey)) {
            if (interactStartTime == 0) interactStartTime = Time.time;

            objectiveManager.setProgressBarValue((int)Mathf.Round((Time.time - interactStartTime) / interactDuration * 100));
            interact();

        } else {
            if(interactStartTime != 0)
                objectiveManager.setProgressBarValue(0);

            interactStartTime = 0;
        }
    }

    void Start() {
        player = GameObject.Find("Player");
        objectiveManager = player.GetComponent<ObjectiveManager>();
        audioSource = gameObject.GetComponent<AudioSource>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        dc = gameObject.GetComponent<DialogueContainer>();
    }

    void Update() {
        if(!isComplete) {
            checkPlayerProximity();
        }
    }
}
