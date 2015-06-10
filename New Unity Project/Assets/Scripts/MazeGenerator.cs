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

		print ("StartTag: " + startModule.tag);
		print ("Start Tags lenght: " + startModule.Tags.Length);
		print ("Pending exits size: " + pendingExits.ToArray().Length);
        for( int iteration = 0; iteration < Iterations; iteration++ )
        {
			print ("loop, iteration: " + iteration);
			List<MazePartConnector> newExits = new List<MazePartConnector>();

			foreach( MazePartConnector pendingExit in pendingExits )
            {
				print ("Pending exit parent tag: " + pendingExit.transform.parent.gameObject.tag);
				print ("Pending exits lenght: " + pendingExits.ToArray().Length);
                string newTag = GetRandom<string>( pendingExit.Tags );
				print ("Newtag: " + newTag);
				MazePart newModulePrefab = GetRandomWithTag( Modules.ToList(), newTag );
				MazePart newModule = (MazePart)Instantiate( newModulePrefab);
				MazePartConnector[] newModuleExits = newModule.GetExits();
				print ("New Part tag: " + newModule.tag);
				print ("New Part Tags lenght: " + newModule.Tags.Length);
				MazePartConnector exitToMatch = newModuleExits.FirstOrDefault( x => x.IsDefault ) ?? GetRandom( newModuleExits );
				newModule.transform.position = pendingExit.transform.parent.transform.position + (2*(pendingExit.transform.parent.transform.position - pendingExit.transform.position));
				newModule.transform.rotation = pendingExit.transform.rotation;
               // MatchExits( pendingExit, exitToMatch );
                newExits.AddRange( newModuleExits.Where( e => e != exitToMatch ) );
            }

            pendingExits = newExits.ToList();
        }
    }


    private void MatchExits( MazePartConnector oldExit, MazePartConnector newExit )
    {
		var newModule = newExit.transform.parent;
		var forwardVectorToMatch = -oldExit.transform.forward;
		var correctiveRotation = Azimuth(forwardVectorToMatch) - Azimuth(newExit.transform.forward);
		newModule.RotateAround(newExit.transform.position, Vector3.up, correctiveRotation);
		var correctiveTranslation = oldExit.transform.position - newExit.transform.position;
		newModule.transform.position += correctiveTranslation;

    }


    private static TItem GetRandom<TItem>( TItem[] array )
    {
		int index = Random.Range (0, array.Length);
		print ("Array Lenght in GetRandom:" + array.Length);
		print ("Array Index in GetRandom:" + index);
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
