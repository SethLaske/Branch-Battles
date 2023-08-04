using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UnitSelectionActiveButton : MonoBehaviour
{
    public int armyIndex;
    public TextMeshProUGUI currentTroopText;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerInfo.TroopSpaces < armyIndex)
        {
            gameObject.SetActive(false);
        }
        else
        {
            UpdateArmyButtonText();
        }
    }

    public void UpdateArmyButtonText()
    {
        if (PlayerInfo.PlayerTroops[armyIndex] != null)
        {
            //Debug.Log("previous unit " + thisTroop + " was " + PlayerInfo.PlayerTroops[thisTroop].unitName);
            currentTroopText.text = PlayerInfo.PlayerTroops[armyIndex].unitName;
            //Debug.Log("Setting unit " + thisTroop + " as " + PlayerInfo.PlayerTroops[thisTroop].unitName);
        }
        else
        {
            currentTroopText.text = "NO UNIT ASSIGNED";
        }

    }
}
