using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// \file
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
	//these are so the death animation can play
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

	public void TakeDamage(IWeapon weapon) /// /param IWeapon reduce this units health by the damage value of the weapon
	{
		health -= weapon.Damage();
	}

	public int Damage() /// \return int return the damage value of this unit
	{
		return damage;
	}

	public void ChangeDamage(int amount) /// \param int amount  
	{
		damage = amount; ///change damage to equal amount
	}

	public void Move(Vector3 target) /// \param Vector3 target
	{
		GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0); ///Set velocity to 0 to stop slide everywhere due to outside forces
		Vector3 direction = target - GetComponent<Transform>().position; ///create direction vector
		GetComponent<Transform>().position = GetComponent<Transform>().position + (direction.normalized * movespeed * Time.deltaTime); /// move this unit
	}

	public void FlipImage(Vector3 target) /// \param Vector3 target
	{
		float horizontal = target.x - transform.position.x; /// Our horizontal var is our direction
		if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight && !dying) /// if there facing the wrong way
		{
			///flip this units image
			facingRight = !facingRight;
			Vector3 flipedImage = gameObject.transform.localScale;
			flipedImage.x *= -1;//flip gameObject
			gameObject.transform.localScale = flipedImage;
		}
	}

	public bool IsAlive() /// \return bool
	{
		if (health > 0) ///if health is greater than 0
		{
			return true; ///return true
		}
		else ///else
		{
			/// set dying var to true then return false
			dying = true;
			return false;
		}
	}

	private void killThis() ///make this units health = 0
	{
		health = 0;
	}

	public void DeathCheck()
	{
		if (dying) ///if this unit is dying
		{
			/// change damage to zero
			ChangeDamage(0); // this is because during the death animation of this we dont want it to continue to deal damage
			gameObject.GetComponent<BoxCollider>().enabled = false; ///set this units collider to false
			aniCon.SetBool("RatIsDead", true); ///play its death animation
			///when the deathTimer runs out then the unit is considered dead
			deathTimer -= Time.deltaTime;
			if (deathTimer < 0)
			{
				dead = true;
				dying = false;
			}
		}
	}
	public bool IsDead() /// \return bool dead var
	{
		return dead;
	}

	public bool IsDying() /// \return bool dying var
	{
		return dying;
	}

	private void OnCollisionEnter(Collision collision) /// \param Collision collision
	{
		///if this unit hits the player and the player isn't defending
		if (collision.gameObject.CompareTag("Player") && !isDefending()) 
		{
			killThis(); ///kill this unit
		}
	}

	private void OnTriggerEnter(Collider other) /// \param  Collider other
	{
		if (other.gameObject.CompareTag("Weapon")) /// if this unit collides with the players weapon
		{
			///apply weapon damage to this unit
			IWeapon hitBy = FindObjectOfType<PlayerCon>().CurrentWeapon(); 
			TakeDamage(hitBy);
		}
	}
	

}
