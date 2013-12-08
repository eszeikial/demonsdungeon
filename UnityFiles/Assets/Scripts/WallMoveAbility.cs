using UnityEngine;
using System.Collections;

public class WallMoveAbility : MonoBehaviour {
	
	
	
	public string upKey; // keypress to raise wall.
	public string downKey; // keypress to lower wall.
	public float power;// amount of wall displacement.
	
	//Gotta get that wall manager.
	private WallManager wm;
	
	private int angleAllowance; // how far you from 0/90/180/270 you can look and still be looking at a wall.
	private bool keyPressed;
	private bool upPressed;
	private bool downPressed;
	
	//Temp cube data
	private WallScript ws;
	private int xPos;
	private int zPos;
	
	
	// Use this for initialization
	void Start () {
		//I'm dumb so I cheat
		string temp = upKey;
		upKey = downKey;
		downKey = temp;
		
		angleAllowance = 20;
		keyPressed = false;
		upPressed = false;
		downPressed = false;
		
		//Getting the wall manager script.
		wm = GameObject.Find("WallManagerGO").GetComponent<WallManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
		if(networkView.isMine)
		{
			//If no key was previously pressed...
			if(!keyPressed)
			{
				//On the frame that you first press a specific key...
				if(Input.GetKeyDown(upKey) || Input.GetKeyDown(downKey))
				{
					keyPressed = true;
					//Find which cube the player is currently on.
					xPos = (int)(transform.position.x + 2.5f) / 5;
					zPos = (int)(transform.position.z + 2.5f) / 5;
			
					//Debug.Log("I am currently standing on row " + xPos + ", col "+ zPos);
					
					//Find the direction that the player is facing.
					float yRot = transform.rotation.eulerAngles.y;
					
					if(yRot <= angleAllowance || yRot >= 360-angleAllowance) // Col++ or increasing Z
					{
						//Debug.Log("I am facing in the positive Z direction");
						zPos++;
					}
					else if(yRot <= 90 + angleAllowance && yRot >= 90-angleAllowance) // Row++ increasing X
					{
						//Debug.Log("I am facing in the positive X direction");
						xPos++;
					}
					else if(yRot <= 180 + angleAllowance && yRot >= 180-angleAllowance) // Col-- decreasing Z
					{
						//Debug.Log("I am facing in the negative Z direction");
						zPos--;
					}
					else if(yRot <= 270 + angleAllowance && yRot >= 270-angleAllowance) // Row-- decreasing X
					{
						//Debug.Log("I am facing in the negative X direction");
						xPos--;
					}
					else
					{
						//Debug.Log("Bad rotation");
					}
					
					
					if(xPos >= 0 && xPos < wm.mazeSize && zPos >= 0 && zPos < wm.mazeSize)
					{
						//Wall Exists! get its script
						ws = wm.WallArray[xPos][zPos].GetComponent<WallScript>();
						
						//Raising or lowering?
						if(Input.GetKeyDown(upKey))
						{
							networkView.RPC("modifyWallMotion",RPCMode.All,xPos,zPos,power);
							//ws.modifyMotion(power);
							upPressed = true;
						}
						else //input.getkeydown(downKey)
						{
							networkView.RPC("modifyWallMotion",RPCMode.All,xPos,zPos,-power);
							//ws.modifyMotion(-power);
							downPressed = true;
						}
					}
					else
					{
						Debug.Log("There is no wall to be raised/lowered there!");	
					}
					
				}//End if(Input.GetKeyDown(upKey) || Input.GetKeyDown(downKey))
				
			}//End if(!keyPressed)
			
			
			else // key was pressed previously.
			{
				//Debug.Log("key was pressed previously");
				
				//which key was pressed?	
				if(downPressed)
				{
					//Check that key for release
					if(Input.GetKeyUp(downKey))
					{
						//ws.modifyMotion(power);
						networkView.RPC("modifyWallMotion",RPCMode.All,xPos,zPos,power);
						downPressed = false;
						keyPressed = false;
					}
				}
				else if(upPressed)
				{
					//Check that key for release
					if(Input.GetKeyUp(upKey))
					{
						//ws.modifyMotion(-power);
						networkView.RPC("modifyWallMotion",RPCMode.All,xPos,zPos,-power);
						upPressed = false;
						keyPressed = false;
					}
				}
			}
		}
	}//End update.
	
	
	[RPC]
	void modifyWallMotion(int x, int z, float motionMod)
	{
		WallScript thisWall = wm.WallArray[x][z].GetComponent<WallScript>();
		thisWall.modifyMotion(motionMod);
	}
	
	
	
}
