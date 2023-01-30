using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackTrigger : MonoBehaviour
{
    public Soldier soldier;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Unit"))         //|| collision.gameObject.CompareTag("Building"))
        {

            int team = collision.GetComponent<Unit>().Team;
            if (team == soldier.Team)
            {
                //soldier.BackComrade = collision.gameObject.GetComponent<Unit>();
                //soldier.BackComrade.State = "Wait";
                Debug.Log("Setting back soldier to wait");
            }
        }
    }
}
