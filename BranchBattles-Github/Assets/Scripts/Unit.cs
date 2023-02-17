using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Super class for all units that can be trained, and fight/have utility
public class Unit : Damageable
{
    public Animator animator;
    public string unitName;     //called to show on buttons etc...
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

    public GameObject HealthBar;    //Health bar is currently handled here... probably shouldnt
    public float AppearanceTime = 1.5f;
    public float maxHealth = .5f;
    public float HealthTimer = 0;

    public string State;    //Determines what actions they need to pursue

    public Damageable Target;   //Targets should only be things we want to attack. NOTHING ELSE
    public LayerMask MovementBlockers;  //things that block movement forward

    public float tolerance = .25f;  //Handles the tolerance to allow for imperfections

    public GameObject corpse;

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
        Instantiate(corpse, transform.position + new Vector3(0, -.25f, 0), Quaternion.identity);
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

    public void Stun(float Duration, float Intensity)
    {
        StartCoroutine(StunDebuff(Duration, Intensity));
    }

    //Implementing it as such will allow the effects to stack, for better or worse
    IEnumerator StunDebuff(float Duration, float Intensity)
    {
        MoveSpeed /= Intensity;
        AttackCooldown *= Intensity;

        yield return new WaitForSeconds(Duration);

        MoveSpeed *= Intensity;
        AttackCooldown /= Intensity;
    }

    public override void TakeDamage(float Damage)
    {
        base.TakeDamage(Damage);
        HealthBar.transform.localScale = new Vector3(HP / maxHealth, HealthBar.transform.localScale.y, HealthBar.transform.localScale.z);
        //slider.value = HealthObject.HP;
        HealthBar.SetActive(true);
        HealthTimer = 0;
    }

    
}
