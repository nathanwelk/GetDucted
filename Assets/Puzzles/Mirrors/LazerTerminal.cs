using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LazerTerminal : DoorLock {

    [SerializeField] public string miniGameScene;
    [SerializeField] float proximity = 1;
    [SerializeField] float closeDelay = 1;

    LazerGame lg;
    GameObject player;
    float flagCloseTime;

    public bool isPlaying = false;
    public bool isComplete = false;

    void closeScene() {
        isPlaying = false;

        SceneManager.UnloadSceneAsync(miniGameScene);
        player.GetComponent<PlayerController>().toggleInteractions(false);
        player.GetComponent<TapeGun>().toggleInteractions(true);
    }

    void openScene() {
        isPlaying = true;

        SceneManager.LoadScene(miniGameScene, LoadSceneMode.Additive);
        player.GetComponent<PlayerController>().toggleInteractions(true);
        player.GetComponent<TapeGun>().toggleInteractions(false);
    }

    void checkInteraction() {
        
        if (isPlaying) {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                closeScene();
            }
        } else {
            if ((player.transform.position - transform.position).magnitude < proximity && Input.GetKeyDown(KeyCode.E)) {
                openScene();
            }
        }
    }

    void checkClose() {
        if (Time.time - flagCloseTime > closeDelay) {
            closeScene();
            flagCloseTime = 0;
        } 
    }

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update() {
        if(flagCloseTime > 0) {
            checkClose();
        }

        if (!isComplete) {
            checkInteraction();
        }

        if(isPlaying && !lg) {
            GameObject lazerGameObj = GameObject.Find("LazerGame");
            if(lazerGameObj) {
                lg = lazerGameObj.GetComponent<LazerGame>();
            }
        } else {

            if (isPlaying && !isComplete) {
                if (lg.isComplete) {
                    isComplete = true;
                    flagCloseTime = Time.time;

                    isUnlocked = true;
                }
            }

        }
    }
}
