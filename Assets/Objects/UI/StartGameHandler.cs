using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameHandler : MonoBehaviour {

    [SerializeField] string firstLevel = "Level01";

    public void loadFirstLevel() {
        Debug.Log("test");
        SceneManager.LoadScene(firstLevel);
    }

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
