using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCon : MonoBehaviour
{
	private List<IWeapon> weapons;
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
				weapon.FollowPlayer();
				if (Input.GetKey(KeyCode.Space))
				{
					weapon.Attack();
				}
				
			}

			
		}
	}


}
