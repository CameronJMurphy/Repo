using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCon : MonoBehaviour
{
	private List<IWeapon> weapons;
	private PlayerCon PC;
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
		foreach (IWeapon weapon in weapons)
		{
			
			if (weapon.IsHeld())
			{
				weapon.UpdateWeapon();
#if UNITY_STANDALONE_WIN
				if (Input.GetMouseButtonDown(0))
				{
					weapon.Attack();
				}
#endif
#if UNITY_ANDROID
				if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) //because the user will often use two fingers
				{
					weapon.Attack();
				}
#endif

			}


		}
	}


}
