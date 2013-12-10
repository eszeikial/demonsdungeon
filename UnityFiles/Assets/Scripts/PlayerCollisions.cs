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
	}
}
