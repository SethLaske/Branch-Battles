using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Enemys equivalent of a general. Might make them reverse and become stronger as units around them die
public class Commander : Soldier
{
    void Start()
    {
        StandardStart();

        ReceiveGeneralOrders();
        
    }

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
            HealthTimer = AppearanceTime; 
        }

        
        FindTarget();
        animator.SetBool("Walking", true);
        if (State == "Charge")    
        {
            
            Charge();
        }
    }

    public override void Walk()
    {


        if (Target != null && (Mathf.Abs(transform.position.x - Target.transform.position.x) < AttackRange))   //or there is a target, but its beyond the agro range
        {
            State = "Attack";
        }
        else
        {
            transform.position += new Vector3(currentSpeed * Time.deltaTime, 0, 0);
            
            
        }
       


    }

    //I think we only want to be in attack if we are actively attacking, otherwise it will be sent to a different state
    public override void Attack()
    {


        if (Target != null)
        {

            if (Mathf.Abs(transform.position.x - Target.transform.position.x) < AttackRange)
            {
                
            }

            else
            {
                State = "Walk";
            }
        }
        else
        {
            State = "Walk";
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

        Damageable thing = collision.GetComponent<Damageable>();
        if (thing != null)    //Need to change this to a damageable check
        {
            if (thing.Team != Team)
            {
                if (Target == null) //No target around found
                {
                    Target = thing;
                }
                else if (Vector3.Distance(transform.position, thing.transform.position) < Vector3.Distance(transform.position, Target.transform.position))
                {
                    Target = thing;
                }

            }
            else
            {
                
            }

            
        }
    }

    public override void Die()
    {
        base.Die();
        General.barracks.Die();
    }
}
