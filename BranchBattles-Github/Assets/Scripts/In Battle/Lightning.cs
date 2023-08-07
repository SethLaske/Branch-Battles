using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A magic spell, which strikes an enemy and then chains bolts together to deal damage to additional enemies
//Might also reduce movement or attack speed
public class Lightning : MonoBehaviour
{
    public GameObject Bolt;
    public Collider2D AOE;
    public float duration;
    public float dropSpeed;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(transform.position.x, 4f, 0);
        StartCoroutine(LightningBolt());
        //Instantiate(Bolt, transform.position, Quaternion.Euler(new Vector2(0, 0)));
        //Instantiate(Bolt, transform.position, Quaternion.Euler(new Vector2(0, 180)));
    }

    // Update is called once per frame
    void Update()
    {
        AOE.offset += Vector2.down * dropSpeed * Time.deltaTime;
    }



    //Very easy to hit two units at a time which spawns in two pairs of bolts, but the second pair dont move
    private void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject other = collider.gameObject;
        Unit unit = other.GetComponent<Unit>();
        if (unit != null)
        { //Bolt would need a team check... but its lightning 
            
            //unit.Stun 
            Debug.Log("Spawning new bolts");
            Instantiate(Bolt, unit.transform.position + new Vector3(1, 0, 0), Quaternion.Euler(new Vector2(0, 0)));
            Instantiate(Bolt, unit.transform.position + new Vector3(-1, 0, 0), Quaternion.Euler(new Vector2(0, 180)));
            
            Destroy(gameObject);
            unit.TakeDamage(50);
        }
    }

    IEnumerator LightningBolt()
    {

        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}
