using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolt : MonoBehaviour
{
    public Bolt bolt;
    public float Duration = .25f;
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Bolt created");
        StartCoroutine(ChainBolt());
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.rotation * (new Vector3(10, 0, 0) * Time.deltaTime);

    }

    IEnumerator ChainBolt() {
        yield return new WaitForSeconds(Duration);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject other = collider.gameObject;
        Unit unit = other.GetComponent<Unit>();
        if (unit != null) { //Bolt would need a team check... but its lightning 
            unit.TakeDamage(5);
            //unit.Stun(5, 1.5f); 
            Debug.Log("REMOVED STUN");

            Bolt chainedBolt = Instantiate(bolt, unit.transform.position + (transform.rotation * new Vector3(0.75f, 0, 0)), transform.rotation);
            chainedBolt.Duration = Duration - .1f;
            Destroy(gameObject);
        }
    }
}
