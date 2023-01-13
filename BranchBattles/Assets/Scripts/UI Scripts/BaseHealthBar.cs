using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseHealthBar : MonoBehaviour
{
	public Slider slider;
	public Damageable HealthObject;
	//public void SetMaxHealth(int health)

	private void Start()
    {
		slider.maxValue = HealthObject.HP;
	}

    void Update()
	{
		
		slider.value = HealthObject.HP;

	}
	/*
	public void SetHealth(int health)
	{
		slider.value = health;

	}*/

}
