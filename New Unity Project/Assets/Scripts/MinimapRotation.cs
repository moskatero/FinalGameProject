/*
 * Author: Run Ming Zhong
 * Description: Makes minimap camera orbits around a 3d map,
 * so it looks like minimap is rotating.
 */

using UnityEngine;
using System.Collections;

public class MinimapRotation : MonoBehaviour {

	public GameObject map;
	public float moveSpeed;

	private float x;
	private float y;

	void Update () 
	{
		if (map) 
		{
			if (Input.GetKey (KeyCode.Keypad6))
			{
				x = 1;
			}
			else if (Input.GetKey (KeyCode.Keypad4))
			{
				x = -1;
			}
			else if (Input.GetKey (KeyCode.Keypad2))
			{
				y = 1;
			}
			else if (Input.GetKey (KeyCode.Keypad8))
			{
				y = -1;
			}
			else
			{
				x = 0;
				y = 0;
			}
			transform.RotateAround(map.transform.position, new Vector3(y, x, 0.0f), moveSpeed);
		}
	}

	void OnPreRender() {
		GL.wireframe = true;
	}
	void OnPostRender() {
		GL.wireframe = false;
	}
}
