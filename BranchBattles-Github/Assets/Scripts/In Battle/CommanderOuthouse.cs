using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommanderOuthouse : Building
{
    public Commander resident;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Die()
    {
        base.Die();
        resident.enabled = true;
        resident.gameObject.SetActive(true);
        General.maxTroopCount = 0;
    }
}
