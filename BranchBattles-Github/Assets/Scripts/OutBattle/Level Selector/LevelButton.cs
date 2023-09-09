using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private string levelName;

    [SerializeField] private int thisLevel;
    [SerializeField] private List<int> requiredLevels;

    [SerializeField] private Button mapButton;
    [SerializeField] private GameObject levelDescription;
    // Start is called before the first frame update
    void Start()
    {
        Image sprite = mapButton.GetComponent<Image>();

        levelDescription.SetActive(false);

        //already finished level
        if (PlayerInfo.LevelKeys.ContainsKey(thisLevel) && PlayerInfo.LevelKeys[thisLevel] == true)
        {
            gameObject.SetActive(true);
            sprite.color = Color.green;
            return;
        }

        foreach (int level in requiredLevels) {
            //One of the required levels is not finished
            if (PlayerInfo.LevelKeys.ContainsKey(level) == false || PlayerInfo.LevelKeys[level] == false)
            {
                gameObject.SetActive(false);
                return;
            }
        }

        //The required levels were all finished, but this level has not been finished
        gameObject.SetActive(true);
        sprite.color = Color.red;
        
    }

    public void ShowLevelDescription() {
        DisableAllSiblingInfo();
        levelDescription.SetActive(true);
    }

    public void HideLevelDescription() {
        levelDescription.SetActive(false);
    }

    public void GoToLevel()
    {
        SceneManager.LoadScene(levelName);
    }

    private void DisableAllSiblingInfo() {
        Transform parentTransform = transform.parent;

        if (parentTransform != null)
        {
            foreach (Transform sibling in parentTransform)
            {
                LevelButton siblingButton = sibling.GetComponent<LevelButton>();
                if (siblingButton != null) {
                    siblingButton.HideLevelDescription();
                }
            }
        }
        else
        {
            
        }
    }

}
