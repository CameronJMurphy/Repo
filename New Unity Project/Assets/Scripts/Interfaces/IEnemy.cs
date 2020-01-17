using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IEnemy : MonoBehaviour
{
	public int health;
	public float movespeed;
	public int scoreValue;

	public delegate bool IsDefending();
	public static IsDefending isDefending;

	public void TakeDamage(IWeapon weapon)
	{
		health -= weapon.Damage();
	}

	public void Move(Vector3 target)
	{
		GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0); //i dont want them to slide everywhere due to outside forces
		Vector3 direction = target - GetComponent<Transform>().position;
		GetComponent<Transform>().position = GetComponent<Transform>().position + (direction.normalized * movespeed * Time.deltaTime); 
	}

	public bool IsAlive()
	{
		if (health > 0)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	private void killThis()
	{
		health = 0;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Player") && !isDefending()) //if they hit the player and he isnt defending
		{
			gameObject.SetActive(false);
			killThis();
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Weapon"))
		{
			IWeapon hitBy = FindObjectOfType<PlayerCon>().CurrentWeapon();
			TakeDamage(hitBy);
		}
	}

}
