using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttack : MonoBehaviour
{
    public float Damage;
    public Collider2D AttackBox;
    public bool MultiHit;

    public bool FinishedAttack;

    public Unit Wielder;


    private void Start()
    {
        AttackBox = GetComponent<Collider2D>();

        Wielder = gameObject.GetComponentInParent<Unit>();
        Damage = Wielder.Damage;

    }

    public virtual void Attack()
    {
        Debug.Log("Starting Attack");
        FinishedAttack = false;
        AttackBox.enabled = true;
        Wielder.Move(Wielder.transform.rotation * new Vector3(-1 * Damage * Time.deltaTime, 0, 0)); //Added kickback, not sure how I feel about it
        StartCoroutine(EndAttack());
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject other = collider.gameObject;
        Damageable uc = other.GetComponent<Damageable>();
        if (uc != null && uc.Team != Wielder.Team && !FinishedAttack)
        {
            uc.TakeDamage(Damage);
            if (!MultiHit) {
                Debug.Log("A single target has been hit so this will stop hitting other things");
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
