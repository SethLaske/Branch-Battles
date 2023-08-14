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
    
    // Start is called before the first frame update
    void Start()
    {
        
        StartCoroutine(LightningStrike());
    }

    IEnumerator LightningStrike() {
        yield return new WaitForSeconds(timeToStrike);

        Ray2D ray = new Ray2D(transform.position, Vector2.down);

        // Perform the raycast and store the hit information in a RaycastHit2D variable.
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, lightningDistance, layerToDamage);

        // Check if the raycast hit a collider.
        if (hit.collider != null)
        {
            //Debug.Log("hit this: " + hit.collider.gameObject.name);
            Unit hitUnit = hit.collider.gameObject.GetComponent<Unit>();
            // Check if the hit GameObject has the tag "Unit".
            if (hitUnit != null)
            {
                hitUnit.Stun(lightningStunTime, lightningStunIntensity);
                hitUnit.TakeDamage(lightningDamage);
            }
        }

        //Perhaps wait a duration for the animation to finish
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - lightningDistance, transform.position.z));
    }
}
