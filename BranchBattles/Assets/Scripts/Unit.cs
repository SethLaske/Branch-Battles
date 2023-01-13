using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : Damageable
{
    //Various Stats each unit has
    public float MoveSpeed;
    public float Damage;
    //public float Armor;
    public float AttackRange;
    public float AgroRange;
    public float AttackCooldown;
    public float Timer;
    public int Cost;
    public int TroopSpaces;
    public float SpawnTime;

    public string State;

    public Damageable Target;   //Targets should only be things we want to attack. NOTHING ELSE
    public LayerMask MovementBlockers;

    public float tolerance = .25f;

    public virtual void Wait() {
        if (General.RallyPoint * Team > (transform.position.x * Team) + tolerance)
        {
            State = "Walk";
        }
        else if (General.RallyPoint * Team < (transform.position.x * Team) - tolerance)
        {
            State = "Retreat";
        }
    }

    public virtual void Walk()
    {
        //Walk information
        transform.position += new Vector3(MoveSpeed * Time.deltaTime, 0, 0);
    }

    public virtual void Attack() {
        //Debug.Log("Mistakes have been made");
        //Attack information
        if (Target != null)
        {
            if (Vector3.Distance(transform.position, Target.transform.position) < AttackRange)
            {
                if (Timer <= 0)
                {
                    Target.TakeDamage(Damage);
                    Timer = AttackCooldown;
                } else
                {
                    Timer -= Time.deltaTime;
                }
            }
            else if (Vector3.Distance(transform.position, Target.transform.position) < AgroRange)
            {
                State = "Walk";
            }
        }
        else {
            State = "Wait";
        }
        
    }

    public virtual void Retreat()
    {
        //Retreat information
        transform.position += new Vector3(-MoveSpeed * 1.25f * Time.deltaTime, 0, 0);
        //Checks for whether to change state
        
    }

    //States to be had:
    //wait
    //walk forward
    //Attack
    //Retreat (move back)

    /*
    Rally mode really just tells the troops if they need to walk backwards/forwards then wait
    Attack mode is just walk forward until something is in triggering range, then attack it until something dies then walk forward
    If something is in triggering range, walk forward until in attack range, then attack
     */

    

    protected void FindNearestTarget() {
        /*
        float distance = Mathf.Infinity;

        GameObject[] PotentialTargets = GameObject.FindGameObjectsWithTag("Unit" or "Building");
        for (int i = 0; i < PotentialTargets.Length; i++) {
            if (Vector3.Distance(transform.position, PotentialTargets[i].transform.position) < distance) { 
                
            }
        }*/
    }

    
    public override void Die() {
        
        General.TroopCount -= TroopSpaces;
        Destroy(gameObject);
    }

    public void Victory() {
        State = "Victory";
    }

    public void Defeat()
    {
        State = "Defeat";
    }
}
