using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General : Unit
{
    public LevelManager levelmanager;
    public BattleUI battleUI;
    //public float baseSpeed = 4;
    //public WeaponAttack Offense;
    //public bool Attacking = true;
    private bool waiting = true;
    
    private float RegenTime = .25f;
    private float RegenTimer = 0;

    private Soldier SelectedSoldier;
    public GameObject HealingAura;
    public GameObject troopselector;

    private float baseHP;
    private float baseArmor;
    private float baseDamage;

    void Start()
    {
        maxHealth = HP;
        baseHP = HP;
        baseArmor = Armor;
        baseDamage = Damage;
        currentSpeed = baseSpeed;
        //MoveSpeed = baseSpeed;
        
    }

    // Update is called once per frame
    void Update()
    {
        //Health bar clearing
        if (HealthTimer < AppearanceTime)
        {
            HealthTimer += Time.deltaTime;
        }
        else if (HealthTimer > AppearanceTime)
        {
            HealthBar.SetActive(false);
            HealthTimer = AppearanceTime; //Stops the timer from continuing to add
        }


        if (Attacking) return;      //Gonna just turn off movement while attacking for a bit, see how I like it

        waiting = true;
        //Movement Controls
        if (Input.GetKey(KeyCode.A))    //|| Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
        {
            
            Move(new Vector2(currentSpeed * -1 * Time.deltaTime, 0));
            transform.rotation = Quaternion.Euler(0, 180, 0);
            waiting = false;
        }
        if (Input.GetKey(KeyCode.D))    //|| Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
        {
            Move(new Vector2(currentSpeed * 1 * Time.deltaTime, 0));
            transform.rotation = Quaternion.Euler(0, 0, 0);
            waiting = false;
        }

        if (Input.GetKey(KeyCode.W))    //|| Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
        {
            Move(new Vector2(0, currentSpeed * 1 * Time.deltaTime));
            waiting = false;
        }
        if (Input.GetKey(KeyCode.S))    //|| Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
        {
            Move(new Vector2(0, currentSpeed * -1 * Time.deltaTime));
            waiting = false;
        }

        //Taunt
        if (Input.GetKeyDown(KeyCode.T))
        {
            animator.SetTrigger("Taunt");
            
        }

        //Attack Controls
        if (Input.GetKey("space") && Attacking == false)
        {
            animator.SetBool("Attacking", true);
            Attacking = true;
            //currentSpeed = 2;
            StartCoroutine(PlayAttack());
            //animator.SetBool("HoldSlice", true);
        }

        if (Input.GetKeyDown(KeyCode.R))    
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 2);
            foreach (Collider2D collider in colliders)
            {
                //Debug.Log("The generals collider is overlapping with " + collider.gameObject.name);
                Miner miner = collider.GetComponent<Miner>();
                if (miner != null)
                {
                    miner.changeResource();
                }
            }
        }

        if (Input.GetKey(KeyCode.C)) {
            OneUnitCharge();
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            troopselector.SetActive(false);
            SelectedSoldier?.ReceiveGeneralOrders();    
        }
       

        if (waiting == true && Attacking == false)
        {
            animator.SetBool("Waiting", true);
        }
        else {
            animator.SetBool("Waiting", false);
        }

    }

    //Ends the level and stops the enemy from spawning more units
    public override void Die()
    {
        base.Die();
        levelmanager.GameOver(General.Team);

    }

    IEnumerator PlayAttack()   //Might need recover to deal with animations, otherwise easy fix to remove it
    {
        yield return new WaitForSeconds(attackHitTime);
        attackSound.Play();
        Offense.Attack();
        yield return new WaitForSeconds(attackAnimation.length - attackHitTime);
        animator.SetBool("Attacking", false);
        Attacking = false;
        //currentSpeed = baseSpeed;
    }

    public void OneUnitCharge() {
        SelectedSoldier = null;
        Vector3 TargetedPoint = transform.position + (transform.rotation * (Vector3.right * 2));
        //Debug.Log("The Targeted point is: " + TargetedPoint);
        float ClosestDistance = 100;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(TargetedPoint, .75f);
        foreach (Collider2D collider in colliders)
        {
            Soldier soldier = collider.GetComponent<Soldier>();
            if (soldier != null)
            {
                float UnitDistance = (soldier.transform.position - TargetedPoint).magnitude;
                if (UnitDistance < ClosestDistance) {
                    ClosestDistance = UnitDistance;
                    SelectedSoldier = soldier;
                }
            }

        }
        if (SelectedSoldier == null) {
            troopselector.SetActive(false);
            return;
        }
        troopselector.transform.position = SelectedSoldier.transform.position;
        troopselector.SetActive(true);
    }
    //Heals while in security
    private void OnTriggerStay2D(Collider2D collider)
    {
        
        if (collider.gameObject.CompareTag("PlayerCamp"))
        {

            if (maxHealth > HP)
            {
                HealingAura.SetActive(true);
                //Debug.Log("Healing");
                if (RegenTimer < RegenTime)
                {
                    RegenTimer += Time.deltaTime;
                }
                else
                {
                    RegenTimer = 0;
                    HP++;
                    HealthBar.transform.localScale = new Vector3(HP / maxHealth, HealthBar.transform.localScale.y, HealthBar.transform.localScale.z);
                    HealthBar.SetActive(true);
                    HealthTimer = 0;
                }
            }
            else {
                HealingAura.SetActive(false);
            }
            
        }
    }

    //Only allows troop training while in the security of the player camp
    private void OnTriggerEnter2D(Collider2D collider)
    {
        
        if (collider.gameObject.CompareTag("PlayerCamp"))
        {
            Damage += 3;
            baseDamage += 3;
            //levelmanager.BattleUIUnits.SetActive(true);
            battleUI.EnableTroops();
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        
        if (collider.gameObject.CompareTag("PlayerCamp"))
        {
            Damage -= 3;
            baseDamage -= 3;
            //levelmanager.BattleUIUnits.SetActive(false);
            battleUI.DisableTroops();
        }
    }


    public void CalculateArmyBuffs() {
        //HP is increased by class 0 count- 1 unit = +5% HP
        float oldMaxHP = maxHealth;
        maxHealth = baseHP + (General.troopCategory[0] * .05f * baseHP);
        HP = HP * maxHealth / oldMaxHP;

        //Armor is increased by class 1 count- 3units = +1 Armor
        Armor = baseArmor + (General.troopCategory[1]/3);
        //Damage is increased by class 2 count- 1 unit = +1 Damage
        Damage = baseDamage + General.troopCategory[2];

        //Speed is increased by class 3 count- 2 units = +1 speed
        currentSpeed = baseSpeed + (General.troopCategory[3] / 2);
    }
}
