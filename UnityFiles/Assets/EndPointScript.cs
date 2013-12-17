using UnityEngine;
using System.Collections;

public class EndPointScript : MonoBehaviour {
	
	public int artifactsGathered;
	
	private GameObject[] otherPlayers;
	private Vector3 startPos;
	
	// Use this for initialization
	void Start () {
		startPos = transform.position; 
	}
	
	// Update is called once per frame
	void Update () {
	
		
		float averageX = 0; float averageZ = 0;
		int count = 0;
			
		otherPlayers = GameObject.FindGameObjectsWithTag("mage");
		foreach(GameObject player in otherPlayers)
		{
			averageX += player.transform.position.x;
			averageZ += player.transform.position.z;
			count++;
		}
		
		
		//Determining whether or not the endzone should appear.
		if(count > 0 && artifactsGathered == count)
		{
			averageX = averageX/count;
			averageZ = averageZ/count;
		
			transform.position = new Vector3(averageX,0,averageZ);
		}
		
		else
		{
			transform.position = startPos;	
		}
		
		
	}
}
