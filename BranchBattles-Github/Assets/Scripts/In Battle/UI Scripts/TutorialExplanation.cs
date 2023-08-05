using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialExplanation : MonoBehaviour
{
    public Tutorial tutorialManager;

    public float ReadBuffer = 2;
    public float timeToNextStep = 5;

    public GameObject explanation;
    public GameObject Arrow;
    public GameObject UIRevealed;

    // Start is called before the first frame update

    public void DisableStep() {
        explanation.SetActive(false);
    }

    public void EnableStep()
    {
        Time.timeScale = 0;
        explanation?.SetActive(true);
        UIRevealed?.SetActive(true);
        Arrow?.SetActive(true);
    }

    public void ReadStep() {
        StartCoroutine(TimerRoutine());
    }

    IEnumerator TimerRoutine()
    {
        yield return new WaitForSeconds(ReadBuffer);
        explanation?.SetActive(false);
        Time.timeScale = 1;


        yield return new WaitForSeconds(timeToNextStep);
        
        Arrow?.SetActive(false);

        tutorialManager.NextTutorialStep();
    }


}
