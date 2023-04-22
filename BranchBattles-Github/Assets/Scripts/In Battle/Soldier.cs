using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : Unit
{
    //public bool Assembled;
    public float FullSpaces;
    public float Tolerance = 2; //Readding this... for now


    //public bool ManAhead;
    //public bool ManBehind;

    private float DistanceFromMiddlePoint;
    private float MaxDistanceFromMiddlePoint;


    public float separationDistance = .5f;
    public float separationForce = 2f;
    public float minimumForce;

    public GameObject RedAura;
    //public bool Highlited = false;
    public List<Unit> nearbyUnits = new List<Unit>();
    

    // Start is called before the first frame update
    void Start()
    {
        StandardStart();
        StartCoroutine(SpreadStart(.3f, Random.Range(-3f, 3f)));
        if (general == null) {
            RedAura.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
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

        
        if (general != null)
        {
            if (general.SelectedSoldier == this) {
                RedAura.SetActive(true);
            }
            else
            {
                RedAura.SetActive(false);
            }
        }
        

        FullSpaces = 0;
        for (int i = 1; i < unitClassification; i++) {  //Skip 0. The Pacifists will all be at 0, and shouldnt affect troops positioning
            
            FullSpaces += Mathf.CeilToInt((float)General.troopCategory[i]/5);
            //Debug.Log(General.troopCategory[i] + " Becomes" + Mathf.CeilToInt((float)General.troopCategory[i] / 5));
        }

        AssemblePoint = General.RallyPoint - Team * General.Spacing *(FullSpaces);
        RearPoint = AssemblePoint - Team * General.Spacing * Mathf.CeilToInt((float)General.troopCategory[unitClassification] / 5);

        DistanceFromMiddlePoint = ((AssemblePoint + RearPoint) / 2 - transform.position.x);
        MaxDistanceFromMiddlePoint = Mathf.Abs((AssemblePoint - RearPoint) / 2);

        SpreadOut();

        //Performs the various actions per the state it is in
        if (State == "Wait")
        {
            //Debug.Log("State is wait");
            Wait();
            animator.SetBool("Waiting", true);
        }
        else if (State == "Walk")
        {
            //Debug.Log("State is walk");
            Walk();
            animator.SetBool("Waiting", false);
            animator.SetBool("Attacking", false);
        }
        else if (State == "Attack")
        {
            //Debug.Log("State is attack");
            Attack();
            //animator.SetBool("Attacking", true);
        }
        else if (State == "Retreat")    //Retreat doesnt really exisit anymore
        {
            //Debug.Log("State is retreat");
            Retreat();
            //animator.SetBool("Attacking", false);
        }
        else if (State == "GeneralCharge")    //Retreat doesnt really exisit anymore
        {
            //Debug.Log("State is to die by command of the king");
            GeneralCharge();
            //animator.SetBool("Attacking", false);
        }


    }

    public override void Wait()
    {
        if (Target == null && IsWithinAssemble() == false)
        {
            State = "Walk";
            animator.SetBool("Waiting", false);
            //Debug.Log("Rally point is ahead: Wait > Walk");
        } else if (IsTargetAggroable() == true){
            State = "Walk";
            animator.SetBool("Waiting", false);
            //Debug.Log("Target is near: Wait > Walk");
        }
        else {
            if (Mathf.Abs((MaxDistanceFromMiddlePoint - Mathf.Abs(DistanceFromMiddlePoint)) / MaxDistanceFromMiddlePoint) > .3f)
            {
                //Debug.Log("Waiting but shiftable");
                //bool behind = false;
                //bool front = false;
                //bool up = false;
                //bool down = false;

                bool FUp = false;
                bool FDown = false;

                Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(separationDistance, separationDistance), 0);
                foreach (Collider2D collider in colliders)
                {
                    Unit unit = collider.GetComponent<Unit>();
                    if (collider.gameObject != this.gameObject && unit != null)
                    {
                        Vector3 diff = unit.transform.position - transform.position;

                        if (diff.y > 0 && diff.x * Team > 0) {
                            FUp = true;
                        }

                        if (diff.y < 0 && diff.x * Team > 0)
                        {
                            FDown = true;
                        }

                        if ((diff.x * Team) - Mathf.Abs(diff.y/2) > 0)
                        {
                            //Debug.Log("Something is in front of me");
                            //front = true;
                        }
                        else if ((diff.y) - Mathf.Abs(diff.x) > 0)
                        {
                            
                            //up = true;
                        }
                        else if ((diff.y < 0) && (diff.x * Team > 0))
                        {

                            //down = true;
                        }
                    }

                }

                if (!FUp && !FDown)
                {
                    Debug.Log("Both, forward");
                    this.Move(new Vector3(.8f * Team * MoveSpeed * Time.deltaTime, 0, 0));
                }
                else if (!FUp) {
                    //Debug.Log("Up");
                    this.Move(new Vector3(.5f * Team * MoveSpeed * Time.deltaTime, .5f * MoveSpeed * Time.deltaTime, 0));
                }
                else if (!FDown)
                {
                    //Debug.Log("Down");
                    this.Move(new Vector3(.5f * Team * MoveSpeed * Time.deltaTime, -.5f * MoveSpeed * Time.deltaTime, 0));
                }

                /*if (front == false)
                {
                    //this.Move(new Vector3(.8f * Team * MoveSpeed * Time.deltaTime, 0, 0));
                } else if (up == false)
                {
                    //this.Move(new Vector3(0, .5f * Team * MoveSpeed * Time.deltaTime, 0));
                }*/
                

            }
            base.Wait();

            //Seperation applied to update

        }
        
    }

    public override void Walk()
    {
        if (Target != null) 
        {
            if (IsTargetAggroable() == true && IsTargetAttackable() == true)
            {
                State = "Attack";
            }
            
            else {
                base.Walk();
            }
            
        }

        //else if ((General.RallyPoint * Team < ((transform.position.x + (2 * (unitClassification - EmptySpaces))) * Team) + Tolerance / 4) &&
          //                     (General.RallyPoint * Team > ((transform.position.x + (2 * (unitClassification - EmptySpaces))) * Team) - Tolerance / 4))
        else if (((RearPoint + MaxDistanceFromMiddlePoint * Team) * Team < (transform.position.x * Team) ) &&
                               ((AssemblePoint - MaxDistanceFromMiddlePoint * Team/ 3) * Team > (transform.position.x * Team) ))
        
        {
            State = "Wait";
            Debug.Log("Walk > Wait at X coord: " + transform.position.x);
        }
        else
        {
            base.Walk();
        }
        
    }

    public override void Attack()
    {

        if (Target == null)
        {
            State = "Walk";
            animator.SetBool("Attacking", false);
            //Debug.Log("Attack > Wait");
        }
        else if (IsTargetAggroable() == false) {
            State = "Walk";
            animator.SetBool("Attacking", false);
        }
        else
        {
            if (IsTargetAttackable() == false)
            {
                State = "Walk";
                animator.SetBool("Attacking", false);
                //Debug.Log("Attack > Walk");
            }
            else
            {
                //Debug.Log("In attack state and trying to attack");
                base.Attack();
                animator.SetBool("Attacking", true);
            }

        }
        
    }

    public override void Retreat()
    {
        if (AssemblePoint * Team > (transform.position.x * Team))
        {
            State = "Wait";
            //Debug.Log("Retreat > Wait");
        }
        else {
            base.Retreat();
        }
        
    }

    public void GeneralCharge() {
        if (Target == null)
        {
            this.Move(new Vector3(Mathf.Sign(Team) * MoveSpeed * Time.deltaTime, 0, 0));
        }
        else if (Vector3.Distance(transform.position, Target.transform.position) > AttackRange) {
            this.Move(Advance(transform.position, Target.transform.position, Mathf.Abs(MoveSpeed) * Time.deltaTime));
        }
        else
        { //kill
            base.Attack();
            animator.SetBool("Attacking", true);
        }
    
    }
    public IEnumerator SpreadStart(float duration, float spread) {

        while (duration > 0) {
            yield return new WaitForSeconds(Time.deltaTime);
            duration -= Time.deltaTime;

            Move(Vector2.up * spread * Time.deltaTime);
        
        }
    }

    //Sets the target as the closest available target
    private void OnTriggerStay2D(Collider2D collision)
    {
       
        Damageable thing = collision.GetComponent<Damageable>();
        if (thing != null)    //Need to change this to a damageable check
        {
            if (thing.Team != Team)
            {
                //Debug.Log("Viable Target found");
                if (Target == null) //No target around found
                {
                    //Debug.Log("Adding Target");
                    Target = thing;
                }
                else if (Vector3.Distance(transform.position, thing.transform.position) < Vector3.Distance(transform.position, Target.transform.position)/2)
                {
                    Target = thing;
                }
                //State = "Attack";

            }
            else {
                //This automatically opens the gate for units, which is fine for AI, but bad for player
                //Gate blocker = collision.GetComponent<Gate>();
                //if (blocker != null && ManAhead) {
                //    blocker.gateSelected();
                //}
            }

            //else
            //{
            //    State = "Wait";
            //}
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Unit nearUnit = collision.GetComponent<Unit>();
        if (nearUnit != null) {
            if (nearUnit.Team == Team) {
                nearbyUnits.Add(nearUnit);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Unit nearUnit = collision.GetComponent<Unit>();
        if (nearUnit != null)
        {
            if (nearUnit.Team == Team)
            {
                nearbyUnits.Remove(nearUnit);
            }
        }
    }

    private void SpreadOut() {

        Vector3 separation = Vector3.zero;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, separationDistance);
        foreach (Collider2D collider in colliders)
        {
            Unit unit = collider.GetComponent<Unit>();
            if (collider.gameObject != this.gameObject && unit != null)
            {
                //Debug.Log("The soldiers collider is overlapping with " + collider.gameObject.name);

                Vector3 diff = transform.position - collider.transform.position;
                if (diff.magnitude < separationDistance)
                {
                    separation += new Vector3(Mathf.Sign(diff.x), 2 * Mathf.Sign(diff.y), 2 * Mathf.Sign(diff.y) / (5))/(diff.magnitude * 2);
                }
            }

        }
        
        //Debug.Log("Dist from mid point: " + DistanceFromMiddlePoint);
        //Debug.Log("Max Dist from mid point: " + MaxDistanceFromMiddlePoint);
        //Debug.Log("Percent from end point: " + Mathf.Abs((MaxDistanceFromMiddlePoint - Mathf.Abs(DistanceFromMiddlePoint)) / MaxDistanceFromMiddlePoint));

        if (Mathf.Sign(separation.x) != Mathf.Sign(DistanceFromMiddlePoint))
        { //If the seperation is the same direction as the distance then we want to apply the weaker force in that direction
            separation.x *= Mathf.Abs((MaxDistanceFromMiddlePoint - Mathf.Abs(DistanceFromMiddlePoint)) / MaxDistanceFromMiddlePoint);
            //Debug.Log("Weakening the force");
        }
        //Debug.Log("Seperation Magnitude: " + separation.magnitude);
       
        //.1 seems to be a really good value
        if (separation.magnitude > minimumForce) {
            separation *= separationForce * Time.deltaTime;
            if (State == "Walk") {
                separation /= 4;
            }
            this.Move(separation);
        }
        
    }

}
