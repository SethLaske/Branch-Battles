using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Handles a lot of the player specific interface to TeamInfo. The buttons bypass the player for spawning, but the rest is done here
public class Tutorial : MonoBehaviour
{
    public GameObject UI;

    public TeamInfo Peasants;
    public TeamInfo Barbarians;

    public Unit miner;
    public Unit fighter;
    public Unit spear;

    public List<TutorialExplanation> allTutorialScreens;
    public int tutorialStepIndex = 0;

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
        PlayerInfo.PlayerTroops[0] = miner;
        PlayerInfo.PlayerTroops[1] = fighter;
        PlayerInfo.PlayerTroops[2] = spear;

        PlayerInfo.TroopKeys[0] = true;
        PlayerInfo.TroopKeys[1] = true;
        PlayerInfo.TroopKeys[2] = true;

        PlayerInfo.TroopSpaces = 2;

        

        Barbarians.SpawnUnit(fighter);
        Barbarians.SpawnUnit(fighter);
        Barbarians.SpawnUnit(fighter);
        Barbarians.SpawnUnit(fighter);
        Barbarians.SpawnUnit(fighter);
        Barbarians.SpawnUnit(spear);
        Barbarians.SpawnUnit(spear);
        Barbarians.SpawnUnit(spear);
        Barbarians.SpawnUnit(spear);

        foreach (TutorialExplanation tutorialScreen in allTutorialScreens)
        {
            tutorialScreen.DisableStep();
            if (tutorialScreen.UIRevealed != null) {
                tutorialScreen.UIRevealed.SetActive(false);
            }
            
        }
    }

    public void StartTutorial() {
        UI.SetActive(true);

        allTutorialScreens[0].EnableStep();
        tutorialStepIndex = 0;
    }

    public void NextTutorialStep() {
        allTutorialScreens[tutorialStepIndex].DisableStep();
        tutorialStepIndex++;

        if (tutorialStepIndex == allTutorialScreens.Count) {
            EndTutorial();
            return;
        }

        allTutorialScreens[tutorialStepIndex].EnableStep();
    }

    public void EndTutorial() {
        UI.SetActive(true);
        foreach (TutorialExplanation tutorialScreen in allTutorialScreens)
        {
            if (tutorialScreen.UIRevealed != null)
            {
                tutorialScreen.UIRevealed.SetActive(true);
            }
        }

        this.gameObject.SetActive(false);
    }

    
}
