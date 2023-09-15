using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : Unit
{
    public bool assembled;

    private float FullSpaces;

    private float DistanceFromMiddlePoint;
    private float MaxDistanceFromMiddlePoint;


    public float separationDistance = .5f;
    public float separationForce = 2f;
    public float minimumForce;

    public GameObject RedAura;

    [SerializeField] float shieldDetectionRadius;

    
    [SerializeField] Vector2 targetDetectionBoxSize;
    [SerializeField] Transform targetDetectionCenter;

    // Start is called before the first frame update
    void Start()
    {
        StandardStart();
        StartCoroutine(SpreadStart(.3f, Random.Range(-3f, 3f)));    //Provides some organic movement away from the tent, thx Hudson

        
        RedAura.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.gameState != GameState.InGame) return; 

        //Deals with timer for the health bar
        if (HealthTimer < AppearanceTime)
        {
            HealthTimer += Time.deltaTime;
        }
        else if (HealthTimer > AppearanceTime)
        {
            HealthBar.SetActive(false);
            HealthTimer = AppearanceTime; //Stops the timer from continuing to add
        }

        
        
        CalculateAssemblePoints();

        if(State != "Charge" && State != "Attack")   SpreadOut();    //Performs flocking mechanics every update, regardless of state

        FindTarget();

        //Performs the various actions per the state it is in
        if (State == "Wait")
        {
            
            Wait();
            //animator.SetBool("Waiting", true);
        }
        else if (State == "Walk")
        {
            
            FindShield();   //Only need to check it if we are actually walking
            Walk();
        }
        else if (State == "Attack")
        {
           
            Attack();
            //animator.SetBool("Attacking", true);
        }
        else if (State == "Charge")    //Only can be given by the king
        {
            
            Charge();
            //animator.SetBool("Attacking", false);
        }
       
    }

   //General structure for states to follow
    //Conditions to change out of state
    //Base State
    //Additional requirements

    public override void Wait()
    {
        if (IsWithinAssemble() == false)  //No target and needs to walk
        {
            State = "Walk";
            assembled = false;
            
            return;

        } else if (IsTargetAggroable() == true){    //There is a target, and we can reach them
            State = "Walk";
            assembled = false;
           
            return;
        }
        
            
        base.Wait();

        //Attempting to move things forward so they are at the front of their zone, rather than the middle. Might allow for smaller spacing, and looks far better for front row
        //I also want them to spread spaces above and below them if they cant move forward to allow for a good looking army, but ideally not completely grid like

        if (assembled == true) {
            animator.SetBool("Walking", false);
            return;
        }

        animator.SetBool("Walking", true);

        if ((AssemblePoint * Team) - .3f > (transform.position.x * Team))
        {
            bool FUp = true;
            bool FDown = true;

            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, .75f);
            foreach (Collider2D collider in colliders)
            {
                Soldier soldier = collider.GetComponent<Soldier>();
                if (collider.gameObject != this.gameObject && soldier != null && soldier.assembled == true)
                {
                    Vector3 diff = soldier.transform.position - transform.position;

                    if (diff.y > 0 && (diff.x * Team > .1f))
                    {
                        FUp = false;
                    }

                    if (diff.y < 0 && (diff.x * Team > .1f))
                    {
                        FDown = false;
                    }
                }
            }

            if (FUp && FDown) //Nothing in front of its box
            {
                this.Move(new Vector3(1 * Team * currentSpeed * Time.deltaTime, 0, 0));
            }
            else if (FUp)  //Nothing above and forward
            {
                if (this.Move(new Vector3(.5f * Team * currentSpeed * Time.deltaTime, .5f * currentSpeed * Time.deltaTime, 0)) == false) {
                    assembled = true;
                }
            }
            else if (FDown)    //Nothing below and forward
            {
                if (this.Move(new Vector3(.5f * Team * currentSpeed * Time.deltaTime, -.5f * currentSpeed * Time.deltaTime, 0)) == false) {
                    assembled = true;
                }
            }
            else
            {
                assembled = true;
            }
        }
        else {
            assembled = true;
        }

        


    }


    public override void Walk()
    {
        animator.SetBool("Walking", true);
        if (IsTargetAggroable() == true && IsTargetAttackable() == true)
        {
            State = "Attack";
            return;
        }


        //Trying to get them comfortably within the bounds, rather than forming a hard line as soon as they touch it
      
        if(IsWithinAssemble() == true && IsTargetAggroable() == false)
       
        {
            State = "Wait";
            assembled = false;
            return;
        }
       
        base.Walk();
        
        
    }

    public override void Attack()
    {
        if (Attacking == true) {
            base.Attack();
            return;
        }

        if (Target == null) 
        {
            State = "Walk";
            //animator.SetBool("Attacking", false);
        }
        else if (IsTargetAggroable() == false) {
            State = "Walk";
            //animator.SetBool("Attacking", false);
        }
        else if (IsTargetAttackable() == false)     //Both are needed to ensure the troop will not get ahead on accident
        {
            State = "Walk";
            //animator.SetBool("Attacking", false);
        }
        else
        {
            base.Attack();
            //animator.SetBool("Attacking", true);
        }
        
    }

    

    //Cant use the base functions, once in general it either walks forward, walks at, or attacks. No way to change its state from here
    public void Charge() {
        
        if (Target == null)
        {
            this.Move(new Vector3(Mathf.Sign(Team) * currentSpeed * Time.deltaTime, 0, 0));
            transform.localScale = new Vector3(Team, 1, 1);
        }
        else if (Vector3.Distance(transform.position, Target.transform.position) > AttackRange && Attacking == false) {
            this.Move(Advance(transform.position, Target.transform.position, Mathf.Abs(currentSpeed) * Time.deltaTime));
            transform.localScale = new Vector3(Mathf.Sign(Target.transform.position.x - transform.position.x), 1, 1);
            //animator.SetBool("Attacking", false);
        }
        else
        { //kill
            base.Attack();
            //animator.SetBool("Attacking", true);
        }
    
    }

    public IEnumerator SpreadStart(float duration, float spread) {

        while (duration > 0) {
            yield return new WaitForSeconds(Time.deltaTime);
            duration -= Time.deltaTime;

            Move(Vector2.up * spread * Time.deltaTime);
        
        }
    }

    /// <summary>
    /// Unit becomes unaffected by player orders, and only moves forward and attacks all enemies
    /// </summary>
    public void ReceiveGeneralOrders() {
        //Change state to Charge
        //Apply visual affects and animation
        State = "Charge";
        animator.SetBool("Walking", true);
        //animator.SetBool("Attacking", false);
        RedAura.SetActive(true);
    }



    public void CalculateAssemblePoints() {
        //Fun Stuff Below
        FullSpaces = 0;
        for (int i = 1; i < unitClassification; i++)
        {  //Skip 0. The Pacifists will all be at 0, and dont affect troops positioning
            FullSpaces += Mathf.CeilToInt((float)General.troopCategory[i] / 5);   //Standard, each troop will get 5 units to a spacing
        }

        float temp = General.rallyPoint - Team * General.spacing * (FullSpaces);
        if (AssemblePoint != temp)
        {
            assembled = false;
        }
        AssemblePoint = temp;  //Moves it back to account for troops further up
        RearPoint = AssemblePoint - Team * General.spacing * Mathf.CeilToInt((float)General.troopCategory[unitClassification] / 5); // Allocated the back room by its own classification

        DistanceFromMiddlePoint = ((AssemblePoint + RearPoint) / 2 - transform.position.x); //takes average, which is the middle and subtracts its position (positive value implies its to the left)
        MaxDistanceFromMiddlePoint = Mathf.Abs((AssemblePoint - RearPoint) / 2);    //needed for ratios later, could be calculated with classifications but this is easier
    }


    private void SpreadOut() {

        Vector3 separation = Vector3.zero;

        //This is more consistent than the list of troops that need to be accounted for (but less efficient)
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

            separation += new Vector3(Mathf.Sign(diff.x), 2 * Mathf.Sign(diff.y), 2 * Mathf.Sign(diff.y) / (5))/(diff.magnitude * 2);

        }
        
        
        if (Mathf.Sign(separation.x) != Mathf.Sign(DistanceFromMiddlePoint))
        { //If the seperation is the same direction as the distance then we want to apply the weaker force in that direction (want to push things inward rather than outward)
            separation.x *= Mathf.Abs((MaxDistanceFromMiddlePoint - Mathf.Abs(DistanceFromMiddlePoint)) / MaxDistanceFromMiddlePoint);  //Gradual scaling back, a unit cant be pushed out
            
        }
        

        //.1 seems to be a really good value to avoid shaking in place
        if (separation.magnitude > minimumForce) {
            separation *= separationForce * Time.deltaTime;
            if (State == "Walk") {
                separation /= 4f;    //Cuts seperation while walking
            }
            this.Move(separation);
        }
        
    }

    protected void FindTarget()
    {
        if (Target != null && Mathf.Abs(transform.position.x - Target.transform.position.x) > AgroRange) {
            Target = null;
        }
        if (Attacking) return;

        Collider2D[] colliders = Physics2D.OverlapBoxAll(targetDetectionCenter.position, targetDetectionBoxSize, 0);
        foreach (Collider2D collider in colliders)
        {
            Damageable damageable = collider.GetComponent<Damageable>();
            if (damageable != null && damageable.Team != Team)
            {
                //Check conditions for assigning a target. I could maybe use this to try and break up every unit from targetting the same unit?
                if (Target == null) {
                    Target = damageable;
                }
                else if (Vector3.Distance(transform.position, damageable.transform.position) < Vector3.Distance(transform.position, Target.transform.position))
                {
                    Target = damageable;
                }

            }

        }
    }

    private void FindShield()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, shieldDetectionRadius);
        foreach (Collider2D collider in colliders)
        {
            Soldier soldier = collider.GetComponent<Soldier>();
            if (soldier != null && soldier.Team == Team)
            {
                if (CheckShieldConditions(soldier) == true)
                {
                    humanshield = soldier;
                }
            }

        }
    }

    private bool CheckShieldConditions(Soldier soldier)
    {
        if (soldier == null)
        {  //This better be redundant
            return false;
        }
        if (soldier.unitClassification >= unitClassification) //Something we actually want to stay behind
        {
            return false;
        }
        if (soldier.Team != Team)
        {
            return false;
        }
        if (humanshield == null)    //Is the shield currently empty
        {
            return true;
        }
        //Now there are some decisions
        //Should it fall behind the closest in class, for now I will say yes
        if (humanshield.unitClassification > soldier.unitClassification)
        {
            return false;
        }
        //Will we follow the closest? I say yes
        if (Vector3.Distance(transform.position, humanshield.transform.position) < Vector3.Distance(transform.position, soldier.transform.position))
        {
            return false;
        }

        return true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, shieldDetectionRadius);
        if (humanshield != null) {
            Gizmos.DrawLine(transform.position, humanshield.transform.position);
        }
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(targetDetectionCenter.position, (targetDetectionBoxSize));
        if (Target != null) {
            Gizmos.DrawLine(transform.position, Target.transform.position);
        }
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + transform.localScale.x * AgroRange, transform.position.y, 0));
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + transform.localScale.x * AttackRange, transform.position.y, 0));


        
    }

    
}
