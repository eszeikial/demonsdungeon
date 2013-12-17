using UnityEngine;
using System.Collections;

public class PlayerCollisions : MonoBehaviour {
	
	void OnTriggerEnter(Collider c)
	{
		Debug.Log("Player collided with" + c.gameObject.tag);
		if(c.gameObject.tag == "skull")
		{
			Debug.Log("Mage dead");
			
			Network.CloseConnection(networkView.owner, false);
		}
		
		if(c.gameObject.tag == "book" && c.gameObject.name == "myArtifact")
		{
			Debug.Log("book picked up");
			
			GameObject.Find("EndZone").GetComponent<EndPointScript>().artifactsGathered++;			
			GameObject.Find ("Beacon").GetComponent<BeaconScript>().gottenArtifact = true;
			Network.Destroy(c.gameObject);
			
		}
		
		if(c.gameObject.tag == "endZone")
		{
			Debug.Log(gameObject + " Has entered the endZone!");
		}
	}
	
	void OnTriggerExit(Collider c)
	{
		if(c.gameObject.tag == "endZone")
		{
			Debug.Log(gameObject + " Has left the endZone!");
		}
	}
}
