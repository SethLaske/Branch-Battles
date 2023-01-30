using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Pacifists wont obey the rally or charge commands, and will perform their own tasks. I might allow them to fight if attacked, but undecided (currently they wont)
//This should be the super class to all pacifists, but currently just directly handles the miner
public class Pacifist : Unit
{
    public bool Full = false;
    public int BagAmount;

    // Start is called before the first frame update
    void Start()
    {
        AttackTimer = AttackCooldown;
        State = "Walk";
    }

    // Update is called once per frame
    void Update()
    {
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


    IEnumerator IMine()
    {
        
        //Debug.Log("Time Starting");
        yield return new WaitForSeconds(2);

        
        Full = true;
        State = "Retreat";

        
        //Debug.Log("Time ending");
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Mine"))        
        {
            State = "Mining";   //Being in the mining State should mean that it does nothing
            StartCoroutine(IMine());
        }

        if (collision.gameObject.CompareTag("Building"))         //Will be compared to barracks/team base eventually
        {
            if (Full == true) {
                General.Gold += BagAmount;
                Full = false;
            }
            State = "Walk";
        }
    }

}
