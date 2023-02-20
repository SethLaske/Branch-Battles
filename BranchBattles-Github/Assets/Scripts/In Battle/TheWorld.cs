using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheWorld : Magic
{
    public float TimeStopTime;
    public GameObject[] Crusaders;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(transform.position.x, -1.1f, 0);
        StartCoroutine(StopTime());
        Crusaders = GameObject.FindGameObjectsWithTag("Unit");
        foreach (GameObject crusader in Crusaders) {
            Unit enemyStand = crusader.GetComponent<Unit>();
            if (enemyStand != null && Vector3.Distance(this.transform.position, enemyStand.transform.position) > 4) {
                enemyStand.Stun(TimeStopTime, 1000);
                //enemyStand.Die();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }



    
    IEnumerator StopTime()
    {

        yield return new WaitForSeconds(TimeStopTime);
        Destroy(gameObject);
    }
}
