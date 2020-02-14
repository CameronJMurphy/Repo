using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IWeapon : MonoBehaviour
{
	[SerializeField] private int damage;
	[SerializeField] private float speed;
	private bool isHeld;
	private float angleIncrement;
	private bool attacking;
	private Vector2 mouseVecOnAttack;
	private void Start()
	{
		angleIncrement = 0;
		attacking = false;
		gameObject.SetActive(false);
	}

	public void Attack()
	{
		//swing weapon
		if (!attacking)
		{
			angleIncrement = 0;
			attacking = true;
			gameObject.SetActive(true);
			mouseVecOnAttack = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
		}
		
	}
	
	public bool IsHeld() //is it held?
	{
		return isHeld;
	}
	public void PickUp() //it is now held
	{
		isHeld = true;
	}
	public void Drop() //it is no longer held
	{
		isHeld = false;
	}

	public void FollowPlayer() //called every update
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

	public int Damage()
	{
		return damage;
	}
}
