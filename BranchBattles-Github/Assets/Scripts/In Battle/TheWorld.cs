using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheWorld : MonoBehaviour
{
    public AudioSource magicSound;
    public float TimeStopTime;
    public float abilityRadius;
    private GameObject[] Crusaders;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StopTime());
        Crusaders = GameObject.FindGameObjectsWithTag("Unit");
        foreach (GameObject crusader in Crusaders) {
            Unit enemyStand = crusader.GetComponent<Unit>();
            if (enemyStand != null && Vector3.Distance(this.transform.position, enemyStand.transform.position) > abilityRadius) {
                //enemyStand.Stun(TimeStopTime, 1000);
                Debug.Log("REMOVED STUN");
                enemyStand.Die();
            }
        }
        
    }
    
    IEnumerator StopTime()
    {

        yield return new WaitForSeconds(TimeStopTime);
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, abilityRadius);
    }
}
