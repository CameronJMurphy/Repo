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
	protected Joystick joystick;
	[SerializeField] protected Joybutton dashButton;
	private bool joystickIsTouched;
	[SerializeField]private Collider playArea;
	private bool heldDown;


	// Start is called before the first frame update
	void Start()
    {
		heldDown = false;
		joystickIsTouched = false;
		facingRight = true;
		defenseMode = false;
		maxHealth = health;
		PC = FindObjectOfType<PlayerCon>();
		weapon.PickUp();
		IEnemy.isDefending += IsDefending;
		joystick = FindObjectOfType<Joystick>();
		
    }

    // Update is called once per frame
    void Update()
    {
		AliveStatus();
		MovementUpdate();
		FlipImage();
	}

	private void MovementUpdate()
	{
#if UNITY_STANDALONE_WIN
		//get the axis
		float h_value = Input.GetAxis("Horizontal");
		float v_value = Input.GetAxis("Vertical");
		//get direction vector
		Vector3 direction = new Vector3(h_value, 0, v_value); //this is the direction of the player movement input
		//PC Walking
		Walking(h_value, v_value);

		//check for dashes
		if(Input.GetButtonDown("Dash"))
		{
			Dash(direction);
		}
#endif
#if UNITY_ANDROID
		//Android Walking
		Vector3 direction = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
		Walking(joystick.Horizontal, joystick.Vertical);
		joystickIsTouched = false;
		foreach (var touch in Input.touches)
		{
			if(Camera.main.ScreenToWorldPoint(touch.position) == joystick.transform.position)
			{
				joystickIsTouched = true;
			}
		}
		if (dashButton.Pressed && heldDown == false)
		{
			heldDown = true;
			Dash(direction); //they dash in the direction of the joystick
		}
		if(!dashButton.Pressed)
		{
			heldDown = false;
		}
#endif

	}
	
	public bool JoystickIsBeingUsed()
	{
		return joystickIsTouched;
	}
	private void Walking(float horizontal, float vertical)
	{
		Vector3 temp = PC.transform.position + new Vector3(horizontal * movementSpeed * Time.deltaTime, 0, vertical * movementSpeed * Time.deltaTime);
		if(WithinBounds(temp))
		{
			PC.transform.position = temp;
		}
		//PC.transform.position += new Vector3(horizontal * movementSpeed * Time.deltaTime, 0, vertical * movementSpeed * Time.deltaTime);
	}

	private bool WithinBounds(Vector3 v)
	{
		return playArea.bounds.Contains(v);
	}
	private void Dash(Vector3 direction)
	{
		Vector3 temp = PC.transform.position + direction.normalized * dashDistance;
		if (WithinBounds(temp))
		{
			PC.transform.position = temp;
		}
		//PC.transform.position += direction.normalized * dashDistance;
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
		//We arent using velocity, so we reset it to make sure that collisions aren't applying unwanted velocity
		PC.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
		if (collision.gameObject.CompareTag("Enemy") && defenseMode == false ) //in defense mode you cant take damage
		{
			TakeDamage(collision.gameObject.GetComponent<IEnemy>().Damage()); //deal damge to the player = to the damage the enemy deals
			
			//transform.position = Vector3.Lerp(transform.position, (transform.position - collision.transform.position).normalized * pushBackAmount, pushBackSpeed); // this is a pushback when the player is hit
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
