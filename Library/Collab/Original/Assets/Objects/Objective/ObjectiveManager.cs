using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectiveManager : DoorLock {

    [SerializeField] TMP_Text objectiveUI;
    [SerializeField] ProgressBar pb;

    List<Objective> objectives = new List<Objective>();

    public void hideProgressBar() {
        //pb.enabled = false;
    }

    public void setProgressBarValue(int barValue) {
        pb.BarValue = barValue;
        //if (barValue > 0) pb.enabled = true;
    }


    void updateUIElements() {
        int totalObjectives = objectives.Count;

        int completedObjectives = 0;
        foreach(Objective obj in objectives) {
            if (obj.isComplete) completedObjectives++;
        }

        if (completedObjectives == totalObjectives) isUnlocked = true;

        objectiveUI.text = "Repairs: " + completedObjectives + "/" + totalObjectives;
    }

    void Start() {

        GameObject[] objectiveGameObjects = GameObject.FindGameObjectsWithTag("Objective");

        foreach (GameObject _obj in objectiveGameObjects) {
            objectives.Add(_obj.GetComponent<Objective>());
        }
        
    }

    void Update() {
        updateUIElements();
    }
}
