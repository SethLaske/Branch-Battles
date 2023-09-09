using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TutorialExplanation : MonoBehaviour
{
    //public Tutorial tutorialManager;
    private Tutorial tutorial;

    public float readBuffer = .25f;
    public float timeToNextStep = 5;

    public GameObject blocker;

    public List<GameObject> elementsToDisplay;  //

    public Button forcedButtonToClick;
    private Transform forcedButtonToClickParent;

    public GameObject UIRevealed;
   

    public TextMeshProUGUI explanationTextObject;
    private string explanationText;
    private float characterSpeed = .05f;

    private bool textOnScreen = false;
    // Start is called before the first frame update

    private void Awake()
    {
        explanationText = explanationTextObject.text;
        explanationTextObject.text = "";

        tutorial = GetComponentInParent<Tutorial>();

        DisableStep();
    }

    

    public void DisableStep() {
        foreach (GameObject displayedElement in elementsToDisplay)
        {
            displayedElement.SetActive(false);
        }

        if (forcedButtonToClick != null && forcedButtonToClickParent != null)
        {
            forcedButtonToClick.transform.SetParent(forcedButtonToClickParent);
            forcedButtonToClick.onClick.RemoveListener(tutorial.NextTutorialStep);
        }

        this.gameObject.SetActive(false);
    }

    public void EnableStep()
    {
        this.gameObject.SetActive(true);
        //Time.timeScale = 0;
        foreach (GameObject displayedElement in elementsToDisplay) {
            displayedElement.SetActive(true);
        }
        
        if(UIRevealed != null) UIRevealed.SetActive(true);

        if (forcedButtonToClick != null) {
            forcedButtonToClickParent = forcedButtonToClick.transform.parent;
            forcedButtonToClick.transform.SetParent(transform);
            forcedButtonToClick.onClick.AddListener(tutorial.NextTutorialStep);
        }

        StartCoroutine(ShowExplanationText());
        //Invoke("TextDelay", .25f);
    }

    public bool CheckIfTextIsDone() {
        blocker.SetActive(false);   //Always turns it off once the player clicked
        if (textOnScreen == false) {
            explanationTextObject.text = explanationText;
            textOnScreen = true;
            return false;
        }

        return true;
    }


    IEnumerator ShowExplanationText() {
        yield return new WaitForSeconds(.25f);
        textOnScreen = false;

        foreach (char c in explanationText.ToCharArray()) {
            if (textOnScreen == true)   break;  //stop the coroutine if switched early  
            
            explanationTextObject.text += c;
            yield return new WaitForSeconds(characterSpeed);
        }

        textOnScreen = true;
    }


}
