using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/// \file
/// <summary>
/// \brief Controls all action the player has direct control of
/// </summary>
public class PlayerCon : MonoBehaviour
{
	///create variables
	private PlayerCon PC;
	public IWeapon weapon; ///< Current weapon
	public float movementSpeed; 
	[SerializeField] private float dashDistance;
	private int maxHealth;
	[SerializeField] private int health;
	//public float pushBackSpeed; //this is triggered when hit by a monster
	//public float pushBackAmount;
	private bool defenseMode; ///< bool to tell when I am shielding
	private bool facingRight;
	protected Joystick joystick;
	[SerializeField] protected Joybutton dashButton;
	private bool joystickIsTouched;
	[SerializeField]private Collider playArea;
	private bool heldDown; ///< bool to know if the dash button is being held down


	// Start is called before the first frame update
	void Start()
    {
		///instanciate variables
		heldDown = false;
		joystickIsTouched = false;
		facingRight = true;
		defenseMode = false;
		maxHealth = health;
		PC = FindObjectOfType<PlayerCon>();
		weapon.PickUp();
		///set up delegate
		IEnemy.isDefending += IsDefending;
		joystick = FindObjectOfType<Joystick>();
		
    }

    // Update is called once per frame
    void Update()
    {
		AliveStatus();///Are we alive
		MovementUpdate(); ///check to see if our player is moving 
	}

	private void MovementUpdate()
	{
		///if the build is on PC then excute this 
#if UNITY_STANDALONE_WIN 
		///get movement axis
		float h_value = Input.GetAxis("Horizontal");
		float v_value = Input.GetAxis("Vertical");
		///get direction vector
		Vector3 direction = new Vector3(h_value, 0, v_value); //this is the direction of the player movement input
		///PC Walking
		Walking(h_value, v_value);

		///check for dashes
		if(Input.GetButtonDown("Dash"))
		{
			Dash(direction);
		}
		///Check to see if image should be flipped
		FlipImage(h_value);
#endif
		///if the build is on Android then excute this
#if UNITY_ANDROID
		///Android Walking
		///Get direction vector
		Vector3 direction = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
		Walking(joystick.Horizontal, joystick.Vertical); ///update players walk inputs  
		//joystickIsTouched = false; /// reset joystickIsTouched variable
		//foreach (var touch in Input.touches)
		//{
		//	if(Camera.main.ScreenToWorldPoint(touch.position) == joystick.transform.position)
		//	{
		//		joystickIsTouched = true;
		//	}
		//}
		if (dashButton.Pressed && heldDown == false) /// if dashbutton has been pressed and it isnt being held down then 
		{
			heldDown = true; ///heldDown equals true
			Dash(direction); ///they dash in the direction of the joystick
		}
		if(!dashButton.Pressed) ///if dash button isn't being pressed then
		{
			heldDown = false; ///heldDown equals  false
		}
		FlipImage(joystick.Horizontal); ///check to see if the players image should be flipped

#endif

	}

	public bool JoystickIsBeingUsed() /// \return bool joystickIsTouched 
	{
		return joystickIsTouched;
	}
	private void Walking(float horizontal, float vertical) /// \Param float horizontal input, float vertical Input
	{
		///create temp position
		Vector3 temp = PC.transform.position + new Vector3(horizontal * movementSpeed * Time.deltaTime, 0, vertical * movementSpeed * Time.deltaTime);
		if(WithinBounds(temp)) ///if the temp position is within bounds
		{
			PC.transform.position = temp; ///update the players position to equal temp
		}
	}

	private bool WithinBounds(Vector3 v) /// \return bool \param vector3  poisition
	{
		return playArea.bounds.Contains(v); ///if the position is within the playArea then return true else return false 
	}
	private void Dash(Vector3 direction)/// \param Vector3 direction
	{
		///make temp position
		Vector3 temp = PC.transform.position + direction.normalized * dashDistance;
		if (WithinBounds(temp)) /// if temp is within bounds 
		{
			PC.transform.position = temp; ///update players position to equal temp position
		}
	}

	private void FlipImage(float horizontal) /// \param float horizontal
	{
		//float horizontal = Input.GetAxis("Horizontal");
		if (horizontal > 0 && !facingRight ///if horizontal input is greater than 0 and not facing right or
		 || horizontal < 0 && facingRight) ///if horizontal input less than 0 and facing right then
		{
			facingRight = !facingRight; ///switch facingRight variable to its oppsite
			///flip the image
			Vector3 flipedImage = gameObject.transform.localScale;
			flipedImage.x *= -1;//flip the player model
			gameObject.transform.localScale = flipedImage;
		}
	}

	public void SetDefending(bool _isDefending) ///use to set defenseMode \param bool _isdefeneding
	{
		defenseMode = _isDefending; 
	}

	public void TakeDamage(int damage) /// \param int damage 
	{
		///reduce health by damage amount
		health -= damage;
	}

	public IWeapon CurrentWeapon() /// \return IWeapon return the current weapon equiped to the player 
	{
		return weapon;
	}

	private void OnCollisionEnter(Collision collision) /// \param Collision
	{
		///We reset velocity to make sure that collisions aren't applying unwanted velocity
		PC.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
		if (collision.gameObject.CompareTag("Enemy") && defenseMode == false ) /// if you hit an enemy and your not in defense mode (in defense mode you cant take damage)
		{
			TakeDamage(collision.gameObject.GetComponent<IEnemy>().Damage()); ///deal damge to the player equal to the damage the enemy deals
		}
	}
	
	private void AliveStatus() ///acts depending on the alive status of the plaer
	{
		if(!IsAlive()) ///if the player isn't alive then
		{
			///reload the scene
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
	}


	private bool IsAlive() /// \return bool returns true if the player is alive and false if they're dead
	{
		if(health > 0) ///if the players health is greater than 0
		{
			return true; ///return true
		}
		else ///else
		{
			return false; ///return false
		}
	}

	public float PlayerHealth() /// \return float the players current health
	{
		return health;
	}
	public float MaxPlayerHealth() /// \return float the players maximum health
	{
		return maxHealth;
	}
	public void healthIncrease(int amount) /// \param int amount 
	{
		///increase the current and max health of the player
		health += amount;
		maxHealth += amount;
	}

	public bool IsDefending() /// \return bool returns true if the player is defending
	{
		return defenseMode;
	}

}
