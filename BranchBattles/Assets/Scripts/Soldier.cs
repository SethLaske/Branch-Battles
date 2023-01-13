using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : Unit
{

    public bool Stopped = false;

    //private float FrontStoppingDistance = 1;
    public float AdvanceIndicatorDistance = 1.3f;
    public float RetreatIndicatorDistance = .85f;
    //private float BackCheckDistance = -1.2f;
    //private float FrontCheckDistance = 1;

    public bool ManAhead;
    public bool ManBehind;

    // Start is called before the first frame update
    void Start()
    {
        Timer = AttackCooldown;
        State = "Walk";
    }

    // Update is called once per frame
    void Update()
    {
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
            if (Vector3.Distance(transform.position, Target.transform.position) < AttackRange)
            {
                if (Timer <= 0)
                {
                    Target.TakeDamage(Damage);
                    Timer = AttackCooldown;
                }
                else
                {
                    Timer -= Time.deltaTime;
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

    //I broke some stuff so im going to try and rewrite it just using man ahead and man behind

    /*
    public override void Wait()
    {
        if (Target == null)
        { //These lines work, so Im seperating them from the stuff that deals with the enemy.
            if (Stopped == false)
            {
                if (General.RallyPoint * Team > (transform.position.x * Team) + tolerance)
                {
                    State = "Walk";
                }
                else if (General.RallyPoint * Team < (transform.position.x * Team) - tolerance)
                {
                    State = "Retreat";
                }
                else if(Physics2D.OverlapCircle(transform.position + new Vector3(RetreatIndicatorDistance * Team, 0, 0), .1f, MovementBlockers))
                    {
                        //Debug.Log("Needs to back up, soldier is backing in");
                        Stopped = true;
                        State = "Retreat";
                    }
            }
            else
            {
                //Conditions to stop waiting while stopped
                //Retreat
                //The unit in front has moved forward

                if (!Physics2D.OverlapCircle(transform.position + new Vector3(AdvanceIndicatorDistance * Team, 0, 0), .1f, MovementBlockers))
                {
                    //Stopped = false;
                    State = "Walk";
                }
                else
                {
                    if (General.RallyPoint * Team < (transform.position.x * Team) || Physics2D.OverlapCircle(transform.position + new Vector3(RetreatIndicatorDistance * Team, 0, 0), .1f, MovementBlockers))
                    {
                        //Debug.Log("Needs to back up, soldier is backing in");
                        State = "Retreat";
                    }
                }
            }

        }
        else //Witchcraft
        {
            if (Vector3.Distance(transform.position, Target.transform.position) < AttackRange)
            {
                State = "Attack";
            }
            else if (Vector3.Distance(new Vector3(General.RallyPoint, Target.transform.position.y, 0), Target.transform.position) > AgroRange)  //Compares distance the enemy is from the rally point to how far the rally point is
            {
                Target = null;
            }
            else if (Stopped == false) {
                State = "Walk";
            }
        }


    }

    public override void Walk()
    {
        //Walk information
        Stopped = false;
        transform.position += new Vector3(MoveSpeed * Time.deltaTime, 0, 0);

        //Debug.Log("Rally at: " + (General.RallyPoint * Team) + "Point at " + (transform.position.x * Team));

        //Checks for whether to change state
        if (Target == null)
        {   //These lines work, so Im seperating them from the stuff that deals with the enemy.
            if (General.RallyPoint * Team < (transform.position.x * Team))
            {
                State = "Wait";
            }
            else if (Physics2D.OverlapCircle(transform.position + new Vector3(FrontStoppingDistance * Team, 0, 0), .1f, MovementBlockers))
            {
                Stopped = true;
                State = "Wait";
                //Debug.Log("Stopping the walk");
            }
        }
        else
        {   //New shit for dealing with an enemy
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


    }

    
    public override void Attack()
    {
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
                }
                else
                {
                    Timer -= Time.deltaTime;
                }
            }
            //else if (Vector3.Distance(transform.position, Target.transform.position) < TriggerRange)
            //{
              //  transform.position += new Vector3(MoveSpeed * Time.deltaTime, 0, 0);    //Not giving them the option to follow regular wait commands
            //}
            else {
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
        //I shouldnt need to adapt this to attack, because it should flip to wait before attacking
        Stopped = false;
        if (Physics2D.OverlapCircle(transform.position + new Vector3(BackCheckDistance * Team, 0, 0), .1f, MovementBlockers))   //Needs to creep back slowly as there is an ally behind it
        {
            transform.position += new Vector3(-MoveSpeed * .7f * Time.deltaTime, 0, 0);
            //Stopped = false;
            State = "Wait";
        }
        else {  //Nothing is behind it so it can back up at full speed
            //Stopped = false;
            transform.position += new Vector3(-MoveSpeed * 1.25f * Time.deltaTime, 0, 0);
          
            if ((General.RallyPoint * Team > (transform.position.x * Team)) && !Physics2D.OverlapCircle(transform.position + new Vector3(FrontCheckDistance * Team, 0, 0), .1f, MovementBlockers)) {
                Stopped = true;
                State = "Wait";                
            }

        }
        //Retreat information
        
        //Checks for whether to change state
        
    }
    */


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Unit") || collision.gameObject.CompareTag("Building") || collision.gameObject.CompareTag("Gate")  )
        {
            Damageable thing = collision.GetComponent<Damageable>();
            if (thing.Team != Team)
            {
                //Debug.Log("Viable Target found");
                if (Target == null)
                {
                    Target = thing;
                }
                else if (Vector3.Distance(transform.position, thing.transform.position) < Vector3.Distance(transform.position, Target.transform.position)) {
                    Target = thing;
                }
                State = "Attack";

            }
            //else
            //{
            //    State = "Wait";
            //}
        }
    }

}
