using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetection : MonoBehaviour
{
    
    private Unit Wielder;


    private void Start()
    {
        Wielder = gameObject.GetComponentInParent<Unit>();
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Soldier soldier = collision.GetComponent<Soldier>();
        if (soldier != null) //It is a soldier and this soldier wants to stay behind it
        {
            if (CheckShieldConditions(soldier) == true)
            {
                Wielder.humanshield = soldier;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Soldier soldier = collision.GetComponent<Soldier>();
        if (soldier != null) //It is a soldier and this soldier wants to stay behind it
        {
            if (Wielder.humanshield == soldier)
            {
                Wielder.humanshield = null;
            }
        }
        //Wielder.GetComponent<Soldier>().PulseUpdate();
    }

    public bool CheckShieldConditions(Soldier soldier)
    {
        if (soldier == null)
        {  //This better be redundant
            return false;
        }
        if (soldier.unitClassification >= Wielder.unitClassification) //Something we actually want to stay behind
        {
            return false;
        }
        if (Wielder.humanshield == null)    //Is the shield currently empty
        {
            return true;
        }
        //Now there are some decisions
        //Should it fall behind the closest in class, for now I will say yes
        if (Wielder.humanshield.unitClassification > soldier.unitClassification)
        {
            return false;
        }
        //Will we follow the closest? I say yes
        if (Vector3.Distance(transform.position, Wielder.humanshield.transform.position) < Vector3.Distance(transform.position, soldier.transform.position))
        {
            return false;
        }

        return true;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {

        Damageable thing = collision.GetComponent<Damageable>();
        if (thing != null)    //Need to change this to a damageable check
        {
            if (thing.Team != Wielder.Team)
            {
                //Debug.Log("Viable Target found");
                if (Wielder.Target == null) //No target around found
                {
                    //Debug.Log("Adding Target");
                    Wielder.Target = thing;
                }
                else if (Vector3.Distance(transform.position, thing.transform.position) < Vector3.Distance(transform.position, Wielder.Target.transform.position))
                {
                    Wielder.Target = thing;
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
}
