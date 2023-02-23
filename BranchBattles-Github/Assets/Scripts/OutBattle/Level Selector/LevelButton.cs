using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public int ThisLevel;
    public Image sprite;
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log(PlayerPrefs.GetInt("CompletedLevels"));
        //sprite = GetComponent<Image>();

        if (PlayerPrefs.GetInt("CompletedLevels") < (ThisLevel - 1))
        {
            gameObject.SetActive(false);
        }

        if (PlayerPrefs.GetInt("CompletedLevels") >= (ThisLevel - 1))
        {
            gameObject.SetActive(true);
            sprite.color = Color.green;
        }

        if (PlayerPrefs.GetInt("CompletedLevels") == (ThisLevel - 1))
        {
            sprite.color = Color.red;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
