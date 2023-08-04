using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectionManager : MonoBehaviour
{
    public Unit selectedUnit;

    public void SelectUnit(Unit unit) {
        selectedUnit = unit;

        //Do wheel stuff
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
    }
}
