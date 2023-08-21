using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Pacifists wont obey the rally or charge commands, and will perform their own tasks. I might allow them to fight if attacked, but undecided (currently they wont)
//This should be the super class to all pacifists, but currently just directly handles the miner
public class Miner : Unit
{
    private string Resource = "Mine";
    //public bool Full = false;

    public float miningSpeed;
    public int maxGoldAmount;
    private int currentGoldAmount;
    public AudioSource dropOff;

    public float miningIncrement;

    private Mine mine;
    private Vector3 mineSpot;
    private Vector3 nullMine = new Vector3(0, 100, -100);

    [SerializeField] Vector2 mineDetectionBoxSize;
    [SerializeField] Transform mineDetectionCenter;
    // Start is called before the first frame update
    void Start()
    {
        StandardStart();
        animator.SetTrigger("Walking");
        animator.ResetTrigger("Walking");
        currentGoldAmount = 0;
        mineSpot = nullMine;
    }

    // Update is called once per frame
    void Update()
    {
        AssemblePoint = General.rallyPoint;
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
    }

    //Honestly, these guys are pretty simple and just walk back and forth. No need to make them more complicated
    public override void Walk()
    {
        
        //Walk information
        if (currentGoldAmount == maxGoldAmount)
        {
            this.Move(new Vector3(currentSpeed * -Team * Time.deltaTime, 0, 0));
        }
        else if (mineSpot != nullMine)
        {
            this.Move(Advance(transform.position, mineSpot, Mathf.Abs(currentSpeed) * Time.deltaTime));
            if ((transform.position - mineSpot).magnitude < .1f) {
                
                State = "Mining";   //Being in the mining State should mean that it does nothing
                StartCoroutine(IMine(mine.miningTimeMultiplier));
                
            }
        }
        else {
            FindMine();
            this.Move(new Vector3(currentSpeed * Team * Time.deltaTime, 0, 0));
        }

        
    }


    IEnumerator IMine(float mineHardness)
    {
        float direction = Mathf.Sign(mine.transform.position.x - transform.position.x);
        transform.localScale = new Vector3(direction, 1, 1);

        mine.IncreaseMultiplier(miningIncrement);
        Debug.Log("Starting to mine");
        animator.SetTrigger("Mining");
        float swings = Mathf.Ceil((float)maxGoldAmount * mineHardness / miningSpeed);
        //Debug.Log("Time Starting");
        yield return new WaitForSeconds(attackAnimation.length * swings);
        currentGoldAmount = maxGoldAmount;

        animator.SetTrigger("Walking");
        State = "Walk";
        transform.localScale = new Vector3(-1 * Team, 1, 1);
        mineSpot = nullMine;
        mine = null;

        //Debug.Log("Time ending");
    }

    /*public void changeResource() {
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
    }*/
    private void FindMine() {
        //Debug.Log("Trying to find a mine");
        Collider2D[] colliders = Physics2D.OverlapBoxAll(mineDetectionCenter.position, mineDetectionBoxSize, 0);
        foreach (Collider2D collider in colliders)
        {
            //Debug.Log("Miner collides with: " + collider.name);
            Mine foundMine = collider.GetComponent<Mine>();
            if (foundMine != null)
            {
                if (mineSpot == nullMine) {
                    this.mine = foundMine;
                    mineSpot = GetMineSpot(foundMine);
                }
                else if ((foundMine.transform.position - transform.position).magnitude < (mineSpot - transform.position).magnitude)  
                {
                    this.mine = foundMine;
                    mineSpot = GetMineSpot(foundMine);
                }
            }

        }
    }

    private Vector3 GetMineSpot(Mine mine) {
        float randomAngle = Random.Range(0f, Mathf.PI * 2f);

        // Calculate random point within the circle using trigonometry
        float x = mine.transform.position.x + mine.radius * Mathf.Cos(randomAngle);
        float y = mine.transform.position.y + mine.radius * Mathf.Sin(randomAngle);

        return new Vector3(x, y, y/5);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        

        if (collision.gameObject.CompareTag("Building"))         //Will be compared to barracks/team base eventually
        {
            if (currentGoldAmount > 0) {
                dropOff.Play();
                if (Resource.Equals("Mine"))
                {
                    General.gold += currentGoldAmount;
                }
            }

            transform.localScale = new Vector3(1 * Team, 1, 1);
            currentGoldAmount = 0;
            //State = "Walk";
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(mineDetectionCenter.position, mineDetectionBoxSize);
    }

}
