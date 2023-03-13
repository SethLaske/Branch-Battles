using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrentTroops : MonoBehaviour
{
    public int thisTroop;
    public TextMeshProUGUI CurrentTroop;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerInfo.TroopSpaces < thisTroop)
        {
            gameObject.SetActive(false);
        }
        else {
            setUnit();
        }

        /*
        if (PlayerInfo.PlayerTroops[thisTroop] == null)
        {
            gameObject.SetActive(false);
        }
        else {
            setUnit();
        }*/
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setUnit() {
        if (PlayerInfo.PlayerTroops[thisTroop] != null)
        {
            //Debug.Log("previous unit " + thisTroop + " was " + PlayerInfo.PlayerTroops[thisTroop].unitName);
            CurrentTroop.text = PlayerInfo.PlayerTroops[thisTroop].unitName;
            //Debug.Log("Setting unit " + thisTroop + " as " + PlayerInfo.PlayerTroops[thisTroop].unitName);
        }
        else {
            CurrentTroop.text = "NO UNIT ASSIGNED";
        }
        
    }
}
