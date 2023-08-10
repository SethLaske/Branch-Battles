using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitButtons : MonoBehaviour
{
    public Button thisButton; 
    public TextMeshProUGUI UnitName;
    public TextMeshProUGUI UnitCost;
    // Start is called before the first frame update
   

    public void SetUnitType(Unit unit) {
        UnitName.text = unit.unitName;
        UnitCost.text = unit.Cost.ToString();
    }

    public void ShowCost() {
        if (thisButton.interactable == true) {
            UnitName.gameObject.SetActive(false);
            UnitCost.gameObject.SetActive(true);
        }
    }

    public void ShowName() {
        UnitName.gameObject.SetActive(true);
        UnitCost.gameObject.SetActive(false);
    }


}
