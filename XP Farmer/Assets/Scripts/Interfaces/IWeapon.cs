﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// \file
/**
 \brief The weapon our character uses
  */
public class IWeapon : MonoBehaviour
{
	[SerializeField] private int damage;
	[SerializeField] private float speed;
	private bool isHeld;
	private float angleIncrement;
	private bool attacking;
	private Vector2 mouseVecOnAttack; ///< stores mouse vector2
	private Vector2 joyVecOnAttack;
	protected Joystick joystick;///< the joystick the player uses to move

	private void Start()
	{
		angleIncrement = 0;
		attacking = false;
		gameObject.SetActive(false);
		joystick = FindObjectOfType<Joystick>(); 

	}

	public void Attack() /// called when the attack key is pressed
	{
		if (!attacking) /// Attack if not attacking already 
		{
			angleIncrement = 0; /// reset angle increment
			attacking = true; /// set attacking bool to true
			gameObject.SetActive(true); /// set weapon active
			mouseVecOnAttack = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position; /// save mouse pos
			joyVecOnAttack = new Vector2(joystick.Vertical, joystick.Horizontal);
		}
		
	}
	
	public bool IsHeld() /// \returns bool isHeld variable
						/// \param none
	{
		return isHeld;
	}
	public void PickUp() /// sets isHeld to true
	{
		isHeld = true;
	}
	public void Drop() /// sets isHeld to false
	{
		isHeld = false;
	}

	public void UpdateWeapon() /// called every update, it updates the position of weapon based on mouse pos
	{
		if (!attacking)
		{
			Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
			float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
			angle -= 45; //this is because the sprite is at a 45 degree angle
			Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

			transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed * Time.deltaTime);


			Debug.Log(angleIncrement);
		}
		if (attacking)
		{
			angleIncrement += speed * Time.deltaTime;

			float angle = Mathf.Atan2(mouseVecOnAttack.y, mouseVecOnAttack.x) * Mathf.Rad2Deg;
			angle -= 90; //this is because the sprite is at a 45 degree angle and i want the attack start from a 45 behind
			Quaternion rotation = Quaternion.AngleAxis(angle + angleIncrement, Vector3.forward);

			transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed * Time.deltaTime);

			if (angleIncrement > 90) // full rotation
			{
				//reset
				attacking = false;
				angleIncrement = 0;
				gameObject.SetActive(false);
			}
		}

		transform.position = FindObjectOfType<PlayerCon>().transform.position;
	}

	public int Damage()///returns damage variable
	{
		return damage;
	}
}
