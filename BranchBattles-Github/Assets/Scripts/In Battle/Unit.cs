using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Super class for all units that can be trained, and fight/have utility  
public class Unit : Damageable
{
    //public float minfollow = 1;
    //public float maxfollow = 2;
    [Header("Unique Identifiers")]
    public Animator animator;
    public string unitName;     //called to show on buttons etc...
    public int UnitNumber;  //Used to code the unit for save data. These should each be unique, and will need to be updated in the UnitCoder.
    public int unitClassification;
    public WeaponAttack Offense;
    protected bool Attacking = false;
    //Various Stats each unit has
    [Header("Stats")]
    public float MoveSpeed; //Speed to travel across the map
    protected float currentspeed;
    public float Damage;    //Damage done per attack
    public float AttackRange;   //Distance from enemy to deal damage
    public float AgroRange;     //The max distance from the rally point a troop will pursue their target
    public float AttackCooldown;    //Time between attacks
    protected float AttackTimer;   
    public int Cost;        //Gold required to train the unit
    public int TroopSpaces;     //Troop spaces the unit takes up in the army
    public float SpawnTime;     //Time the unit needs to spawn

    [Header("Misc")]
    public GameObject HealthBar;    //Health bar is currently handled here... probably shouldnt
    public float AppearanceTime = 1.5f;

    protected float HealthTimer = 0;

    public string State;    //Determines what actions they need to pursue

    public Damageable Target;   //Targets are always for the enemy      //Could go private 
    public float AssemblePoint;         //Could go private 
    public float RearPoint;             //Could go private 
    public LayerMask MovementBlockers;  //things that block movement forward

    //public float tolerance = .25f;  //Handles the tolerance to allow for imperfections

    public GameObject corpse;
    protected General general;

    public AudioSource attackSound;

    private float DebuffMult = 1;   //Stores speed multipliers so they can be undone when passed back to the general  

    public Soldier humanshield;

    /// <summary>
    /// The Unit is roughly in position, and will face towards the enemy base
    /// </summary>
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

