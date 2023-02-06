using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelButton : MonoBehaviour
{
    public int ThisLevel;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("CompletedLevels") <= (ThisLevel - 1)) {
            gameObject.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
