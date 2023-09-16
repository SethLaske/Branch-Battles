using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Super class for all units that can be trained, and fight/have utility  
public class Unit : Damageable
{
    [Header("Unique Identifiers")]
    public Animator animator;
    public string unitName;     //called to show on buttons etc...
    public int UnitNumber;  //Used to code the unit for save data. These should each be unique, and will need to be updated in the UnitCoder.
    public int unitClassification;
    public WeaponAttack Offense;
    protected bool Attacking = false;
    //Various Stats each unit has
    [Header("Stats")]
    public float baseSpeed; //Speed to travel across the map
    protected float currentSpeed;
    public float Damage;    //Damage done per attack
    public float AttackRange;   //Distance from enemy to deal damage
    public float AgroRange;     //The max distance from the rally point a troop will pursue their target
    public float attackHitTime;    //Time from animation start to activating hitbox
    public int Cost;        //Gold required to train the unit
    public int TroopSpaces;     //Troop spaces the unit takes up in the army
    public float SpawnTime;     //Time the unit needs to spawn

    [Header("Misc")]
    public GameObject HealthBar;    //Health bar is currently handled here... probably shouldnt
    protected float healthBarStartScale;
    public float AppearanceTime = 1.5f;

    protected float HealthTimer = 0;

    public string State;    //Determines what actions they need to pursue
    public SpriteRenderer spriteRenderer;
    public Material spriteMaterial;

    public Damageable Target;   //Targets are always for the enemy      //Could go private 
    protected float AssemblePoint;         //Could go private 
    protected float RearPoint;             //Could go private 
    public LayerMask walkableLayer;

    
    public GameObject corpse;
    protected General general;

    public AudioSource attackSound;

    protected float DebuffMult = 1;   //Stores speed multipliers so they can be undone when passed back to the general  

    public Soldier humanshield;

    public AnimationClip attackAnimation;
    public Sprite identifierSprite;

    [SerializeField] protected float attackRangeY;
    private float distanceBehindShield = 1;

    /// <summary>
    /// Initializes the typical start script for the different unit classes, including setting max health, speeds, attack timer, state, sprite color, and assigning its general
    /// </summary>
    public virtual void StandardStart()
    {
        maxHealth = HP;
        currentSpeed = baseSpeed;
        if (State == "" || State == null) State = "Walk";

        if (Team < 0)
        {
            HealthBar.GetComponent<SpriteRenderer>().color = Color.red;
        }
        general = General.general;
        healthBarStartScale = HealthBar.transform.localScale.x;

        ChangeUnitColors();

    }

    /// <summary>
    /// The Unit is roughly in position, and will face towards the enemy base
    /// </summary>
    public virtual void Wait() {
        
        transform.localScale = new Vector3(Team, 1, 1);
       
    }

