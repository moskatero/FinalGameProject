using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
	public int maxHealth;

	int health;

	void Start()
	{
		health = maxHealth;
	}	

	public void TakeDamage(int amount)
	{
		health -= amount;
		if (health <= 0)
		{
			Destroy(gameObject);
		}
	}
}