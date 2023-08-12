using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A magic spell, which strikes an enemy and then chains bolts together to deal damage to additional enemies
//Might also reduce movement or attack speed
public class Lightning : MonoBehaviour
{
    public GameObject lightningBolt;
    public float stormSpawnHeight;
    public float lightningDuration; 
    public float spawnRadius;
    public float spawnHeightRange;
    public int chanceToSpawn; //Will get called each frame so will be quite low

    // Start is called before the first frame update
    public void UseMagic()
    {
        transform.position = new Vector3(transform.position.x, stormSpawnHeight, 0);
        StartCoroutine(LightningStorm());
    }

    // Update is called once per frame
    void Update()
    {
        if (Random.Range(0, 1000) < chanceToSpawn) {
            SpawnLightningBolt();
        }
    }

    private void SpawnLightningBolt() {
        float xPos = Random.Range(transform.position.x - spawnRadius, transform.position.x + spawnRadius);
        float yPos = Random.Range(transform.position.y - spawnHeightRange/2, transform.position.y + spawnHeightRange/2);
        Instantiate(lightningBolt, new Vector3(xPos, yPos, 0), Quaternion.identity);
    }

    IEnumerator LightningStorm()
    {

        yield return new WaitForSeconds(lightningDuration);
        transform.parent.gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(spawnRadius * 2, spawnHeightRange, 1));
    }
}
