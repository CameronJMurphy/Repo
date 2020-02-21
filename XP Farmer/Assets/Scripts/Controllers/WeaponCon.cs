using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCon : MonoBehaviour
{
	private List<IWeapon> weapons;
	[SerializeField] private Joybutton attackButton;
	// Start is called before the first frame update
	void Start()
    {
		weapons = new List<IWeapon>(FindObjectsOfType<IWeapon>());
	}

    // Update is called once per frame
    void Update()
    {
		WeaponCheck();
	}

	private void WeaponCheck()
	{
		foreach (IWeapon weapon in weapons)
		{
			
			if (weapon.IsHeld())
			{
				weapon.UpdateWeapon();
				if (Input.GetMouseButtonDown(0) || attackButton.Pressed)
				{
					weapon.Attack();
				}
				
			}

			
		}
	}


}