    /// <summary>
    /// The Unit needs to walk. Will either approach a target to attack, or move in the direction of the teams rally point. 
    /// </summary>
    public virtual void Walk()
    {
        float x = 0;

        if (humanshield != null && transform.position.x * Team < (AssemblePoint + RearPoint) / 2 * Team) { // Will try and stay behind the shield, but only when it needs to walk forward to its destination
            if (.5f > (humanshield.transform.position.x - transform.position.x) * Team) 
            {
                Debug.Log("Waiting for shield");
                return;
            }
            else if (1.5f > (humanshield.transform.position.x - transform.position.x) * Team)
            {
                Debug.Log("Walking with shield");
                currentspeed = humanshield.currentspeed;
                float distance = ((AssemblePoint + RearPoint) / 2 - transform.position.x);
                this.Move(new Vector3(Mathf.Sign(distance) * humanshield.currentspeed * Time.deltaTime, 0, 0));
                return;
            }
        }

        currentspeed = MoveSpeed / DebuffMult;

        if (Target != null && IsTargetAggroable() == true)
        {
            Debug.Log("Walking at enemy");
            this.Move(Advance(transform.position, Target.transform.position, Mathf.Abs(currentspeed) * Time.deltaTime));
            x = Mathf.Sign(Target.transform.position.x - transform.position.x);
        }
        else {
            Debug.Log("Walking at rally");
            float distance = ((AssemblePoint + RearPoint)/2 - transform.position.x);
            this.Move(new Vector3(Mathf.Sign(distance) * currentspeed * Time.deltaTime, 0, 0));
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

    /// <summary>
    /// The Unit is within range of attacking a target and will continue to do so while the target is alive and in agro range
    /// </summary>
    public virtual void Attack() {

        //Moves in the Y to ensure the target stays within the hit area
        if (Target != null && Mathf.Abs(Target.transform.position.y - transform.position.y) > .1) {
            float YMove = (Mathf.Sign(Target.transform.position.y - transform.position.y) * currentspeed * Time.deltaTime);
            transform.position += new Vector3(0, YMove, YMove / 5); 
        }

        if (Attacking == false) {
            Attacking = true;
            attackSound.Play();
            StartCoroutine(Attack(AttackCooldown, .05f));
        }

        //Redundant
        if (Target != null) {
            if (Target.transform.position.x - transform.position.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
        
    }

   

    /// <summary>
    /// Checking if the unit is within an acceptable range from the teams rally point, using RearPoint and AssemblePoint
    /// </summary>
    protected bool IsWithinAssemble() { //Solely checks if it is within the rear and assemble
        if (RearPoint * Team > transform.position.x * Team) {
            return false;
        }
        if (AssemblePoint * Team < transform.position.x * Team) {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Returns true if the unit has a target, and the distance to that target is less than its attack range
    /// </summary>
    protected bool IsTargetAttackable() {
        if (Target == null) {
            return false;
        }

        if (Vector3.Distance(transform.position, Target.transform.position) > AttackRange)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Returns true if the unit has a target, and the distance to that target from the units assemble point is less than its agro range
    /// </summary>
    protected bool IsTargetAggroable()
    {
        if (Target == null)
        {
            return false;
        }

        if ((Target.transform.position.x - AssemblePoint) * Team > AgroRange)
        {
            return false;
        }

        Debug.Log("Target is agroable");
        return true;
    }

    //Calls the attack and provides timings here. One flaw is that they will attack even if the target already died while they are charging
    IEnumerator Attack(float ChargeTime, float RecoverTime)   //Might need recover to deal with animations, otherwise easy fix to remove it
    {
        currentspeed /= 2; //Troops will still be able to move, but this will limit their ability to sprint or retreat once that attack has been done
        DebuffMult *= 2;
        yield return new WaitForSeconds(ChargeTime);
        Offense.Attack();
        yield return new WaitForSeconds(RecoverTime);
        Attacking = false;
        currentspeed *= 2;
        DebuffMult /= 2;
    }
    

    /// <summary>
    /// Initializes the typical start script for the different unit classes, including setting max health, speeds, attack timer, state, sprite color, and assigning its general
    /// </summary>
    public virtual void StandardStart() {
        maxHealth = HP;
        currentspeed = MoveSpeed;
        AttackTimer = AttackCooldown;
        State = "Walk";
        if (Team < 0)
        {
            HealthBar.GetComponent<SpriteRenderer>().color = Color.red;
        }
        general = General.general;
    }

    //Override take damage to allow for the health bar to be displayed
    public override void TakeDamage(float Damage)
    {
        base.TakeDamage(Damage);
        HealthBar.transform.localScale = new Vector3(HP / maxHealth, HealthBar.transform.localScale.y, HealthBar.transform.localScale.z);
        //slider.value = HealthObject.HP;
        HealthBar.SetActive(true);
        HealthTimer = 0;
    }

    //A lot of things get taken care of for the general and team info
    public override void Die() {
        
        General.TroopCount -= TroopSpaces;
        General.troopCategory[unitClassification]--;
        General.TotalSpeed -= (MoveSpeed);
        General.ActiveCount--;
        General.Souls++;
        General.UpdateGeneral();
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

    /// <summary>
    /// Slows down the units move speed and attack speed by a factor of the intensity for the length of the duration
    /// </summary>
    public void Stun(float Duration, float Intensity)
    {
        StartCoroutine(StunDebuff(Duration, Intensity));
    }

    //Implementing it as such will allow the effects to stack, for better or worse
    IEnumerator StunDebuff(float Duration, float Intensity)
    {
        currentspeed /= Intensity;
        DebuffMult *= Intensity;
        AttackCooldown *= Intensity;

        yield return new WaitForSeconds(Duration);

        currentspeed *= Intensity;
        DebuffMult /= Intensity;
        AttackCooldown /= Intensity;
    }

    //Essentially the same as transform += vector3, but checks to make sure it can step there.
    public bool Move(Vector2 movement)
    {
        Vector3 NewPosition = new Vector3 (movement.x, movement.y, movement.y/5) + transform.position;
        if (Physics2D.OverlapCircle(NewPosition, .2f, MovementBlockers))
        {
            transform.position = NewPosition;
            return true;
            //Debug.Log("Moving to a position");
        }
        else if (movement.x * transform.position.x < 0)
        { //If they are spawned outside of lines they can walk linearly in the direction towards 0. hopefully
            transform.position += Vector3.right * movement.x;
            return false;
            //Debug.Log("Cant move, outside of walk");
        }
        else {
            return false;
            //Debug.Log("Cant move " + movement);
        }

    }

    //Very similar to Unitys movetowards, but ignores the z in favor of my method
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
