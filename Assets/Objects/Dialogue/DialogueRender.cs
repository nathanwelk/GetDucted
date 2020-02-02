using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueRender : MonoBehaviour {


    [SerializeField] TMP_Text dialogueUI;
    [SerializeField] Image img;

    public Queue<DialogueContainer.Dialogue> dialogues = new Queue<DialogueContainer.Dialogue>();

    float lastPlayed = 0;
    float playDuration = 0;
    DialogueContainer.Dialogue nextDialogue;

    void queueNext() {
        if (dialogues.Count > 0) {
            nextDialogue = dialogues.Dequeue();
        } else if (Time.time - lastPlayed > playDuration) {
            dialogueUI.text = "";
            nextDialogue = null;
            lastPlayed = 0;
            playDuration = 0;

        } else {
            nextDialogue = null;
        }
    }

    void updateDialogue() {
        if(nextDialogue != null) {
            if (Time.time - lastPlayed > playDuration) {
                dialogueUI.text = nextDialogue.dialogueText;
                img.enabled = true;

                playDuration = nextDialogue.dialogueDuration;
                lastPlayed = Time.time;

                queueNext();
            }
        } else {
            queueNext();
        }

    }

    void toggleBackground() {
        if(playDuration == 0) img.enabled = false;
    }

    void Start()
    {
        
    }


    void Update() {
        updateDialogue();

        toggleBackground();
    }
}
