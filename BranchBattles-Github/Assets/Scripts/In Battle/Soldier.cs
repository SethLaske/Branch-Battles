using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : Unit
{
    //public bool Assembled;
    public float FullSpaces;
    public float Tolerance = 2; //Readding this... for now
    public float RearPoint;

    //public bool ManAhead;
    //public bool ManBehind;

    

    public float separationDistance = .5f;
    public float separationForce = 2f;

    public List<Unit> nearbyUnits = new List<Unit>();


    // Start is called before the first frame update
    void Start()
    {
        StandardStart();
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

        FullSpaces = 0;
        for (int i = 1; i < unitClassification; i++) {  //Skip 0. The Pacifists will all be at 0, and shouldnt affect troops positioning
            /*if (General.troopCategory[i] == 0) { //==0 would be sufficient but I expect problems
                Debug.Log(i);
                EmptySpaces++;
            }*/
            FullSpaces += Mathf.CeilToInt((float)General.troopCategory[i]/5);
            //Debug.Log(General.troopCategory[i] + " Becomes" + Mathf.CeilToInt((float)General.troopCategory[i] / 5));
        }

        AssemblePoint = General.RallyPoint - Team * General.Spacing *(FullSpaces);
        RearPoint = AssemblePoint - General.Spacing * Mathf.CeilToInt((float)General.troopCategory[unitClassification] / 5);

        Vector3 separation = Vector3.zero;

        /*Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 3);
        foreach (Collider2D collider in colliders)
        {
            Unit unit = collider.GetComponent<Unit>();
            if (collider.gameObject != this.gameObject && unit != null) {
                Debug.Log("The soldiers collider is overlapping with " + collider.gameObject.name);

                Vector3 diff = transform.position - collider.transform.position;
                if (diff.magnitude < separationDistance)
                {
                    separation += new Vector3(Mathf.Sign(diff.x) / (diff.magnitude), Mathf.Sign(diff.x) / diff.magnitude, Mathf.Sign(diff.x) / (diff.magnitude * 5));


                }
            }
            
        }*/
        foreach (Unit unit in nearbyUnits)
        {
            if (unit != null)
            {
                Vector3 diff = transform.position - unit.transform.position;
                if (diff.magnitude < separationDistance)
                {
                    separation += new Vector3(Mathf.Sign(diff.x), Mathf.Sign(diff.y), Mathf.Sign(diff.y) / ( 5));

                    //Random.Range(-1, 1)
                }
            }
            else {
                //nearbyUnits.Remove(unit);
            }
            
        }
        separation *= separationForce * Time.deltaTime;
        //transform.position += separation;
        this.Move(separation);


        
        if (Target == null)
        {
            if (AssemblePoint * Team < (transform.position.x * Team))  //If the rally point is behind, we always prioritize that
            {
                State = "Walk";
                animator.SetBool("Waiting", false);
            }
        }
        else {
            if ((AssemblePoint + (AgroRange * Team)) * Team < Target.transform.position.x * Team)  //If the rally point is behind, we always prioritize that
            {
                State = "Walk";
                animator.SetBool("Waiting", false);
                //Debug.Log("Attack -> Retreat");
                Target = null;
                //Debug.Log("Setting target as null");
            }
            
        }


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
            animator.SetBool("Attacking", true);
        }
        else if (State == "Retreat")    //Retreat doesnt really exisit anymore
        {
            //Debug.Log("State is retreat");
            Retreat();
            //animator.SetBool("Attacking", false);
        }

        
    }

    public override void Wait()
    {
        if (Target == null && ((RearPoint * Team > (transform.position.x * Team)) ||
                               (AssemblePoint * Team < (transform.position.x * Team) )) )
        {
            State = "Walk";
            animator.SetBool("Waiting", false);
            //Debug.Log("Rally point is ahead: Wait > Walk");
        } else if (Target != null && ((AssemblePoint + (AgroRange * Team)) * Team > Target.transform.position.x * Team)){
            State = "Walk";
            animator.SetBool("Waiting", false);
            //Debug.Log("Target is near: Wait > Walk");
        }
        else {
            base.Wait();

            //Seperation applied to update

        }
        
    }

    public override void Walk()
    {
        if (Target != null) 
        {
            if (Vector3.Distance(transform.position, Target.transform.position) <= AttackRange)
            {
                State = "Attack";
                //Debug.Log("Walk > Attack");
            }
            else {
                base.Walk();
            }
            
        }

        //else if ((General.RallyPoint * Team < ((transform.position.x + (2 * (unitClassification - EmptySpaces))) * Team) + Tolerance / 4) &&
          //                     (General.RallyPoint * Team > ((transform.position.x + (2 * (unitClassification - EmptySpaces))) * Team) - Tolerance / 4))
        else if ((RearPoint * Team < (transform.position.x * Team) ) &&
                               (AssemblePoint * Team > (transform.position.x * Team) ))
        {
            State = "Wait";
            //Debug.Log("Walk > Wait");
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
        else {
            if (Vector3.Distance(transform.position, Target.transform.position) > AttackRange)
            {
                State = "Walk";
                animator.SetBool("Attacking", false);
                //Debug.Log("Attack > Walk");
            }
            else {
                //Debug.Log("In attack state and trying to attack");
                base.Attack();
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

    /*
     public class TroopController : MonoBehaviour {
    public float separationDistance = 2f;
    public float separationForce = 1f;

    private List<GameObject> nearbyTroops;

    void Start () {
        nearbyTroops = new List<GameObject>();
    }

    void Update () {
        // Calculate separation force based on nearby troops
        Vector3 separation = Vector3.zero;
        foreach (GameObject troop in nearbyTroops) {
            Vector3 diff = transform.position - troop.transform.position;
            if (diff.magnitude < separationDistance) {
                separation += diff.normalized / diff.magnitude;
            }
        }
        separation *= separationForce;

        // Apply separation force to troop's movement
        Vector3 movement = Vector3.zero;
        // ... add other movement behaviors (e.g. alignment, cohesion)
        movement += separation;
        transform.position += movement * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Troop")) {
            nearbyTroops.Add(other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Troop")) {
            nearbyTroops.Remove(other.gameObject);
        }
    }
}

    */

}
