using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Gates allow for players to split their own units, and create buffers from the enemy
public class Gate : Building
{
    public GameObject GateDoor; //
    public LayerMask MovementBlockers;
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public void gateSelected() {
        
        if (!Physics2D.OverlapCircle(transform.position, .75f, MovementBlockers)) { //.75 is half the width, so units shouldnt get trapped inside
            GateDoor.SetActive(!GateDoor.activeInHierarchy);    //Completely switches the gate door on and off. Enemies will attack the gate whether its open or not
                                                                //This might need to get changed to allow for enemy trapping, but will work fine, at least until redesign
        }
            
    }
}
