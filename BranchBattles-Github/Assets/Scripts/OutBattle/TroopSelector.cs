using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TroopSelector : MonoBehaviour
{
    public GameObject selectedTroopUI;
    public Unit selectedTroop;

    public Image troopSprite;
    public TextMeshProUGUI stats;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void showTroop(GameObject newUI) {
        selectedTroopUI.SetActive(false);

        selectedTroopUI = newUI;
        

        //selectedTroopUI.SetActive(true);
    }

    public void selectTroop(GameObject newUnitObject) {
        Unit newUnit = newUnitObject.GetComponent<Unit>();
        selectedTroop = newUnit;
        troopSprite.sprite = newUnitObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
        string text = string.Format("{0}\n\nDamage:  {1}\nCooldown:  {2}\nSpeed:  {3}\nArmor:  {4}", newUnit.unitName, newUnit.Damage, newUnit.attackAnimation.length, newUnit.MoveSpeed, newUnit.Armor);
        stats.SetText(text); 
    }
    

    public void addTroop(int selection) {
        bool found = false;
        //Debug.Log("Length of the array: " + PlayerInfo.PlayerTroops.Length);
        for (int i = 0; i < PlayerInfo.PlayerTroops.Length; i++) {
            if ((PlayerInfo.PlayerTroops[i] != null) && (selectedTroop == PlayerInfo.PlayerTroops[i])) {
                //Debug.Log("Already have this troop in the squad");
                found = true;
            }
            //Debug.Log("Troop 1 is " + PlayerInfo.PlayerTroops[i].unitName);
        }
        if (found == false) {
            if (selection == 0 && selectedTroop.unitClassification == 0)
            {
                PlayerInfo.PlayerTroops[selection] = selectedTroop;
                //Debug.Log("Adding pacifist to the squad");
            }
            else if (selection != 0 && selectedTroop.unitClassification != 0) {
                PlayerInfo.PlayerTroops[selection] = selectedTroop;
                //Debug.Log("Adding soldier to the squad");
            }
            else
            {
                //Debug.Log("Cant add that unit there");
            }
            
        }
    }
}
