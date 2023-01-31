using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Super class for all units that can be trained, and fight/have utility
public class Unit : Damageable
{
    //Various Stats each unit has
    public float MoveSpeed; //Speed to travel across the map
    public float Damage;    //Damage done per attacl
    //public float Armor;
    public float AttackRange;   //Distance from enemy to deal damage
    public float AgroRange;     //The max distance from the rally point a troop will pursue their target
    public float AttackCooldown;    //Time between attacks
    public float AttackTimer;   
    public int Cost;        //Gold required to train the unit
    public int TroopSpaces;     //Troop spaces the unit takes up in the army
    public float SpawnTime;     //Time the unit needs to spawn

    public string State;    //Determines what actions they need to pursue

    public Damageable Target;   //Targets should only be things we want to attack. NOTHING ELSE
    public LayerMask MovementBlockers;  //things that block movement forward

    public float tolerance = .25f;  //Handles the tolerance to allow for imperfections

    //These are just rough plans for units to follow, and are edited in sub classes
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
                if (AttackTimer <= 0)
                {
                    Target.TakeDamage(Damage);
                    AttackTimer = AttackCooldown;
                } else
                {
                    AttackTimer -= Time.deltaTime;
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


    //Decreases the troop count for the general
    public override void Die() {
        
        General.TroopCount -= TroopSpaces;
        Destroy(gameObject);
    }

    //Currently unused but might be used for animations in the future
    public void Victory() {
        State = "Victory";
    }

    public void Defeat()
    {
        State = "Defeat";
    }

    public void Stun(float Duration)
    {
        StartCoroutine(StunDebuff(Duration));
    }

    //Implementing it as such will allow the effects to stack, for better or worse
    IEnumerator StunDebuff(float Duration)
    {
        MoveSpeed /= 2;
        AttackCooldown *= 1.25f;

        yield return new WaitForSeconds(Duration);

        MoveSpeed *= 2;
        AttackCooldown /= 1.25f;
    }
}
