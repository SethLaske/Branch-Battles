using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pacifist : Unit
{
    public bool Full = false;
    public int BagAmount;

    // Start is called before the first frame update
    void Start()
    {
        Timer = AttackCooldown;
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
        if (collision.gameObject.CompareTag("Mine"))         //|| collision.gameObject.CompareTag("Building"))
        {
            State = "Mining";   //Being in the mining State should mean that it does nothing
            StartCoroutine(IMine());
        }

        if (collision.gameObject.CompareTag("Building"))         //|| collision.gameObject.CompareTag("Building"))
        {
            if (Full == true) {
                General.Gold += BagAmount;
                Full = false;
            }
            State = "Walk";
        }
    }

}
