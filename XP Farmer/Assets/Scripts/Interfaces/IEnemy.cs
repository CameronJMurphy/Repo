using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IEnemy : MonoBehaviour
{
	public int health;
	public float movespeed;
	public int scoreValue;
	[SerializeField]private int damage;

	public delegate bool IsDefending();
	public static IsDefending isDefending;

	private Animator aniCon;

	private bool facingRight;
	//these are so the detah animation can play
	private float deathTimer;
	private bool dying;
	private bool dead;
	private void Start()
	{
		dying = false;
		dead = false;
		deathTimer = 2;
		aniCon = GetComponent<Animator>();
		facingRight = true;
	}

	public void TakeDamage(IWeapon weapon)
	{
		health -= weapon.Damage();
	}

	public int Damage()
	{
		return damage;
	}

	public void ChangeDamage(int amount)
	{
		damage = amount;
	}

	public void Move(Vector3 target)
	{
		GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0); //i dont want them to slide everywhere due to outside forces
		Vector3 direction = target - GetComponent<Transform>().position;
		GetComponent<Transform>().position = GetComponent<Transform>().position + (direction.normalized * movespeed * Time.deltaTime);
	}

	public void FlipImage(Vector3 target)
	{
		float horizontal = target.x - transform.position.x;
		if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight && !dying)
		{
			facingRight = !facingRight;
			Vector3 flipedImage = gameObject.transform.localScale;
			flipedImage.x *= -1;//flip gameObject
			gameObject.transform.localScale = flipedImage;
		}
	}

	public bool IsAlive()
	{
		if (health > 0)
		{
			return true;
		}
		else
		{
			
			dying = true;
			return false;
		}
	}

	private void killThis()
	{
		health = 0;
	}

	public void DeathCheck()
	{
		if (dying)
		{
			ChangeDamage(0); // this is because during the death animation of this we dont want it to continue to deal damage
			gameObject.GetComponent<BoxCollider>().enabled = false;
			aniCon.SetBool("RatIsDead", true);
			deathTimer -= Time.deltaTime;
			if (deathTimer < 0)
			{
				dead = true;
				dying = false;
			}
		}
	}
	public bool IsDead()
	{
		return dead;
	}

	public bool IsDying()
	{
		return dying;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Player") && !isDefending()) //if they hit the player and he isnt defending
		{
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
