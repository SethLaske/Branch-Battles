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

    public virtual void TakeDamage(float damage)
    {
        //Debug.Log("Taken Damage: " + damage);
        //Simple stuff
        if (damage > Armor) {
            this.HP -= damage;
        }

        if (HP <= 0)
        {
            Die();
        }
    }

    //Changed by different units
    public virtual void Die()
    {
        Destroy(gameObject);
    }
}
