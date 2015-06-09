using UnityEngine;
using System.Collections;

public class Lazermove : MonoBehaviour {

	public float speed;

	void Update () 
	{
		transform.position += transform.forward * Time.deltaTime;
	}
}
