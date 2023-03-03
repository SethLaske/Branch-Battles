using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetection : MonoBehaviour
{
    
    public Unit Wielder;


    private void Start()
    {
        Wielder = gameObject.GetComponentInParent<Unit>();
        
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
