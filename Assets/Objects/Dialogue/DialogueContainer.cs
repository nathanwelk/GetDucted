using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueContainer : MonoBehaviour {

    [System.Serializable]
    public class Dialogue {
        public string dialogueText;
        public float dialogueDuration;

        public Dialogue(string _DT, float _DD) {
            dialogueText = _DT;
            dialogueDuration = _DD;
        }
    }

    [SerializeField] public bool isProximityTrigger = false;
    [SerializeField] public Dialogue[] dialogues;
    [SerializeField] private KeyCode interactButton = KeyCode.E;
    [SerializeField] private float proximity = 1;

    float lastClick = 0;
    float dialogueDuration = 0;

    GameObject player;

    void queueDialogue(DialogueRender dr) {
        lastClick = Time.time;

        foreach (Dialogue dialogue in dialogues) {
            dr.dialogues.Enqueue(dialogue);
        }
    }

    void checkPlayerProximity() {
        Vector2 playerpos = player.transform.position;
        Vector2 objPos = transform.position;

        if ((playerpos - objPos).magnitude < proximity) {
            if (Input.GetKeyDown(interactButton) || isProximityTrigger) {

                queueDialogue(player.GetComponent<DialogueRender>());
            }
        }
    }

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");

        foreach (Dialogue dialogue in dialogues) {
            dialogueDuration += dialogue.dialogueDuration;
        }
    }

    void Update() {
        if (lastClick == 0 || Time.time - lastClick > dialogueDuration) {
            checkPlayerProximity();
        }
    }
}
