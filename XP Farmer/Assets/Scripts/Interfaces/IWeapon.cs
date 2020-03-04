using System.Collections;
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
	private GameObject ground;
	private GameObject pivot;

	private void Start()
	{
		angleIncrement = 0;
		attacking = false;
		gameObject.SetActive(false);
		joystick = FindObjectOfType<Joystick>();
		ground = GameObject.FindGameObjectWithTag("Ground");
		pivot = transform.parent.gameObject;

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
		if (attacking) /// if player is attack
		{
			angleIncrement += speed * Time.deltaTime; ///add to our angleIncrement var
			///apply raycast from the touch on the screen or mouse to the ground
			RaycastHit info;
			Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (ground.GetComponent<BoxCollider>().Raycast(mouseRay, out info, 10000))
			{
				///setup diection vector and angle
				Vector3 direction = info.point - transform.position;
				float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;

				///apply roation to the weapons pivot
				Quaternion rotation = Quaternion.AngleAxis(-angle + angleIncrement, Vector3.up);
				pivot.transform.rotation = Quaternion.Slerp(pivot.transform.rotation, rotation, speed * Time.deltaTime);
			}
			
			if (angleIncrement > 45) // full rotation
			{
				//reset
				attacking = false;
				angleIncrement = 0;
				gameObject.SetActive(false);
			}
		}

		transform.position = FindObjectOfType<PlayerCon>().transform.position; ///have the weapon follow the player
	}

	public int Damage()///returns damage variable
	{
		return damage;
	}
}
