using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialExplanation : MonoBehaviour
{
    public float ReadBuffer = 2;
    public float UseTime = 5;
    public bool Read = false;

    public GameObject Explanation;
    public GameObject UIRevealed;

    public GameObject NextTutorial;
    
    // Start is called before the first frame update
    void Start()
    {
        Explanation.SetActive(true);
        if (UIRevealed != null) {
            UIRevealed.SetActive(true);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Read == false) {
            StartCoroutine(TimerRoutine());
            Read = true;
        }
    }

    IEnumerator TimerRoutine()
    {
        yield return new WaitForSeconds(ReadBuffer);
        Explanation.SetActive(false);
        yield return new WaitForSeconds(UseTime);
        if (NextTutorial != null) {
            NextTutorial.SetActive(true);
        }
        
        Destroy(gameObject);
    }


}