    /// <summary>
    /// The Unit needs to walk. Will either approach a target to attack, or move in the direction of the teams rally point. 
    /// </summary>
    public virtual void Walk()
    {
        
        if (humanshield != null && transform.position.x * Team < (AssemblePoint + RearPoint) / 2 * Team) { // Will try and stay behind the shield, but only when it needs to walk forward to its destination

            float distanceFromGoal = 0;
            float distanceFromShield = Mathf.Abs(humanshield.transform.position.x - transform.position.x);
            float shieldDistanceFromGoal = 0;

            if (Target != null)
            {
                distanceFromGoal = Mathf.Abs(Target.transform.position.x - transform.position.x);
                shieldDistanceFromGoal = Mathf.Abs(Target.transform.position.x - humanshield.transform.position.x);
            }
            else { 
                distanceFromGoal = Mathf.Abs(General.rallyPoint - transform.position.x);
                shieldDistanceFromGoal = Mathf.Abs(General.rallyPoint - humanshield.transform.position.x);
            }

            if (Target != null && humanshield.State == "Attack")
            {
                //Handle Attacks
            } else if (Target != humanshield.Target) { 
                //skip
            }
            else if (shieldDistanceFromGoal + distanceBehindShield > distanceFromGoal)
            {
                //get behind shield
                RetreatBehindShield();
                return;
            }
            else {
                //Walk behind shield
                WalkBehindShield();
                return;
            }
      

        }

        
        float x = 0;

        if (Target != null && IsTargetAggroable() == true)
        {
            x = Mathf.Sign(Target.transform.position.x - transform.position.x);
            Vector3 spreadVector = GetSpreadVector(.5f);
            if (Vector3.Angle(spreadVector, Target.transform.position - transform.position) < 90)
            {
                //Debug.Log("target and agrroable");
                this.Move(Advance(transform.position, Target.transform.position, Mathf.Abs(currentSpeed) * Time.deltaTime));
            }
            else {
                this.Move(new Vector3(0, spreadVector.y) * Time.deltaTime);
            }
        }
        else {
            //Debug.Log("not target and agrroable");
            float distance = ((AssemblePoint + RearPoint)/2 - transform.position.x);
            this.Move(new Vector3(Mathf.Sign(distance) * currentSpeed * Time.deltaTime, 0, 0));
            x = Mathf.Sign(distance);
        }

        //Debug.Log("Setting the direction from the main walk function");
        if (x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

    }

    private void WalkBehindShield() {
        if (humanshield == null) {
            return;
        }

        //Need to be using the speed of the furthest forward shield.
        Soldier frontHumanSheild = humanshield;
        while (frontHumanSheild.humanshield != null)
        {
            frontHumanSheild = frontHumanSheild.humanshield;
        }

        float direction = humanshield.transform.position.x - (distanceBehindShield * humanshield.transform.localScale.x) - transform.position.x;
        if (frontHumanSheild.currentSpeed > currentSpeed)
        {
            this.Move(new Vector3(Mathf.Sign(direction) * currentSpeed * Time.deltaTime, 0, 0));
        }
        else
        {
            this.Move(new Vector3(Mathf.Sign(direction) * frontHumanSheild.currentSpeed * Time.deltaTime, 0, 0));
        }

        transform.localScale = new Vector3(Mathf.Sign(direction), 1, 1);

        //Debug.Log("Walking behind shield, moving in the " + Mathf.Sign(direction) + " direction, facing in the " + transform.localScale.x);

        return;
    }

    private void RetreatBehindShield()
    {
        if (humanshield == null)
        {
            return;
        }


        float direction = (humanshield.transform.position.x - (distanceBehindShield * humanshield.transform.localScale.x)) - transform.position.x;

        this.Move(new Vector3(Mathf.Sign(direction) * currentSpeed/4 * Time.deltaTime, 0, 0));

        transform.localScale = new Vector3(Mathf.Sign(-1 * direction), 1, 1);

        //Debug.Log("Retreating behind shield, moving in the " + Mathf.Sign(direction) + " direction, facing in the " + transform.localScale.x);
        return;
    }

    /// <summary>
    /// The Unit is within range of attacking a target and will continue to do so while the target is alive and in agro range
    /// </summary>
    public virtual void Attack() {
        if (Target == null)
        {
            return;     
        }

        if (Attacking == false) {
            StartCoroutine(PlayAttack());
        }

        
        //Making slight adjustments to keep enemies cleanly within the attack range
        Vector3 adjustments = new Vector3();
        if (Mathf.Abs(Target.transform.position.x - transform.position.x) > AttackRange * .95f)   
        {
            adjustments.x = (Mathf.Sign(Target.transform.position.x - transform.position.x) * currentSpeed * Time.deltaTime);
            

        }
        if (Mathf.Abs(Target.transform.position.y - transform.position.y) > attackRangeY/2) {
            adjustments.y = (Mathf.Sign(Target.transform.position.y - transform.position.y) * currentSpeed * Time.deltaTime); 
        }

        
        if (unitClassification != 0)
        {
            //Debug.Log("Adjust attack");
            Vector3 spreadVector = GetSpreadVector(.5f) * Time.deltaTime;
            //Debug.Log("Spread Vector" + spreadVector);
            if (adjustments.y * spreadVector.y < 0)
            {
                adjustments.y = 0;
            }
            else if (adjustments.y * spreadVector.y == 0) {
                adjustments.y += spreadVector.y;
            }
            //Debug.Log("Y Adjustments: " + adjustments.y);
        }

       
        Move(adjustments);
       


        
        
        if (Target.transform.position.x - transform.position.x > 0)
        {
            //transform.rotation = Quaternion.Euler(0, 0, 0);
            transform.localScale = new Vector3(1, 1, 1);
        }
        else {
            //transform.rotation = Quaternion.Euler(0, 180, 0);
            transform.localScale = new Vector3(-1, 1, 1);
        } 
    }

    //Calls the attack and provides timings here. One flaw is that they will attack even if the target already died while they are charging
    IEnumerator PlayAttack()   //Might need recover to deal with animations, otherwise easy fix to remove it
    {
        animator.SetBool("Attacking", true);
        Attacking = true;
       
        float attackDebuff = 3;
        DebuffMult *= attackDebuff;

        while (DebuffMult > 100) {
            yield return new WaitForSeconds(.05f);
        }

        yield return new WaitForSeconds(attackHitTime * DebuffMult/attackDebuff);


        while (DebuffMult > 100)
        {
            yield return new WaitForSeconds(.05f);
        }

        attackSound.Play();
        Offense.Attack();


        
        yield return new WaitForSeconds((attackAnimation.length - attackHitTime) * DebuffMult / attackDebuff);

        while (DebuffMult > 100)
        {
            yield return new WaitForSeconds(.05f);
        }


        Attacking = false;
        DebuffMult /= attackDebuff;

        if (IsTargetAggroable() && IsTargetAttackable() && State == "Attack")
        {
            StartCoroutine(PlayAttack());
        }
        else
        {
            animator.SetBool("Attacking", false);

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

        if (Vector3.Distance(transform.position, Target.transform.position) > AttackRange + .2f)
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

        return true;
    }


    protected Vector3 GetSpreadVector(float separationDistance) {
        Vector3 separation = Vector3.zero;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, separationDistance);

        foreach (Collider2D collider in colliders)
        {
            Unit unit = collider.GetComponent<Unit>();
            if (collider.gameObject == this.gameObject || unit == null || unit.Team != Team || unit.unitClassification > unitClassification)
            {
                continue;
            }

            Vector3 diff = transform.position - collider.transform.position;


            if (unit.unitClassification == 0 && State != "Wait") diff = Vector3.zero;   //Miners can only push soldiers when they are waiting



            if (diff == Vector3.zero || diff.magnitude > separationDistance)
            {
                continue;     //Infinity breaks things
            }

            separation += new Vector3(Mathf.Sign(diff.x), 2 * Mathf.Sign(diff.y), 2 * Mathf.Sign(diff.y) / (5)) / (diff.magnitude * 2);

        }

        return separation;
    }

    //Override take damage to allow for the health bar to be displayed
    public override void TakeDamage(float Damage)
    {
        base.TakeDamage(Damage);
        HealthBar.transform.localScale = new Vector3(healthBarStartScale * HP / maxHealth, HealthBar.transform.localScale.y, HealthBar.transform.localScale.z);
        HealthBar.SetActive(true);
        HealthTimer = 0;
    }

    public override void Die() {
        
        General.troopCount -= TroopSpaces;
        General.troopCategory[unitClassification]--;
        General.souls++;
        General.UpdateGeneral();
        corpse = Instantiate(corpse, transform.position + new Vector3(0, -.25f, 0), Quaternion.identity);
        corpse.transform.localScale = new Vector3(corpse.transform.localScale.x * transform.localScale.x, corpse.transform.localScale.y, corpse.transform.localScale.z);
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
        DebuffMult *= Intensity;
        animator.speed /= Intensity;
        
        yield return new WaitForSeconds(Duration);

        DebuffMult /= Intensity;
        animator.speed *= Intensity; 
    }

    public void ChangeUnitColors() {
        spriteMaterial = new Material(spriteRenderer.material);

        if (spriteMaterial == null)
        {
            return;
        }
       
        spriteMaterial.SetColor("_PantColor", General.unitPantsColor);
        spriteMaterial.SetColor("_BeltColor", General.unitBeltColor);
        spriteMaterial.SetColor("_ShoulderColor", General.unitShouldersColor);
        spriteMaterial.SetColor("_HornColor", General.unitHornColor);
        spriteRenderer.material = spriteMaterial;
    }

    //Essentially the same as transform += vector3, but checks to make sure it can step there.
    public bool Move(Vector2 movement)
    {
        //Debug.Log("Move: " + movement + " With a magnitude of: " + movement.magnitude);
        Vector3 NewPosition = new Vector3 (movement.x, movement.y, movement.y/5) / DebuffMult + transform.position;

/*        if (unitClassification != 0 && State == "Attack") {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(NewPosition, .5f, 1 << gameObject.layer);
            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject != gameObject)
                {
                    Debug.Log("Something is already here");
                    return false;
                }
            }
        }*/
        
            
        
        if (Physics2D.OverlapCircle(NewPosition, .2f, walkableLayer) || !Physics2D.OverlapCircle(transform.position, .25f, walkableLayer))
        {
            transform.position = NewPosition;
            return true;
        }
        else if (movement.x * transform.position.x < 0)
        { //If they are spawned outside of lines they can walk linearly in the direction towards 0. hopefully
            transform.position += Vector3.right * movement.x;
            return false;
            
        }
        else {
            return false;
           
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
            return new Vector3(direction.x, direction.y, direction.y / 5) * maxDistanceDelta;
        }

        direction = direction.normalized;
        
        Vector3 displacement = new Vector3(direction.x, direction.y, direction.y/5) * maxDistanceDelta;

        return displacement;
    }

    

}
