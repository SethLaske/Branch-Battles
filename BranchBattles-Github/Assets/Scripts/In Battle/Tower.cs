using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Building
{
    public TowerSoldier defender;

    public Animator animator;

    public float Damage;
    public float AttackSpeed;
    public float AttackRange;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = HP;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Something Entered");
        if (defender == null) {
            Soldier soldier = collision.GetComponent<Soldier>();
            if (soldier != null)
            {
                //Debug.Log("Soldier Entered");
                if (soldier.Team == Team && soldier.AttackRange > 4)
                {
                    soldier.gameObject.AddComponent<TowerSoldier>();
                    defender = soldier.GetComponent<TowerSoldier>();
                    defender.Team = Team;
                    defender.Damage = Damage;
                    defender.AttackSpeed = AttackSpeed;
                    defender.AttackRange = AttackRange;

                    soldier.enabled = false;
                    defender.transform.position += new Vector3(1 * Team, 4, 0);
                    defender.gameObject.layer = 0;
                }

            }
        }
        
    }

    public override void Die()
    {
        base.Die();
        if (defender != null) {
            defender.GetComponent<Soldier>().enabled = true;
            defender.enabled = false;
            Destroy(defender.GetComponent<CircleCollider2D>());
            defender.transform.position += new Vector3(-1 * Team, -4, 0);
            defender.gameObject.layer = 6;
        }
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        float damagePercent = HP / maxHealth;
        if (damagePercent < .2) {
            animator.SetInteger("Destruction", 5);
        } else if (damagePercent < .4)
        {
            animator.SetInteger("Destruction", 4);
        }
        else if (damagePercent < .6)
        {
            animator.SetInteger("Destruction", 3);
        }
        else if (damagePercent < .8)
        {
            animator.SetInteger("Destruction", 2);
        }
    }
}
