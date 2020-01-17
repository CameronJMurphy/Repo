using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IWeapon : MonoBehaviour
{
	[SerializeField] private int damage;
	private bool isHeld;
	private void Start()
	{
	}

	public void Attack()
	{
		////swing weapon
		//gameObject.SetActive(true);
		//animator.SetTrigger("Base_Attack");
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

	public void FollowPlayer()
	{
		float speed = 5f;
		Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		angle -= 45; //this is because the sprite is at a 45 degree angle
		Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

		transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed * Time.deltaTime);
		transform.position = FindObjectOfType<PlayerCon>().transform.position;
	}

	public int Damage()
	{
		return damage;
	}
}
