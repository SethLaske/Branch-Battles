using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General : Unit
{
    public LevelManager levelmanager;
    //public WeaponAttack Offense;
    //public bool Attacking = true;

    
    public float RegenTime = 1;
    public float RegenTimer = 0;

    private Vector3 NewPosition;

    void Start()
    {
        maxHealth = HP;
        AttackTimer = 0;

    }

    // Update is called once per frame
    void Update()
    {

        //Movement Controls
        if (Input.GetKey(KeyCode.A))    //|| Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
        {
            NewPosition = transform.position + (MoveSpeed * Vector3.left * Time.deltaTime);
            if (Physics2D.OverlapCircle(NewPosition, .2f, MovementBlockers))
            {
                transform.position = NewPosition;
            }
        }
        if (Input.GetKey(KeyCode.D))    //|| Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
        {
            NewPosition = transform.position + (MoveSpeed * Vector3.right * Time.deltaTime);
            if (Physics2D.OverlapCircle(NewPosition, .2f, MovementBlockers))
            {
                transform.position = NewPosition;
            }
        }

        if (Input.GetKey(KeyCode.W))    //|| Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
        {
            NewPosition = transform.position + (MoveSpeed * Vector3.up * Time.deltaTime);
            if (Physics2D.OverlapCircle(NewPosition, .2f, MovementBlockers))
            {
                transform.position = NewPosition;
                //transform.position += 2f * Vector3.forward * Time.deltaTime;
            }
        }
        if (Input.GetKey(KeyCode.S))    //|| Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
        {
            NewPosition = transform.position + (MoveSpeed * Vector3.down * Time.deltaTime);
            if (Physics2D.OverlapCircle(NewPosition, .2f, MovementBlockers))
            {
                transform.position = NewPosition;
                //transform.position += 2f * Vector3.back * Time.deltaTime;
            }
        }

        //Attack Controls
        if (Input.GetKeyDown("space") && AttackTimer == 0f)
        {
            animator.SetBool("Attacking", true);
            AttackTimer = 0.01f;
            //animator.SetBool("HoldSlice", true);
        }
        if (Input.GetKeyUp("space") && !Attacking)
        {
            AttackTimer = 0;
            animator.SetBool("Attacking", false);
            //animator.SetBool("HoldSlice", false);
        }
        if (AttackTimer > 0)
        {
            AttackTimer += Time.deltaTime;
            if (AttackTimer >= AttackCooldown && !Attacking)
            {
                Offense.Attack();
                //slice.Play();
                Attacking = true;
            }
            if (AttackTimer >= AttackCooldown * 1.25)
            {
                //animator.SetBool("HoldSlice", false);
                animator.SetBool("Attacking", false);
                Attacking = false;
                AttackTimer = 0f;
                if (Input.GetKey("space"))
                {
                    animator.SetBool("Attacking", true);
                    AttackTimer = 0.01f;
                }
            }
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

        

    }
    public override void Die()
    {
        base.Die();
        General.Opponent.TroopMax = 0;
        General.Opponent.SpawnUnits.Clear();
        levelmanager.GameOver(General.Team);

    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        
        if (collider.gameObject.CompareTag("PlayerCamp"))
        {
            
            if (maxHealth > HP)
            {
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
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        
        if (collider.gameObject.CompareTag("PlayerCamp"))
        {
            
            levelmanager.BattleUIUnits.SetActive(true);
            
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        
        if (collider.gameObject.CompareTag("PlayerCamp"))
        {
            
            levelmanager.BattleUIUnits.SetActive(false);
            
        }
    }

}
