using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitStatsDisplay : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public Slider healthSlider;
    public Slider damageSlider;
    public Slider dpsSlider;
    public Slider costSlider;
    public Slider armorSlider;
    public Slider speedSlider;
    public Image spriteImage;
    public void DisplayUnit(Unit unit) {
        nameText.text = unit.unitName;

        healthSlider.value = unit.HP;
        damageSlider.value = unit.Damage;
        dpsSlider.value = unit.Damage/unit.attackAnimation.length;
        costSlider.value = unit.Cost;
        armorSlider.value = unit.Armor;
        speedSlider.value = unit.baseSpeed;

        if (unit.identifierSprite != null)
        {
            spriteImage.enabled = true;
            spriteImage.sprite = unit.identifierSprite;
        }
    }
}
