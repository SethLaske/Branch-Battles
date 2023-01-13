using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public TeamInfo General;    //I think the buildings also need to know their General
    public int Team;
    public float HP;

    public void TakeDamage(float damage)
    {

        this.HP -= damage;
        //DamagePopup indicator = Instantiate(damageText, transform.position, Quaternion.identity).GetComponent<DamagePopup>();
        //indicator.SetDamageText(amount);

        if (HP <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }
}
