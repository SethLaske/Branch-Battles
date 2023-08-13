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
        


    }

    public override void Attack()
    {
        GameObject FiredProjectileObject = Instantiate(Projectile, transform.position, Quaternion.identity);
        Projectile FiredProjectile = FiredProjectileObject.GetComponent<Projectile>();
        Collider2D ProjectileHitbox = FiredProjectileObject.GetComponent<Collider2D>();
        Rigidbody2D rb = FiredProjectileObject.GetComponent<Rigidbody2D>();
        FiredProjectile.Speed = Speed;
        FiredProjectile.Damage = Wielder.Damage;
        FiredProjectile.LifeTime = LifeTime;
        FiredProjectile.Team = Wielder.Team;
        FiredProjectile.transform.localScale = Wielder.transform.localScale;
        ProjectileHitbox.enabled = true;

        rb.velocity = new Vector3(Speed * Wielder.transform.localScale.x,0,0);
        //Debug.Log("Initial Velocity: " + rb.velocity);
    }

    
}

