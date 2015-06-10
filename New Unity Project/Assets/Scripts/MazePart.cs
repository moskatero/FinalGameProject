using UnityEngine;
using System.Collections;

public class MazePart : MonoBehaviour 
{

	public string[] Tags;
	
	public MazePartConnector[] GetExits()
	{
		return GetComponentsInChildren<MazePartConnector>();
	}
}
