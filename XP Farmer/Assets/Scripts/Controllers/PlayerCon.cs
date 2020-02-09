using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCon : MonoBehaviour
{
	private PlayerCon PC;
	public IWeapon weapon;
	public float movementSpeed;
	[SerializeField] private float dashDistance;
	private int maxHealth;
	[SerializeField] private int health;
	public float pushBackSpeed; //this is triggered when hit by a monster
	public float pushBackAmount;
	private bool defenseMode;
	private bool facingRight;

    // Start is called before the first frame update
    void Start()
    {
		facingRight = true;
		defenseMode = false;
		maxHealth = health;
		PC = FindObjectOfType<PlayerCon>();
		weapon.PickUp();
		IEnemy.isDefending += IsDefending;
    }

    // Update is called once per frame
    void Update()
    {
		AliveStatus();
		Movement();
		FlipImage();
	}

	private void Movement()
	{
		//get the axis
		float h_value = Input.GetAxis("Horizontal");
		float v_value = Input.GetAxis("Vertical");
		Vector3 direction = new Vector3(h_value, v_value,0);
		//direction vector
		direction *= Time.deltaTime * movementSpeed;
		//apply vector to position
		PC.transform.position += direction;
		//check for dashes
		if(Input.GetButtonDown("Dash"))
		{
			PC.transform.position += direction.normalized * dashDistance;//dash
		}
		//Android movement
		#if UNITY_ANDROID


		#endif
	}

	private void FlipImage()
	{
		float horizontal = Input.GetAxis("Horizontal");
		if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
		{
			facingRight = !facingRight;
			Vector3 flipedImage = gameObject.transform.localScale;
			flipedImage.x *= -1;//flip the player model
			gameObject.transform.localScale = flipedImage;
		}
	}

	public void SetDefending(bool _isDefending) //use to set defenseMode
	{
		defenseMode = _isDefending; 
	}

	public void TakeDamage(int damage)
	{
		health -= damage;
	}

	public IWeapon CurrentWeapon()
	{
		return weapon;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Enemy") && defenseMode == false ) //in defense mode you cant take damage
		{
			TakeDamage(collision.gameObject.GetComponent<IEnemy>().Damage()); //deal damge to the player = to the damage the enemy deals
			
			transform.position = Vector3.Lerp(transform.position, (transform.position - collision.transform.position).normalized * pushBackAmount, pushBackSpeed); // this is a pushback when the player is hit
		}
	}
	
	private void AliveStatus()
	{
		if(!IsAlive())
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
	}


	private bool IsAlive()
	{
		if(health > 0)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public float PlayerHealth()
	{
		return health;
	}
	public float MaxPlayerHealth()
	{
		return maxHealth;
	}
	public void healthIncrease(int amount)
	{
		health += amount;
		maxHealth += amount;
	}

	public bool IsDefending()
	{
		return defenseMode;
	}

}
