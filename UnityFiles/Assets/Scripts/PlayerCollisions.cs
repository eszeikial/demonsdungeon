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
		
		if(c.gameObject.tag == "book")
		{
			Debug.Log("book picked up");
			
			GameObject.Find ("Beacon").GetComponent<BeaconScript>().gottenArtifact = true;
			Network.Destroy(c.gameObject);
			
		}
	}
}
