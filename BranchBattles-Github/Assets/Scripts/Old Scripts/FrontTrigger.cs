using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontTrigger : MonoBehaviour
{
    public Soldier soldier;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Unit"))         //|| collision.gameObject.CompareTag("Building"))
        {
            
            int team = collision.GetComponent<Unit>().Team;
            if (team == soldier.Team)
            {
                //soldier.FrontComrade = collision.gameObject.GetComponent<Unit>();

            }
        }
    }
}
