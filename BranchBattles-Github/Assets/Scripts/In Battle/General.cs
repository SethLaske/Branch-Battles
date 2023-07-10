using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General : Unit
{
    public LevelManager levelmanager;
    public BattleUI battleUI;
    public float baseSpeed = 4;
    //public WeaponAttack Offense;
    //public bool Attacking = true;
    public bool waiting = true;
    
    public float RegenTime = 1;
    public float RegenTimer = 0;

    private Vector3 NewPosition;

    public Soldier SelectedSoldier;
    public GameObject HealingAura;
    void Start()
    {
        maxHealth = HP;
        AttackTimer = 0;
        MoveSpeed = baseSpeed;
        General.generalSpeed = baseSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        waiting = true;
        //Movement Controls
        if (Input.GetKey(KeyCode.A))    //|| Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
        {
            
            Move(new Vector2(MoveSpeed * -1 * Time.deltaTime, 0));
            transform.rotation = Quaternion.Euler(0, 180, 0);
            /*NewPosition = transform.position + (MoveSpeed * Vector3.left * Time.deltaTime);   saving the original just in case
            if (Physics2D.OverlapCircle(NewPosition, .2f, MovementBlockers))
            {
                Debug.Log("Moving left from " + transform.position + " to " + NewPosition);
                transform.position = NewPosition;
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }*/
            waiting = false;
        }
        if (Input.GetKey(KeyCode.D))    //|| Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
        {
            Move(new Vector2(MoveSpeed * 1 * Time.deltaTime, 0));
            transform.rotation = Quaternion.Euler(0, 0, 0);
            waiting = false;
        }

        if (Input.GetKey(KeyCode.W))    //|| Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
        {
            Move(new Vector2(0, MoveSpeed * 1 * Time.deltaTime));
            waiting = false;
        }
        if (Input.GetKey(KeyCode.S))    //|| Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
        {
            Move(new Vector2(0, MoveSpeed * -1 * Time.deltaTime));
            waiting = false;
        }

        //Attack Controls
        if (Input.GetKeyDown("space") && AttackTimer == 0f)
        {
            animator.SetBool("Attacking", true);
            AttackTimer = 0.01f;
            MoveSpeed = 2;
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
            if (SelectedSoldier != null) {
                SelectedSoldier.State = "GeneralCharge";
                SelectedSoldier.animator.SetBool("Waiting", false);
            }
                
        }
        //SelectedSoldier = null;
        //People want to spam the button, so I guess Ill just allow it
        /*if (Input.GetKeyUp("space") && !Attacking)
        {
            MoveSpeed *= 2;
            AttackTimer = 0;
            animator.SetBool("Attacking", false);
            //animator.SetBool("HoldSlice", false);
        }*/
        if (AttackTimer > 0)
        {
            waiting = false;
            AttackTimer += Time.deltaTime;
            if (AttackTimer >= AttackCooldown && !Attacking)
            {
                Offense.Attack();
                attackSound.Play();
                Attacking = true;
            }
            if (AttackTimer >= AttackCooldown * 1.25)
            {
                //animator.SetBool("HoldSlice", false);
                animator.SetBool("Attacking", false);
                Attacking = false;
                AttackTimer = 0f;
                MoveSpeed = baseSpeed;
                if (Input.GetKey("space"))
                {
                    animator.SetBool("Attacking", true);
                    AttackTimer = 0.01f;
                    MoveSpeed = 2;
                }
            }
        }
        else {
            MoveSpeed = baseSpeed;
        }

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

        if (waiting == true)
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
        General.Opponent.TroopMax = 0;
        General.Opponent.SpawnUnits.Clear();
        levelmanager.GameOver(General.Team);

    }

    public void OneUnitCharge() {
        SelectedSoldier = null;
        Vector3 TargetedPoint = transform.position + (transform.rotation * (Vector3.right * 2));
        Debug.Log("The Targeted point is: " + TargetedPoint);
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
            Offense.Damage += 3;
            //levelmanager.BattleUIUnits.SetActive(true);
            battleUI.EnableTroops();
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        
        if (collider.gameObject.CompareTag("PlayerCamp"))
        {
            Damage -= 3;
            Offense.Damage -= 3;
            //levelmanager.BattleUIUnits.SetActive(false);
            battleUI.DisableTroops();
        }
    }

}
