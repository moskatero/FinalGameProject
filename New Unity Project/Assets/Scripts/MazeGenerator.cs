using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class MazeGenerator : MonoBehaviour 
{
	
	public MazePart[] Modules;
	public MazePart StartModule;
	
	public int Iterations = 5;
	
	
	void Start()
	{
		MazePart startModule = (MazePart)Instantiate( StartModule, transform.position, transform.rotation );
		List<MazePartConnector> pendingExits =  startModule.GetExits().ToList();
		
		//p/rint ("StartTag: " + startModule.tag);
		//print ("Start Tags lenght: " + startModule.Tags.Length);
		//print ("Pending exits size: " + pendingExits.ToArray().Length);
		for( int iteration = 0; iteration < Iterations; iteration++ )
		{
			print ("loop, iteration: " + iteration);
			List<MazePartConnector> newExits = new List<MazePartConnector>();
			
			foreach( MazePartConnector pendingExit in pendingExits )
			{
				//print ("Pending exit parent tag: " + pendingExit.transform.parent.gameObject.tag);
				//print ("Pending exits lenght: " + pendingExits.ToArray().Length);
				string newTag = GetRandom<string>( pendingExit.Tags );
				//print ("Newtag: " + newTag);
				MazePart newModulePrefab = GetRandomWithTag( Modules.ToList(), newTag );
				MazePart newModule = (MazePart)Instantiate( newModulePrefab);
				MazePartConnector[] newModuleExits = newModule.GetExits();
				//print ("New Part tag: " + newModule.tag);
				//print ("New Part Tags lenght: " + newModule.Tags.Length);
				MazePartConnector exitToMatch = newModuleExits.FirstOrDefault( x => x.IsDefault ) ?? GetRandom( newModuleExits );
				Vector3 correctiveRotation = exitToMatch.transform.rotation.eulerAngles - pendingExit.transform.rotation.eulerAngles;

				print ("BEFORE: exitToMatch.transform.localPosition " + exitToMatch.transform.localPosition);
				print ("BEFORE: exitToMatch.transform.position " + exitToMatch.transform.position);
				print ("BEFORE: pendingExit.transform.localPosition " + pendingExit.transform.localPosition);
				print ("BEFORE: pendingExit.transform.position " + pendingExit.transform.position);
				print ("BEFORE: newModule.transform.localPosition " + newModule.transform.localPosition);
				print ("BEFORE: newModule.transform.position " + newModule.transform.position);
				print ("BEFORE: pendingExit.transform.parent.position  " + pendingExit.transform.parent.position );
				print ("BEFORE: pendingExit.transform.parent.localPosition  " + pendingExit.transform.parent.localPosition );

				//newModule.transform.Rotate(correctiveRotation);
				//exitToMatch.transform.localPosition = pendingExit.transform.localPosition;
				newModule.transform.position = pendingExit.transform.position + (pendingExit.transform.position - pendingExit.transform.position);

				print ("AFTER: exitToMatch.transform.localPosition " + exitToMatch.transform.localPosition);
				print ("AFTER: exitToMatch.transform.position " + exitToMatch.transform.position);
				print ("AFTER: pendingExit.transform.localPosition " + pendingExit.transform.localPosition);
				print ("AFTER: pendingExit.transform.position " + pendingExit.transform.position);
				print ("AFTER: newModule.transform.localPosition " + newModule.transform.localPosition);
				print ("AFTER: newModule.transform.position " + newModule.transform.position);
				print ("AFTER: pendingExit.transform.parent.position  " + pendingExit.transform.parent.position );
				print ("AFTER: pendingExit.transform.parent.localPosition  " + pendingExit.transform.parent.localPosition );
				newModule.transform.position += RotatePointAroundPivot(newModule.transform.position, pendingExit.transform.localPosition, Quaternion.Euler(correctiveRotation));

				print ("POST: exitToMatch.transform.localPosition " + exitToMatch.transform.localPosition);
				print ("POST: exitToMatch.transform.position " + exitToMatch.transform.position);
				print ("POST: pendingExit.transform.localPosition " + pendingExit.transform.localPosition);
				print ("POST: pendingExit.transform.position " + pendingExit.transform.position);
				print ("POST: newModule.transform.localPosition " + newModule.transform.localPosition);
				print ("POST: newModule.transform.position " + newModule.transform.position);
				print ("POST: pendingExit.transform.parent.position  " + pendingExit.transform.parent.position );
				print ("POST: pendingExit.transform.parent.localPosition  " + pendingExit.transform.parent.localPosition );

				//newModule.transform.position += exitToMatch.transform.localPosition;
				// MatchExits( pendingExit, exitToMatch );
				newExits.AddRange( newModuleExits.Where( e => e != exitToMatch ) );
			}
			pendingExits = newExits.ToList();
		}
	}
	
	
	private void MatchExits( MazePartConnector oldExit, MazePartConnector newExit )
	{
		var newModule = newExit.transform.parent;
		var forwardVectorToMatch = oldExit.transform.position;
		var correctiveTranslation = newExit.transform.position - newModule.transform.position;
		var correctiveRotation = newExit.transform.rotation.eulerAngles - oldExit.transform.rotation.eulerAngles;
		print ("New Exit pos pre:" + newExit.transform.position);
		newExit.transform.position = oldExit.transform.position;
		print ("Old Exit pos:" + oldExit.transform.position);
		print ("New Exit pos post:" + newExit.transform.position);
		//newModule.transform.position += oldExit.transform.position;
		//newModule.transform.position += correctiveTranslation;
		Quaternion rot = Quaternion.FromToRotation(newExit.transform.position, -oldExit.transform.position);
		//RotatePointAroundPivot (newExit.transform.position, newExit.transform.position, rot);
	}
	
	
	private static TItem GetRandom<TItem>( TItem[] array )
	{
		int index = Random.Range (0, array.Length);
		//print ("Array Lenght in GetRandom:" + array.Length);
		//print ("Array Index in GetRandom:" + index);
		return array[index];
	}
	
	
	private static MazePart GetRandomWithTag( List<MazePart> modules, string tagToMatch )
	{
		MazePart[] matchingModules = modules.Where( m => m.Tags.Contains( tagToMatch ) ).ToArray();
		print ("Array Lenght in GetRandomWithTag:" + modules.ToArray().Length);
		print ("Array Index in GetRandomWithTag:" + matchingModules.Length);
		print ("Tag to match in GetRandomWithTag:" + tagToMatch);
		return GetRandom<MazePart>( matchingModules );
	}
	
	
	private static float Azimuth(Vector3 vector)
	{
		return Vector3.Angle(Vector3.forward, vector) * Mathf.Sign(vector.x);
	}
	
	private Vector3 RotatePointAroundPivot( Vector3 point,  Vector3 pivot, Quaternion angles)
	{
		Vector3 dir = point - pivot; // get point direction relative to pivot
		dir = angles * dir; // rotate it
		point = dir + pivot; // calculate rotated point
		return point; // return it
	}
}
