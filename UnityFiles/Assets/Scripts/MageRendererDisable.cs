using UnityEngine;
using System.Collections;

public class MageRendererDisable : MonoBehaviour {

	// Use this for initialization
	void Start () {
		if(networkView.isMine)
		{
			foreach( Renderer r in gameObject.GetComponentsInChildren<Renderer>())
			{
				if(!(r.gameObject.name == "staff" || r.gameObject.name == "stafforb"))
					r.enabled = false;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
