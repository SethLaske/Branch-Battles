using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttack : MonoBehaviour
{
    //[HideInInspector] public float Damage;
    private Collider2D AttackBox;
    public bool MultiHit;

    private bool FinishedAttack;

    protected Unit Wielder;


    private void Start()
    {
        AttackBox = GetComponent<Collider2D>();
        AttackBox.enabled = false;
        Wielder = gameObject.GetComponentInParent<Unit>();
        //Damage = Wielder.Damage;

    }

    public virtual void Attack()
    {
        //Debug.Log("Starting Attack");
        FinishedAttack = false;
        AttackBox.enabled = true;
        //Wielder.Move(Wielder.transform.rotation * new Vector3(-4 * Time.deltaTime, 0, 0)); //Added kickback, not sure how I feel about it
        StartCoroutine(EndAttack());
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject other = collider.gameObject;
        Damageable uc = other.GetComponent<Damageable>();
        if (uc != null && uc.Team != Wielder.Team && !FinishedAttack)
        {
            uc.TakeDamage(Wielder.Damage);
            if (!MultiHit) {
                //Debug.Log("A single target has been hit so this will stop hitting other things");
                AttackBox.enabled = false;
                FinishedAttack = true;
            }
            //
        }
    }

    IEnumerator EndAttack() {
        yield return new WaitForSeconds(.15f);
        AttackBox.enabled = false;
    }
}
