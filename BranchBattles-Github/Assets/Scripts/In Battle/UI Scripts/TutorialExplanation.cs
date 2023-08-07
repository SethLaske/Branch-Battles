using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialExplanation : MonoBehaviour
{
    //public Tutorial tutorialManager;

    public float readBuffer = .25f;
    public float timeToNextStep = 5;

    public GameObject blocker;

    public List<GameObject> elementsToDisplay;  //

    public GameObject UIRevealed;
   

    public TextMeshProUGUI explanationTextObject;
    private string explanationText;
    private float characterSpeed = .05f;

    private bool textOnScreen = false;
    // Start is called before the first frame update

    private void Awake()
    {
        DisableStep();
        explanationText = explanationTextObject.text;
        explanationTextObject.text = "";
    }

    public void DisableStep() {
        foreach (GameObject displayedElement in elementsToDisplay)
        {
            displayedElement.SetActive(false);
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

        

        Invoke("TextDelay", .25f);
    }

    public bool CheckIfTextIsDone() {
        blocker.SetActive(false);   //Always turns it off once the player clicked
        if (textOnScreen == false) {
            //characterSpeed /= 10;     Might change it from auto finishing to just speeding up by 10x
            explanationTextObject.text = explanationText;
            textOnScreen = true;
            return false;
        }

        return true;
    }

    private void TextDelay() {
        
        StartCoroutine(ShowExplanationText());
    }

    IEnumerator ShowExplanationText() {
        textOnScreen = false;

        foreach (char c in explanationText.ToCharArray()) {
            if (textOnScreen == true)   break;      
            explanationTextObject.text += c;
            yield return new WaitForSeconds(characterSpeed);
        }

        textOnScreen = true;
    }


}
