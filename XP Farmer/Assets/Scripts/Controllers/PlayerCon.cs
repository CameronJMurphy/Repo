using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCon : MonoBehaviour
{
	private PlayerCon PC;
	public IWeapon weapon;
	public float movementSpeed;
	[SerializeField][Range(0,1)] private float dashSpeed;
	[SerializeField] private float dashDistance;
	private int maxHealth;
	[SerializeField] private int health;
	public float pushBackSpeed; //this is triggered when hit by a monster
	public float pushBackAmount;
	private bool defenseMode;
	private Animator walk;
	private bool facingRight;

    // Start is called before the first frame update
    void Start()
    {
		facingRight = true;
		walk = GetComponent<Animator>();
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
	}

	private void Movement()
	{
		GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0); // this stops the player sliding everywhere when the collide with otherthings
		//on PC WASD movement
		if (Input.GetKey(KeyCode.W))
		{
			PC.transform.position = PC.transform.position + (Vector3.up * movementSpeed * Time.deltaTime);//normal movement
			if(Input.GetKeyDown(KeyCode.Space)) 
			{
				PC.transform.position = Vector3.Lerp(PC.transform.position, PC.transform.position + (Vector3.up * dashDistance * Time.deltaTime), dashSpeed) ;//dash
			}
		}
		if (Input.GetKey(KeyCode.A))
		{
			PC.transform.position = PC.transform.position + (Vector3.left * movementSpeed * Time.deltaTime);//normal movement
			FlipImage();
			if (Input.GetKeyDown(KeyCode.Space))
			{
				PC.transform.position = Vector3.Lerp(PC.transform.position, PC.transform.position + (Vector3.left * dashDistance * Time.deltaTime), dashSpeed);//dash
			}
		}
		if (Input.GetKey(KeyCode.S))
		{
			PC.transform.position = PC.transform.position + (Vector3.down * movementSpeed * Time.deltaTime);//normal movement
			if (Input.GetKeyDown(KeyCode.Space))
			{
				PC.transform.position = Vector3.Lerp(PC.transform.position, PC.transform.position + (Vector3.down * dashDistance * Time.deltaTime), dashSpeed);//dash
			}
		}
		if (Input.GetKey(KeyCode.D))
		{
			PC.transform.position = PC.transform.position + (Vector3.right * movementSpeed * Time.deltaTime);//normal movement
			FlipImage();
			if (Input.GetKeyDown(KeyCode.Space))
			{
				PC.transform.position = Vector3.Lerp(PC.transform.position, PC.transform.position + (Vector3.right * dashDistance * Time.deltaTime), dashSpeed);//dash
			}
		}
		
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
