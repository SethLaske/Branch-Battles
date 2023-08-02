using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    public int activeButtons = 0;

    private Player player;

    public BaseHealthBar playerBase;    //set in prefab
    public BaseHealthBar enemyBase;     //set in prefab

    public Button rally;
    public Button charge;

    public GoldCounter counters;

    public Button pacifist1;
    //public Button pacifist2;

    public Button soldier1;
    public Button soldier2;
    public Button soldier3;
    public Button soldier4;

    public Button magicPrep1;
    public Button magicPrep2;

    public void SetPlayer(Player player) {
        this.player = player;
    }

    public void PlayerRally() {
        player.PrepRallyPoint();
        rally.interactable = false;
        charge.interactable = true;
    }

    public void PlayerCharge() {
        player.Peasants.Charge();
        charge.interactable = false;
    }


    public void EnableTroops() {
        pacifist1.interactable = true;
        soldier1.interactable = true;
        soldier2.interactable = true;
        soldier3.interactable = true;
        soldier4.interactable = true;
    }

    public void DisableTroops() {
        pacifist1.interactable = false;
        soldier1.interactable = false;
        soldier2.interactable = false;
        soldier3.interactable = false;
        soldier4.interactable = false;
    }

    public void DelayRallyButton() { 
        StartCoroutine(ToggleButtonDelayed(rally, .25f, true));
    }

    IEnumerator ToggleButtonDelayed(Button button, float duration, bool toggle) {
        yield return new WaitForSeconds(duration);
        button.interactable = toggle;
    }



    
}
