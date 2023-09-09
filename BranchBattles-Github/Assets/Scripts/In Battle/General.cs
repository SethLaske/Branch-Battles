using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General : Unit
{
    private LevelManager levelmanager;
    private BattleUI battleUI;
    
    private bool walking = false;
    private bool taunting; 
    
    public float regenTime = .25f;
    private float regenTimer = 0;

    private Soldier SelectedSoldier;
    public GameObject HealingAura;
    public GameObject troopselector;

    public GameObject goldBar;
    public float goldBarAppearanceTime;
    private float goldBarTimer;
    public Transform minePoint;
    public int goldPerHit;
    public int maxGoldAmount;
    public float miningIncrement;
    private int currentGoldAmount;
    public AudioSource goldDropOff;

    private float baseHP;
    private float baseArmor;
    private float baseDamage;

    void Awake()
    {
        maxHealth = HP;
        baseHP = HP;
        baseArmor = Armor;
        baseDamage = Damage;
        currentSpeed = baseSpeed;
        taunting = false;
        HealthBar.SetActive(false);
        healthBarStartScale = HealthBar.transform.localScale.x;
    }

    void Update()
    {
        if (LevelManager.gameState != GameState.InGame)
        {
            return;
        }

        if (HP >= maxHealth)
        {
            HealthBar.SetActive(false);
        }
        else {
            HealthBar.SetActive(true);
        }

        if (goldBar.activeSelf)
        {
            goldBarTimer += Time.deltaTime;
            if (goldBarTimer > goldBarAppearanceTime)
            {
                goldBar.SetActive(false);
            }
        }
        

        if (Attacking || taunting) return;      //Gonna just turn off movement while attacking for a bit, see how I like it


        if (Input.GetKey("space") && Attacking == false)
        {
            animator.SetBool("Attacking", true);
            Attacking = true;
            //currentSpeed = 2;
            StartCoroutine(PlayAttack());
            
            return;
        }

       

        //Taunt
        if (Input.GetKeyDown(KeyCode.T))
        {
            //this.Stun(10, 2);
            animator.SetTrigger("Taunt");
            taunting = true;
            return;
        }


       

        if (Input.GetKey(KeyCode.C))
        {
            OneUnitCharge();
        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            troopselector.SetActive(false);
            SelectedSoldier?.ReceiveGeneralOrders();
        }

        walking = false;
        //Movement Controls
        if (Input.GetKey(KeyCode.A))   
        {
            Move(new Vector2(currentSpeed * -1 * Time.deltaTime, 0));
            transform.localScale = new Vector3(-1, 1, 1);
            walking = true;
        }
        if (Input.GetKey(KeyCode.D))    
        {
            Move(new Vector2(currentSpeed * 1 * Time.deltaTime, 0));
            transform.localScale = new Vector3(1, 1, 1);
            walking = true;
        }

        if (Input.GetKey(KeyCode.W))  
        {
            Move(new Vector2(0, currentSpeed * 1 * Time.deltaTime));
            walking = true;
        }
        if (Input.GetKey(KeyCode.S))  
        {
            Move(new Vector2(0, currentSpeed * -1 * Time.deltaTime));
            walking = true;
        }

        animator.SetBool("Walking", walking);
    }

    //Ends the level and stops the enemy from spawning more units
    public override void Die()
    {
        base.Die();
        levelmanager.GameOver(General.Team);

    }

    IEnumerator PlayAttack()   //Might need recover to deal with animations, otherwise easy fix to remove it
    {
        yield return new WaitForSeconds(attackHitTime * DebuffMult);

        attackSound.Play();
        Offense.Attack();
        GeneralMine();

        yield return new WaitForSeconds((attackAnimation.length - attackHitTime) * DebuffMult);
        if (Input.GetKey("space") == false)
        {
            animator.SetBool("Attacking", false);
            Attacking = false;
        }
        else {
            StartCoroutine(PlayAttack());
        }
        
        //currentSpeed = baseSpeed;
    }

    /*IEnumerator GeneralMine()   //Might need recover to deal with animations, otherwise easy fix to remove it
    {
        yield return new WaitForSeconds(attackHitTime * DebuffMult);

        Ray2D ray = new Ray2D(minePoint.position, Vector2.right * transform.localScale.x);

        // Perform the raycast and store the hit information in a RaycastHit2D variable.
        RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction, 2);

        // Check if the raycast hit a collider.
        foreach (RaycastHit2D hit in hits)
        {
            Debug.Log("hit this mining: " + hit.collider.gameObject.name);
            Mine mine = hit.collider.gameObject.GetComponent<Mine>();
            if(mine != null){
                Debug.Log("Made contact with mine");

                if (currentGoldAmount != maxGoldAmount) {
                    goldBar.SetActive(true);
                    goldBarTimer = 0;
                }
                currentGoldAmount += goldPerHit;
                if (currentGoldAmount > maxGoldAmount) currentGoldAmount = maxGoldAmount;

                mine.IncreaseMultiplier(miningIncrement);
                goldBar.transform.localScale = new Vector3((float)currentGoldAmount / maxGoldAmount, goldBar.transform.localScale.y, goldBar.transform.localScale.z);

            }
        }
        yield return new WaitForSeconds((attackAnimation.length - attackHitTime) * DebuffMult);

        if (Input.GetKey("space") == false)
        {
            animator.SetBool("Attacking", false);
            Attacking = false;
        }
        else
        {
            StartCoroutine(PlayAttack());
        }

        //currentSpeed = baseSpeed;
    }*/

    private void GeneralMine() {
        Ray2D ray = new Ray2D(minePoint.position, Vector2.right * transform.localScale.x);

        // Perform the raycast and store the hit information in a RaycastHit2D variable.
        RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction, 2);

        // Check if the raycast hit a collider.
        foreach (RaycastHit2D hit in hits)
        {
            //Debug.Log("hit this mining: " + hit.collider.gameObject.name);
            Mine mine = hit.collider.gameObject.GetComponent<Mine>();
            if (mine != null)
            {
                //Debug.Log("Made contact with mine");

                if (currentGoldAmount != maxGoldAmount)
                {
                    goldBar.SetActive(true);
                    goldBarTimer = 0;
                }
                currentGoldAmount += goldPerHit;
                if (currentGoldAmount > maxGoldAmount) currentGoldAmount = maxGoldAmount;

                mine.IncreaseMultiplier(miningIncrement);
                mine.MineHit(transform.position);
                goldBar.transform.localScale = new Vector3((float)currentGoldAmount / maxGoldAmount, goldBar.transform.localScale.y, goldBar.transform.localScale.z);

            }
        }
    }

    public void EndTaunt() {
        taunting = false;
    }
    public void OneUnitCharge() {
        SelectedSoldier = null;
        Vector3 TargetedPoint = transform.position + (transform.localScale.x * (Vector3.right * 2));
        //Debug.Log("The Targeted point is: " + TargetedPoint);
        float ClosestDistance = 100;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(TargetedPoint, .75f);
        foreach (Collider2D collider in colliders)
        {
            Soldier soldier = collider.GetComponent<Soldier>();
            if (soldier != null  && soldier.Team == Team)
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
                if (regenTimer < regenTime)
                {
                    regenTimer += Time.deltaTime;
                }
                else
                {
                    regenTimer = 0;
                    HP++;
                    HealthBar.transform.localScale = new Vector3(HP / maxHealth, HealthBar.transform.localScale.y, HealthBar.transform.localScale.z);
                    //HealthBar.SetActive(true);
                    //HealthTimer = 0;
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

        TeamBase teamBase = collider.GetComponent<TeamBase>();
        if(teamBase != null && teamBase.Team == Team)
        {
            if (currentGoldAmount > 0) goldDropOff.Play();
            General.gold += currentGoldAmount;
            currentGoldAmount = 0;
            
            //Audio source and updating any UI
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
            HealingAura.SetActive(false);
        }
    }


    public void CalculateArmyBuffs() {
        if (maxHealth < 5) {
            return;
        }

        //HP is increased by class 0 count- 1 unit = +5% HP
        float oldMaxHP = maxHealth;
        maxHealth = baseHP + (General.troopCategory[0] * .15f * baseHP);
        HP = HP * maxHealth / oldMaxHP;

        //Armor is increased by class 1 count- 3units = +1 Armor
        Armor = baseArmor + (General.troopCategory[1]/3);
        
        //Damage is increased by class 2 count- 1 unit = +1 Damage
        Damage = baseDamage + Mathf.Min(General.troopCategory[2]/2, 5);

        //Speed is increased by class 3 count- 2 units = +1 speed
        currentSpeed = baseSpeed + Mathf.Min((General.troopCategory[3] / 3), 3.5f);
    }

    public void SetLevelManager(LevelManager levelManager) {
        this.levelmanager = levelManager;
    }
    public void SetBattleUI(BattleUI ui)
    {
        battleUI = ui;
    }
}
