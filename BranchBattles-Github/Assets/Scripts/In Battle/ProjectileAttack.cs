using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttack : WeaponAttack
{

    public GameObject Projectile;
    public float Speed;
    public float LifeTime;

    // Start is called before the first frame update
    void Start()
    {
        //AttackBox = GetComponent<Collider2D>();

        Wielder = gameObject.GetComponentInParent<Unit>();
        Damage = Wielder.Damage;


    }

    public override void Attack()
    {
        GameObject FiredProjectileObject = Instantiate(Projectile, transform.position + transform.rotation * new Vector3(0.1f, 0, 0), transform.rotation);
        Projectile FiredProjectile = FiredProjectileObject.GetComponent<Projectile>();
        Collider2D ProjectileHitbox = FiredProjectileObject.GetComponent<Collider2D>();
        Rigidbody2D rb = FiredProjectileObject.GetComponent<Rigidbody2D>();
        FiredProjectile.Speed = Speed;
        FiredProjectile.Damage = Damage;
        FiredProjectile.LifeTime = LifeTime;
        FiredProjectile.Team = Wielder.Team;
        ProjectileHitbox.enabled = true;

        rb.velocity = transform.rotation * new Vector3(Speed,0,0);
        Debug.Log(transform.rotation);
        Debug.Log(rb.velocity);
    }

    
}

