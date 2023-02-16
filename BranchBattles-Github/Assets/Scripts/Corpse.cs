using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corpse : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CorpseDisappear());
    }

    IEnumerator CorpseDisappear() {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}
