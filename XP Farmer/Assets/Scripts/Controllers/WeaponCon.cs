using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// \file
public class WeaponCon : MonoBehaviour
{
	private List<IWeapon> weapons;///< List of weapons in the gameworld
	private PlayerCon PC; ///< reference to player
	// Start is called before the first frame update
	void Awake()
    {
		PC = FindObjectOfType<PlayerCon>();
		weapons = new List<IWeapon>(FindObjectsOfType<IWeapon>());
	}

    // Update is called once per frame
    void Update()
    {
		WeaponCheck();
	}

	private void WeaponCheck()
	{
		foreach (IWeapon weapon in weapons) /// foreach weapon is game world
		{
			
			if (weapon.IsHeld()) /// if it is held
			{
				weapon.UpdateWeapon(); /// update the weapon
#if UNITY_STANDALONE_WIN
				if (Input.GetMouseButtonDown(0)) /// if on computer && the left click with a mouse
				{
					weapon.Attack(); ///attack with the weapon
				}
#endif
#if UNITY_ANDROID
				if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) /// if on Android && the player presses with one or two finger on the screen 
				{
					weapon.Attack(); /// the weapon attacks
				}
#endif

			}


		}
	}


}
