using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitSelectionManager : MonoBehaviour
{
    public Unit selectedUnit;
    public UnitStatsDisplay statDisplay;

    //Could change these to be the scripts instead
    public UnitSelectionActiveButton[] armyButtons;
    public Button[] reserveButtons;

    public void SelectUnit(Unit unit) {
        if (unit == null)     return;


        selectedUnit = unit;
       
        statDisplay.DisplayUnit(unit);

        foreach (UnitSelectionActiveButton button in armyButtons) {
            button.button.interactable = true;
        }
        EnableAllReserveButtons();
    }

    public void AddUnitToActiveArmy(int armyIndex) {
        if (armyIndex == 0) {
            Debug.Log("Nothing can replace miner yet");
            return;
        }

        if (selectedUnit == null) {
            return;
        }

        for (int i = 0; i < PlayerInfo.PlayerTroops.Length; i++)
        {
            if ((PlayerInfo.PlayerTroops[i] != null) && (selectedUnit == PlayerInfo.PlayerTroops[i]))
            {
                //Debug.Log("Already have this troop in the squad");
                return;
            }
          
        }

        PlayerInfo.PlayerTroops[armyIndex] = selectedUnit;

        //Update the button that needs to change names
        armyButtons[armyIndex].UpdateArmyButtonText();
    }

    private void EnableAllReserveButtons() {
        foreach (Button button in reserveButtons)
        {
            button.interactable = true;
        }
    }
}
