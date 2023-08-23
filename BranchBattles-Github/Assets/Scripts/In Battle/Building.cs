using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Currently no different from Damageable, but I want it to be easier to make changes in the future, especially with animations/sound effects
public class Building : Damageable
{
    protected Slider healthBar;

    public void SetHealthBar(Slider slider)
    {
        healthBar = slider;
        healthBar.maxValue = HP;
        healthBar.value = HP;
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        if (healthBar != null)
        {
            healthBar.value = HP;
        }

    }
}
