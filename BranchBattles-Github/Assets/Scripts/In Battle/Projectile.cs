using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    //Speed might be redundant since the projectileattack provides it with velocity
    public float Speed;
    public float Damage;
    public float LifeTime;

    public int Team;

    void Update(){
        //transform.position += transform.rotation * new Vector3(Speed,0 , 0) * Time.deltaTime;     moves but sometimes too fast
        LifeTime -= Time.deltaTime;
        if (LifeTime <= 0) {
            Destroy(gameObject);
        }
        Debug.Log(GetComponent<Rigidbody2D>().velocity);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject other = collider.gameObject;
        Damageable uc = other.GetComponent<Damageable>();
        if (uc != null && uc.Team != Team)
        {
            uc.TakeDamage(Damage);
            Destroy(gameObject);
        }
    }
}