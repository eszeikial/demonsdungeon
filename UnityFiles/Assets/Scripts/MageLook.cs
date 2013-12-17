using UnityEngine;
using System.Collections;

public class MageLook : MonoBehaviour {
	
	public Transform target;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		if(!target)
		{
			return;
		}
		
		
		/*transform.position = new Vector3(target.position.x,
										 target.position.y + 1.8f,
										 target.position.z);*/
		
		float x, y, z, xRot, yRot, zRot;
		x = target.position.x;
		y = target.position.y + 1.8f;
		z = target.position.z;
		
		xRot = target.rotation.eulerAngles.x;
		yRot = target.rotation.eulerAngles.y;
		zRot = target.rotation.eulerAngles.z;
			
		
		float mouseLookHeight = Input.mousePosition.y/Screen.height;
		if(mouseLookHeight > .5f) // if height is above half screen height...
		{
			mouseLookHeight-= .5f;//mouselook is anywhere between 0.0 and 0.5 now.
			mouseLookHeight*= 60;//now its between 0.0 and 15.
			xRot -= mouseLookHeight;
		}
		
		transform.rotation = Quaternion.Euler(xRot,yRot,zRot);
		transform.position = new Vector3(x,y,z);
	
	}
}
