using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour 
{
	public GameObject[] weapons;
	public int current = 0;
	Weapon currentWeapon;
	
	void Start()
	{
		Screen.lockCursor = true;

		for(int i = 0;i < weapons.Length; ++i)
		{
			if(i == current)
			{
				weapons[i].gameObject.SetActive(true);
			}
			else
			{
				weapons[i].gameObject.SetActive(false);
			}
		}
	}
	
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Alpha1))
		{
			current = 0;
			SwitchWeapons(0);
		}
		else if(Input.GetKeyDown(KeyCode.Alpha2))
		{
			current = 1;
			SwitchWeapons(1);
		}
		else if(Input.GetMouseButtonDown(0))
		{
			currentWeapon = GetComponentInChildren<Weapon>();
			currentWeapon.Fire();
		}
	}

void SwitchWeapons(int num)
{
		current = num;
		for(int i = 0;i < weapons.Length; ++i)
		{
			if(i == num)
			{
				weapons[i].gameObject.SetActive(true);
				currentWeapon = GetComponentInChildren<Weapon>();
			}
			else
			{
				weapons[i].gameObject.SetActive(false);
				currentWeapon = GetComponentInChildren<Weapon>();
			}
		}
}

}