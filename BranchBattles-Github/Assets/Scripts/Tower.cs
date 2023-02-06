using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Building
{
    public TowerSoldier defender;

    public float Damage;
    public float AttackSpeed;
    public float AttackRange;

    // Start is called before the first frame update
    void Start()
    {

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
}
