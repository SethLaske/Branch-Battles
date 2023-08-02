using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Enemys equivalent of a general. Might make them reverse and become stronger as units around them die
public class Commander : Soldier
{
    // Start is called before the first frame update
    void Start()
    {
        transform.Rotate(new Vector3(0, 180, 0));

        maxHealth = HP;
        
        State = "Walk";
        if (Team < 0)
        {
            HealthBar.GetComponent<SpriteRenderer>().color = Color.red;
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

        //Checks if anything is ahead or behind it

        //The one can likely be replaced with larger/smaller units sizes to do stuff like that
        //Attack Range must be greater than this check distance
        //Debug.Log("Coords to check ahead are: " + (transform.position + new Vector3(.9f * Team, 0, 0)));
        

        

        //Performs the various actions per the state it is in
        if (State == "Wait")
        {
            Wait();
            animator.SetBool("Attacking", false);
        }
        else if (State == "Walk")
        {
            Walk();
            animator.SetBool("Attacking", false);
        }
        else if (State == "Attack")
        {
            Attack();
            animator.SetBool("Attacking", true);
        }
    }

    public override void Walk()
    {


        if (Target != null && (Mathf.Abs(transform.position.x - Target.transform.position.x) < AttackRange))   //or there is a target, but its beyond the agro range
        {
            State = "Attack";
            //Debug.Log("Wait > Attack");
        }
        else
        {
            transform.position += new Vector3(MoveSpeed * Time.deltaTime, 0, 0);
            //fix backup and overshooting
            
        }
       


    }

    //I think we only want to be in attack if we are actively attacking, otherwise it will be sent to a different state
    public override void Attack()
    {


        if (Target != null)
        {

            if (Mathf.Abs(transform.position.x - Target.transform.position.x) < AttackRange)
            {
                //Not gonna deal with this right now
            }

            else
            {
                State = "Walk";
                //Debug.Log("Attack -> Walk");
            }
        }
        else
        {
            State = "Walk";
            //Debug.Log("Attack -> Wait");
        }
    }

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
                //State = "Attack";

            }
            else
            {
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

    public override void Die()
    {
        base.Die();
        General.barracks.Die();
    }
}
