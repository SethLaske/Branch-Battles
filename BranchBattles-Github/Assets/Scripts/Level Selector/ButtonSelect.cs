using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Used to show a levels information, and then to start the fight
public class ButtonSelect : MonoBehaviour
{
    public GameObject selectedLevel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void levelSelected(GameObject levelInfo) {
        //if allowed
        selectedLevel.SetActive(false); //clears the previous info
        selectedLevel = levelInfo;
        selectedLevel.SetActive(true);  //Shows the new levels info
    }

    public void startLevel() {
        //take the name from selectedLevel
        //Start the scene
        //Debug.Log("Level " + selectedLevel.name + " has been started. Destroy them.");
        SceneManager.LoadScene(selectedLevel.name);
    }

}
