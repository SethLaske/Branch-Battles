using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Basic form of a soldier, but just stands atop the tower firing
public class TowerSoldier : MonoBehaviour
{
    public int Team;
    public Damageable Target;
    public float Damage;
    public float AttackSpeed;
    public float AttackTimer;
    public float AttackRange;

    void Start()
    {
        CircleCollider2D collider = gameObject.AddComponent<CircleCollider2D>();
        collider.radius = AttackRange - 1;
        //collider.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Target != null) {

            if (AttackTimer < AttackSpeed)
            {
                AttackTimer += Time.deltaTime;
            }
            else {
                Target.TakeDamage(Damage);
                AttackTimer = 0;
            }

            if (Vector3.Distance(transform.position, Target.transform.position) > AttackRange)
            {
                Target = null;
            }

        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("something entered tower range");
        Damageable thing = collision.GetComponent<Damageable>();
        if (thing != null)    //Need to change this to a damageable check
        {
            Debug.Log("Its damageable");
            
            if (thing.Team != Team)
            {
                Debug.Log("Viable Target found");
                if (Target == null) //No target around found
                {
                    //Debug.Log("Adding Target");
                    Target = thing;
                }
                else if (Vector3.Distance(transform.position, thing.transform.position) < Vector3.Distance(transform.position, Target.transform.position))
                {
                    Target = thing;
                }
                

            }
            
        }
    }
}
