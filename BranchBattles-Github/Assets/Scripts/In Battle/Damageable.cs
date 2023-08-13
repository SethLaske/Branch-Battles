using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Super class to any gameobject that needs to take damage
public class Damageable : MonoBehaviour
{
    public TeamInfo General;    //If a damageable has a team, it will need to know its general
    public int Team;    //Team will be used throughout, and may be used at times where a general isnt present (likely to be denoted by 0)
    public float HP;
    public float Armor = 0; //can be used either to stop attacks that do little damage or reduce damage from all attacks
    protected float maxHealth = 0f;
    private bool IsDead = false;

    public virtual void TakeDamage(float damage)
    { 
        if (damage > Armor)
        {
            HP -= damage;
        }
        else {      
            HP -= damage / (1 + Armor); //This could be used as the default damage formula, but I still like the idea of the armor being a threshold. This system really benefits units with high Damage
        }

        if (HP <= 0 && !IsDead)
        {
            IsDead = true;
            Die();
        }
    }

    //Changed by different units
    public virtual void Die()
    {
        Destroy(gameObject);
    }
}
