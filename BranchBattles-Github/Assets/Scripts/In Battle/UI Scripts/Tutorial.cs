using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Handles a lot of the player specific interface to TeamInfo. The buttons bypass the player for spawning, but the rest is done here
public class Tutorial : MonoBehaviour
{
    public GameObject backgroundButton;

    public GameObject UI;

    public TeamInfo Peasants;
    public TeamInfo Barbarians;

    public Unit miner;
    public Unit fighter;
    public Unit spear;

    public int screenToChargeOn;
    public GameObject introPrompt;
    public List<TutorialExplanation> allTutorialScreens;
    public int tutorialStepIndex = 0;

    private bool stepActive = false;    //Using to ensure the user cant double click through steps

    /*
    [Header ("Tutorial Explanation Screens")]
    public GameObject CameraScreen;
    public GameObject SoldiersUI;
    public GameObject SoldiersScreen;
    public GameObject PacifistUI;
    public GameObject PacifistScreen;
    public GameObject ArchersUI;
    public GameObject ArchersScreen;
    public GameObject RallyUI;
    public GameObject RallyScreen;
    public GameObject ChargeUI;
    public GameObject ChargeScreen;
    //public GameObject */
    
    void Start()
    {
        introPrompt.SetActive(true);
        PlayerInfo.PlayerTroops[0] = miner;
        PlayerInfo.PlayerTroops[1] = fighter;
        PlayerInfo.PlayerTroops[2] = spear;

        PlayerInfo.TroopKeys[0] = true;
        PlayerInfo.TroopKeys[1] = true;
        PlayerInfo.TroopKeys[2] = true;

        PlayerInfo.TroopSpaces = 2;


        Barbarians.gold = 1000;
        Barbarians.TrainUnit(fighter);
        Barbarians.TrainUnit(fighter);
        Barbarians.TrainUnit(fighter);
        //Barbarians.TrainUnit(fighter);
        Barbarians.TrainUnit(fighter);
        Barbarians.TrainUnit(spear);
        Barbarians.TrainUnit(spear);
        Barbarians.TrainUnit(spear);
        //Barbarians.TrainUnit(spear);
        Barbarians.gold = 0;
        Barbarians.AFKGoldAmount = 0;
        //Barbarians.SetRallyPoint(15);

        foreach (TutorialExplanation tutorialScreen in allTutorialScreens)
        {
            tutorialScreen.DisableStep();
            if (tutorialScreen.UIRevealed != null) {
                tutorialScreen.UIRevealed.SetActive(false);
            }
            
        }

        UI.SetActive(false);
    }

    private void Update()
    {
        if (LevelManager.gameState == GameState.Gameover)
        {
            //I dont really like adding this update, but I want the tutorial to immediately go away if we reach a gameover, and I don't want to add a check in Level manager for a tutorial
            gameObject.SetActive(false);
            return;
        }
    }

    public void StartTutorial() {
        UI.SetActive(true);

        backgroundButton.SetActive(true);

        allTutorialScreens[0].gameObject.SetActive(true);
        allTutorialScreens[0].EnableStep();
        stepActive = true;
        tutorialStepIndex = 0;

        LevelManager.gameState = GameState.InGame;
    }

    public void NextTutorialStep() {
        if (stepActive == false) {
            return;
        }

        stepActive = false;
        backgroundButton.SetActive(false);
        if (allTutorialScreens[tutorialStepIndex].CheckIfTextIsDone() == false) {
            StartCoroutine(DelayRoutine());
            return;
        }

        StartCoroutine(TimerRoutine());
    }



    public void EndTutorial() {
        LevelManager.gameState = GameState.InGame;
        UI.SetActive(true);
        foreach (TutorialExplanation tutorialScreen in allTutorialScreens)
        {
            if (tutorialScreen.UIRevealed != null)
            {
                tutorialScreen.UIRevealed.SetActive(true);
            }
        }

        Barbarians.gold = 50;
        Barbarians.AFKGoldAmount = 3;

        this.gameObject.SetActive(false);
    }

    IEnumerator DelayRoutine() {
        yield return new WaitForSeconds(allTutorialScreens[tutorialStepIndex].readBuffer);
        StartCoroutine(TimerRoutine());
    }
    IEnumerator TimerRoutine()
    {
        //Time.timeScale = 1;
        //yield return new WaitForSeconds(allTutorialScreens[tutorialStepIndex].readBuffer);

        allTutorialScreens[tutorialStepIndex].DisableStep();
        


        yield return new WaitForSeconds(allTutorialScreens[tutorialStepIndex].timeToNextStep);

        tutorialStepIndex++;

        if (screenToChargeOn == tutorialStepIndex)
        {
            yield return new WaitForSeconds(4);
            int enemiesToSend = 3;

            GameObject[] allUnits = GameObject.FindGameObjectsWithTag("Unit");
            foreach (GameObject unit in allUnits)
            {
                Soldier soldier = unit.GetComponent<Soldier>();
                if (soldier != null)
                {
                    if (soldier.Team == Barbarians.Team && enemiesToSend > 0)
                    {
                        soldier.ReceiveGeneralOrders();
                        enemiesToSend--;
                    }
                }
            }

            Barbarians.gold = 50;
            Barbarians.AFKGoldAmount = 3;
        }

        if (tutorialStepIndex < allTutorialScreens.Count)
        {

            allTutorialScreens[tutorialStepIndex].gameObject.SetActive(true);
            allTutorialScreens[tutorialStepIndex].EnableStep();

            
            
            backgroundButton.SetActive(true);
            stepActive = true;

        }
        else {
            EndTutorial();
        }
    }

}
