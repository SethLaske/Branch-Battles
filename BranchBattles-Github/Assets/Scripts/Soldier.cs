using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : Unit
{
    public GameObject HealthBar;    //Health bar is currently handled here... probably shouldnt
    public float AppearanceTime = 1.5f;
    private float maxHealth = .5f;
    public float HealthTimer = 0;

    //A series of tests to try and improve the appearance of troops as they move back and forth in a group
    //private float FrontStoppingDistance = 1;
    public float AdvanceIndicatorDistance = 1.3f;
    public float RetreatIndicatorDistance = .85f;
    //private float BackCheckDistance = -1.2f;
    //private float FrontCheckDistance = 1;

    //Lets the unit know what is ahead and behind them
    public bool ManAhead;
    public bool ManBehind;

    public AudioSource attackSound;
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = HP;
        AttackTimer = AttackCooldown;
        State = "Walk";
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

        //Checks if anything is ahead or behind it

        //The one can likely be replaced with larger/smaller units sizes to do stuff like that
        //Attack Range must be greater than this check distance
        //Debug.Log("Coords to check ahead are: " + (transform.position + new Vector3(.9f * Team, 0, 0)));
        if (Physics2D.OverlapCircle(transform.position + new Vector3(.9f * Team, 0, 0), .1f, MovementBlockers))
        { //Checks if there is anything one length ahead
            ManAhead = true;
        }
        else {
            ManAhead = false;
        }

        if (Physics2D.OverlapCircle(transform.position + new Vector3(-1.1f * Team, 0, 0), .1f, MovementBlockers))
        { //Checks if there is anything one length behind
            ManBehind = true;
        }
        else
        {
            ManBehind = false;
        }


        //Performs the various actions per the state it is in
        if (State == "Wait")
        {
            Wait();
        }
        else if (State == "Walk")
        {
            Walk();
        }
        else if (State == "Attack")
        {
            Attack();
        }
        else if (State == "Retreat")
        {
            Retreat();
        }

        
    }

    public override void Wait()
    {
            if (Target != null) //Transitions for when there is a target
            {
                //Debug.Log(Vector3.Distance(transform.position, Target.transform.position));

                if (Vector3.Distance(transform.position, Target.transform.position) < AttackRange)
                {    //The target is within range
                    State = "Attack";
                }
                else if ((General.RallyPoint + (AgroRange * Team)) * Team < Target.transform.position.x * Team)  //Hiding this for now as well && ManAhead == false)
                {   //We want to reach the target and no one is infront of us
                    //if man ahead is true its not the enemy
                    //Debug.Log("Need to advance forward my max target is: " + ((General.RallyPoint + (AgroRange * Team)) * Team) + "And the enemy is at " + Target.transform.position.x);
                    State = "Walk";
                }
                else {  //If we reach this point, then it has a target it cant reach. If the target is out of range, this covers it. If its falsely removing it, the Collision detector will add it back

                    //Just going to do nothing to see what happens (might need to cover retreating here later)
                    State = "Retreat";
                    Target = null;
                }
            }
            else
            {
                if (General.RallyPoint * Team < (transform.position.x * Team) - tolerance)  //If the rally point is behind, we always prioritize that
                {
                    State = "Retreat";
                }
                else if (General.RallyPoint * Team > (transform.position.x * Team) + tolerance && !Physics2D.OverlapCircle(transform.position + new Vector3(AdvanceIndicatorDistance * Team, 0, 0), .1f, MovementBlockers))     //Needs to walk forward and the next guy up has given him room
                {
                    State = "Walk";
                }
                else if (Physics2D.OverlapCircle(transform.position + new Vector3(RetreatIndicatorDistance * Team, 0, 0), .1f, MovementBlockers))
                {     //Physics2D.OverlapCircle(transform.position + new Vector3(RetreatIndicatorDistance * Team, 0, 0), .1f, MovementBlockers) will use this to replace it, expecting to bounce back and forth currently
                    State = "Retreat";  //Moving backwards when someone is too close to us
                }
            }
        

    }

    public override void Walk()
    {

        if (Target == null && General.RallyPoint * Team < (transform.position.x * Team)) {   //There is no target and they only respect the rally point
            State = "Retreat";
        }
        else if( Target != null && (General.RallyPoint + (AgroRange * Team)) * Team < Target.transform.position.x * Team)   //or there is a target, but its beyond the agro range
            {
                State = "Retreat";
            }
        
        else
        {
            if (ManAhead == false)
            {
                transform.position += new Vector3(MoveSpeed * Time.deltaTime, 0, 0);
                if (Target != null && (Vector3.Distance(transform.position, Target.transform.position) < AttackRange))   //or there is a target, but its beyond the agro range
                {
                    State = "Attack";
                }
            }
            else {
                State = "Wait"; //If theres someone ahead, need to wait
            }
        }

        //Letting wait do a lot of the work right now. Walk will switch to retreat, walk forward, or transition to wait

        /*
        if (Target == null)
        {  
            if (General.RallyPoint * Team < (transform.position.x * Team))
            {
                State = "Wait";
            }
            else if (Physics2D.OverlapCircle(transform.position + new Vector3(FrontStoppingDistance * Team, 0, 0), .1f, MovementBlockers))
            {
                Stopped = true;
                State = "Wait";
               
            }
        }
        else
        {  
            if (Vector3.Distance(transform.position, Target.transform.position) < AttackRange)
            {
                State = "Attack";
            }
            else if (Physics2D.OverlapCircle(transform.position + new Vector3(FrontStoppingDistance * Team, 0, 0), .1f, MovementBlockers))
            {
                Stopped = true;
                State = "Wait";

            }
        }
        */

    }

    //I think we only want to be in attack if we are actively attacking, otherwise it will be sent to a different state
    public override void Attack()
    {
        //if ((General.RallyPoint + AgroRange) * Team < (transform.position.x * Team))  //If the rally point is behind, we always prioritize that
        //{
        //    State = "Retreat";
        //}

        if (Target != null)
        {
            if ((General.RallyPoint + (AgroRange * Team)) * Team < Target.transform.position.x * Team)  //If the rally point is behind, we always prioritize that
            {
                State = "Retreat";
            }
            if (Mathf.Abs(transform.position.x - Target.transform.position.x) < AttackRange)
            {
                if (AttackTimer > AttackCooldown)
                {
                    Target.TakeDamage(Damage);
                    attackSound.Play();
                    AttackTimer = 0;
                }
                else
                {
                    AttackTimer += Time.deltaTime;
                }
            }
            
            else
            {
                State = "Walk";
            }
        }
        else
        {
            State = "Wait";
        }
    }

    public override void Retreat()
    {
        if (General.RallyPoint * Team > (transform.position.x * Team) && !Physics2D.OverlapCircle(transform.position + new Vector3(RetreatIndicatorDistance * Team, 0, 0), .1f, MovementBlockers))  //If the rally point is ahead we prioritize that to stop retreating
        {
            State = "Wait";
        }
        else if (Physics2D.OverlapCircle(transform.position + new Vector3(-.65f * Team, 0, 0), .1f, MovementBlockers)) { 
            //pass
        }
        else if (ManBehind == true) //(Physics2D.OverlapCircle(transform.position + new Vector3(BackCheckDistance * Team, 0, 0), .1f, MovementBlockers))  Maybe switch back to it later
        {
            transform.position += new Vector3(-MoveSpeed * .5f * Time.deltaTime, 0, 0);
            //Debug.Log("Backing up slowly");
            //State = "Wait"; //Allow for recalculating and slowing
        }
        else
        {
            transform.position += new Vector3(-MoveSpeed * 1.25f * Time.deltaTime, 0, 0);



        }
        

    }

    public override void TakeDamage(float Damage) {
        base.TakeDamage(Damage);
        HealthBar.transform.localScale = new Vector3(HP / maxHealth, HealthBar.transform.localScale.y, HealthBar.transform.localScale.z);
        //slider.value = HealthObject.HP;
        HealthBar.SetActive(true);
        HealthTimer = 0;
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
                else if (Vector3.Distance(transform.position, thing.transform.position) < Vector3.Distance(transform.position, Target.transform.position))
                {
                    Target = thing;
                }
                State = "Attack";

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

}
