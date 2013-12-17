using UnityEngine;
using System.Collections;

public class BeaconScript : MonoBehaviour {
	
	public bool gottenArtifact;
	private GameObject[] otherPlayers;
	
	// Use this for initialization
	void Start () {
		gottenArtifact = false;
	}
	
	// Update is called once per frame
	void Update () {
		
		//Artifact hasn't been picked up. locate it.
		if(!gottenArtifact)
		{
	
			GameObject myArtifact = GameObject.Find("myArtifact");
			
			//If for some reason myartifact hasn't been spawned yet, simply end update.
			if(!myArtifact)
				return;
			
			transform.position = myArtifact.transform.position;
		}
		
		//Artifact has been picked up. Find other players.
		else
		{
			float averageX = 0; float averageZ = 0;
			int count = 0;
			
			otherPlayers = GameObject.FindGameObjectsWithTag("mage");
			foreach(GameObject player in otherPlayers)
			{
				averageX += player.transform.position.x;
				averageZ += player.transform.position.z;
				count++;
			}
			
			averageX = averageX/count;
			averageZ = averageZ/count;
			
			transform.position = new Vector3(averageX,0,averageZ);
		}
		
	}
}
