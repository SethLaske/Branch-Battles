using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Pacifists wont obey the rally or charge commands, and will perform their own tasks. I might allow them to fight if attacked, but undecided (currently they wont)
//This should be the super class to all pacifists, but currently just directly handles the miner
public class Miner : Unit
{
    public string Resource = "Mine";
    public bool Full = false;

    public int BagAmount;
    public AudioSource dropOff;

    public SpriteRenderer pick;
    // Start is called before the first frame update
    void Start()
    {
        AttackTimer = AttackCooldown;
        State = "Walk";
        maxHealth = HP;
        if (Team < 0)
        {
            HealthBar.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    // Update is called once per frame
    void Update()
    {
        AssemblePoint = General.RallyPoint;
        if (HealthTimer < AppearanceTime)
        {
            HealthTimer += Time.deltaTime;
        }
        else if (HealthTimer > AppearanceTime)
        {
            HealthBar.SetActive(false);
            HealthTimer = AppearanceTime; //Stops the timer from continuing to add
        }

        if (State == "Wait")
        {
            Wait();
        }
        else if (State == "Walk")
        {
            Walk();
        }
        else if (State == "Attack")
        {
            Attack();
        }
        else if (State == "Retreat")
        {
            Retreat();
        }
    }

    public override void Walk()
    {
        
        //Walk information
        if (!Full)
        {
            this.Move(new Vector3(MoveSpeed * Team * Time.deltaTime, 0, 0));
            
            
        }
        else
        {
            this.Move(new Vector3(MoveSpeed * -Team * Time.deltaTime, 0, 0));

        }

        
    }


    IEnumerator IMine()
    {
        Full = true;
        //Debug.Log("Time Starting");
        yield return new WaitForSeconds(2);

        
        
        State = "Walk";
        transform.Rotate(new Vector3(0, 180, 0));

        //Debug.Log("Time ending");
    }

    public void changeResource() {
        if (!Full)
        {
            if (Resource.Equals("Mine"))
            {
                Resource = "Gem";
                pick.color = Color.magenta;
            }
            else if (Resource.Equals("Gem"))
            {
                Resource = "Mine";
                pick.color = Color.yellow;

            }
        } 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Resource))        
        {
            State = "Mining";   //Being in the mining State should mean that it does nothing
            StartCoroutine(IMine());
        }

        if (collision.gameObject.CompareTag("Building"))         //Will be compared to barracks/team base eventually
        {
            if (Full == true) {
                dropOff.Play();
                if (Resource.Equals("Mine"))
                {
                    General.Gold += BagAmount;
                }
                else if (Resource.Equals("Gem") && General.Gems < 2) {
                    General.Gems++;
                }
                
                Full = false;
                transform.Rotate(new Vector3(0, 180, 0));
            }
            //State = "Walk";
           
        }
    }

}
