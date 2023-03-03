using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Super class for all units that can be trained, and fight/have utility
public class Unit : Damageable
{
    public Animator animator;
    public string unitName;     //called to show on buttons etc...
    public int unitClassification;

    public WeaponAttack Offense;
    public bool Attacking = false;
    //Various Stats each unit has
    public float MoveSpeed; //Speed to travel across the map
    public float Damage;    //Damage done per attack
    public float AttackRange;   //Distance from enemy to deal damage
    public float AgroRange;     //The max distance from the rally point a troop will pursue their target
    public float AttackCooldown;    //Time between attacks
    public float AttackTimer;   
    public int Cost;        //Gold required to train the unit
    public int TroopSpaces;     //Troop spaces the unit takes up in the army
    public float SpawnTime;     //Time the unit needs to spawn

    public GameObject HealthBar;    //Health bar is currently handled here... probably shouldnt
    public float AppearanceTime = 1.5f;
    
    public float HealthTimer = 0;

    public string State;    //Determines what actions they need to pursue

    public Damageable Target;   //Targets are always for the enemy
    public LayerMask MovementBlockers;  //things that block movement forward

    //public float tolerance = .25f;  //Handles the tolerance to allow for imperfections

    public GameObject corpse;


    //These are just rough plans for units to follow, and are edited in sub classes
    //Ideally edits will be the conditions for switching between states, as that is what is most likely to change

    public virtual void Wait() {
        if (Team < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public virtual void Walk()
    {
        float x = 0;
        //Walk information
        if (Target != null)
        {
            //transform.position += Advance(transform.position, Target.transform.position, Mathf.Abs(MoveSpeed) * Time.deltaTime);
            this.Move(Advance(transform.position, Target.transform.position, Mathf.Abs(MoveSpeed) * Time.deltaTime));
            x = Mathf.Sign(Target.transform.position.x - transform.position.x);
        }
        else {
            float distance = (General.RallyPoint - transform.position.x);
            //if (distance != 0) {
              //  distance = distance / Mathf.Abs(distance);
            //}
            //transform.position += new Vector3(Mathf.Sign(distance) * MoveSpeed * Time.deltaTime, 0, 0);
            this.Move(new Vector3(Mathf.Sign(distance) * MoveSpeed * Time.deltaTime, 0, 0));
            x = Mathf.Sign(distance);
        }

        if (x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

    }

    public virtual void Attack() {
        if (Attacking == false) {
            //Debug.Log("Starting Attack");
            Attacking = true;
            StartCoroutine(Attack(AttackCooldown, .05f));
        }
        
    }

    public virtual void Retreat()
    {
        //Retreat information
        //transform.position += new Vector3(-MoveSpeed * 1.25f * Time.deltaTime, 0, 0);
        this.Move(new Vector3(-MoveSpeed * 1.25f * Time.deltaTime, 0, 0));
        //Checks for whether to change state
        
    }

    IEnumerator Attack(float ChargeTime, float RecoverTime)   //Might need recover to deal with animations, otherwise easy fix to remove it
    {
        MoveSpeed /= 2; //Troops will still be able to move, but this will limit their ability to sprint or retreat once that attack has been done
        yield return new WaitForSeconds(ChargeTime);
        Offense.Attack();
        yield return new WaitForSeconds(RecoverTime);
        Attacking = false;
        MoveSpeed *= 2;
    }
    //States to be had:
    //wait
    //walk forward
    //Attack
    //Retreat (move back)


    public virtual void StandardStart() {
        maxHealth = HP;
        AttackTimer = AttackCooldown;
        State = "Walk";
        if (Team < 0)
        {
            HealthBar.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    public override void TakeDamage(float Damage)
    {
        base.TakeDamage(Damage);
        HealthBar.transform.localScale = new Vector3(HP / maxHealth, HealthBar.transform.localScale.y, HealthBar.transform.localScale.z);
        //slider.value = HealthObject.HP;
        HealthBar.SetActive(true);
        HealthTimer = 0;
    }

    public override void Die() {
        
        General.TroopCount -= TroopSpaces;
        General.troopCategory[unitClassification]--;
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

    public void Move(Vector3 movement)
    {
        Vector3 NewPosition = transform.position + movement;
        if (Physics2D.OverlapCircle(NewPosition, .2f, MovementBlockers))
        {
            transform.position = NewPosition;
        }
    }


    public Vector3 Advance(Vector3 current, Vector3 target, float maxDistanceDelta)
    {
        // Get the direction from the current position to the target position
        Vector2 direction = new Vector2(target.x, target.y) - new Vector2(current.x, current.y);

        // Get the distance between the current position and the target position
        float distance = direction.magnitude;

        // Check if we've already reached the target position
        if (distance <= maxDistanceDelta || distance == 0f)
        {
            return target;
        }

        direction = direction.normalized;
        // Calculate the new position based on the max distance delta
        //Vector3 newPosition = current + direction.normalized * maxDistanceDelta;
        Vector3 displacement = new Vector3(direction.x, direction.y, direction.y/5) * maxDistanceDelta;

        return displacement;
    }


}
