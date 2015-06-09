using UnityEngine;
using System.Collections;

public class VFXManager : MonoBehaviour 
{

	public static VFXManager Instance;
	public GameObject[]effects;

	void Awake () 
	{
		Instance = this;
	}

	public void Spawn (string name, Vector3 position, Quaternion rotation) 
	{
		foreach (GameObject fx in effects) 
		{
			if (fx.name == name)
			{
				Instantiate (fx, position, rotation);
				return;
			}
		}
		Debug.Log ("Cannot find effect" + name);
	}
}
