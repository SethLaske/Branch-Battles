using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int damage;
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject other = collider.gameObject;
        Unit unit = other.GetComponent<Unit>();
        if (unit != null)
        {
            unit.TakeDamage(damage); 
        }
    }
}
