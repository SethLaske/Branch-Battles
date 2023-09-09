using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBolt : MonoBehaviour
{
    public LayerMask layerToDamage;
    public float lightningDistance;
    public float timeToStrike;
    public float lightningDamage;

    public float lightningStunTime;
    public float lightningStunIntensity;
    
    void Start()
    {
        
        StartCoroutine(LightningStrike());
    }

    IEnumerator LightningStrike() {
        yield return new WaitForSeconds(timeToStrike);

        Ray2D ray = new Ray2D(transform.position, Vector2.down);

        
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, lightningDistance, layerToDamage);

        
        if (hit.collider != null)
        {
            
            Unit hitUnit = hit.collider.gameObject.GetComponent<Unit>();
            
            if (hitUnit != null)
            {
                hitUnit.Stun(lightningStunTime, lightningStunIntensity);
                hitUnit.TakeDamage(lightningDamage);
            }
        }

        
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - lightningDistance, transform.position.z));
    }
}
