using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudGenerator : MonoBehaviour
{
    public GameObject clouds;

    public float MaxInterval;

    public float minSpeed;
    public float maxSpeed;

    public float minSize;
    public float maxSize;

    private void Start()
    {
        StartCoroutine(SpawnClouds());
    }

    IEnumerator SpawnClouds() {
        float delay = Random.Range(0, MaxInterval);
        yield return new WaitForSeconds(delay);

        Debug.Log("Spawning Cloud");
        GameObject NewCloud = Instantiate(clouds, new Vector3(transform.position.x, transform.position.y + Random.Range(-1, 1), transform.position.z), Quaternion.identity, transform);
        

        Cloud ThisCloud = NewCloud.GetComponent<Cloud>();

        ThisCloud.speed = Random.Range(minSpeed, maxSpeed);
        ThisCloud.time = 30 / ThisCloud.speed;

        //float size = Random.Range(minSize, maxSize);
        NewCloud.transform.localScale = new Vector3(Random.Range(minSize, maxSize), Random.Range(minSize, maxSize)/2, 0);


        StartCoroutine(SpawnClouds());
    }
}
