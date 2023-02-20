using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Handles the very basic unit health
public class UnitHealth : MonoBehaviour
{
   
    public GameObject HealthBar;    //Very simple rectangle to represent HP
    private float maxHealth;    //Saved to be used for calcs
    public Damageable HealthObject;     //The unit/building it belongs to
    public float AppearanceTime;    //The length of time the bar will show for
    
    public float Timer = 0;

    private void Start()
    {
        maxHealth = HealthObject.HP;    //Save the max HP for reference
    }

    void Update()
    {
        if (Timer < AppearanceTime)
        {
            Timer += Time.deltaTime;
        }
        else if (Timer > AppearanceTime) {
            HealthBar.SetActive(false);     //Hides the timer once it has been active long enough
            Timer = AppearanceTime; //Stops the timer from continuing to add
        }

    }

    public void showHealth()
    {
        //Sets the new size, reactivates the health and restarts the time 
        HealthBar.transform.localScale = new Vector3(HealthObject.HP / maxHealth, HealthBar.transform.localScale.y, HealthBar.transform.localScale.z);
        HealthBar.SetActive(true);
        Timer = 0;
    }

}
