using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Handles the Base health which is always shown in the UI
public class BaseHealthBar : MonoBehaviour
{
	public Slider slider;
	public Damageable HealthObject;
	
	private void Start()
    {
		slider.maxValue = HealthObject.HP;
	}

    void Update()
	{
		
		slider.value = HealthObject.HP;

	}
	

}
