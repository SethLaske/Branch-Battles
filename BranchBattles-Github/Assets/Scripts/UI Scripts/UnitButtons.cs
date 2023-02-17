using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UnitButtons : MonoBehaviour
{
    public TextMeshProUGUI UnitName;
    public TextMeshProUGUI UnitCost;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setUnitType(Unit unit) {
        UnitName.text = unit.unitName;
        UnitCost.text = unit.Cost.ToString();
    }
}
