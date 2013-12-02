﻿using UnityEngine;
using System.Collections;

public class WallManager: MonoBehaviour {
	
	
	/* //OLD CODE
	//public variables
	public float mapSize;
	public float scale;
	public GameObject XWall;
	public GameObject ZWall;
	public GameObject Pillar;
	
	public GameObject WallGO;
	
	//private variables
	GameObject[][] pillarArray;
	GameObject[][] XWallArray;
	GameObject[][] ZWallArray;
	float[][] XWallState; //value is 0.0 - 1.0
	float[][] ZWallState; //0 being down, 1 being entirely raised.
	int numPieces;
	GameObject parent;
	
	//randomizer
	System.Random rng;
	
	/// <summary>
	/// Creates the WallManager instance.
	/// The maze objects are all created and added to the heiarchy neatly.
	/// The walls are generated with a yLoc value of either 0 or 1, up or down.
	/// </summary>
	void Start () {
		rng = new System.Random();
		parent = GameObject.Find("WallManagerGO");
		numPieces = (int)(mapSize/scale) - 1;
		
		//Creating pillars
		pillarArray = new GameObject[numPieces][];
		for(int i = 0; i < numPieces ; i++)
		{
			pillarArray[i] = new GameObject[numPieces];
			
			for(int j = 0; j < numPieces; j++)
			{
				pillarArray[i][j]  = (GameObject)Object.Instantiate
					(Pillar, new Vector3(i*scale - (mapSize/2) + scale , 1, j*scale -(mapSize/2) + scale),Quaternion.identity);
				pillarArray[i][j].transform.parent = parent.transform; //sets the GO neatly in heiarchy. 
			}
		}
		
		//Creating XWalls and data
		XWallArray = new GameObject[numPieces+1][];
		XWallState = new float[numPieces+1][];
		for(int i = 0; i < numPieces+1; i++)
		{
			XWallArray[i] = new GameObject[numPieces];
			XWallState[i] = new float[numPieces];
			
			for(int j = 0; j < numPieces; j++)
			{
				int rnd = rng.Next()%2; // Randomly decide if wall is raised or not.
				
				XWallArray[i][j]  = (GameObject)Object.Instantiate
					(XWall, new Vector3(i*scale - (mapSize/2) + .5f * scale , 1, j*scale -(mapSize/2) + scale),Quaternion.identity);
				XWallState[i][j] = 1.0f;
				
				XWallArray[i][j].transform.parent = parent.transform; //sets the GO neatly in heiarchy. 
				XWallArray[i][j].GetComponent<WallScript>().Setup(i,j,.75f,-1.75f); //gives the wall info about itself, like its location, max/min height.
				XWallArray[i][j].GetComponent<WallScript>().SetParent(XWallArray[i][j]); // lets the object know about itself.
				XWallArray[i][j].GetComponent<WallScript>().SetHeight((float)rnd); // sets height.
			}
		}
		
		//Creating ZWalls and data
		ZWallArray = new GameObject[numPieces][];
		ZWallState = new float[numPieces][];
		for(int i = 0; i < numPieces; i++)
		{
			ZWallArray[i] = new GameObject[numPieces+1];
			ZWallState[i] = new float[numPieces+1];
			for(int j = 0; j < numPieces+1; j++)
			{
				int rnd = rng.Next()%2; // Randomly decide if wall is raised or not.
				
				ZWallArray[i][j] = (GameObject)Object.Instantiate
					(ZWall, new Vector3(i*scale - (mapSize/2) + scale, 1, j*scale -(mapSize/2) + .5f * scale ),Quaternion.identity);
				ZWallState[i][j] = 1.0f; 
				
				ZWallArray[i][j].transform.parent = parent.transform; //sets the GO neatly in heiarchy. 
				ZWallArray[i][j].GetComponent<WallScript>().Setup(i,j,.75f,-1.75f); //gives the wall info about itself, like its location, max/min height.
				ZWallArray[i][j].GetComponent<WallScript>().SetParent(ZWallArray[i][j]); // lets the object know about itself.
				ZWallArray[i][j].GetComponent<WallScript>().SetHeight((float)rnd); // sets height.
			}
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		
		//Every 5 frames randomly move a wall.
		if(Time.frameCount %5 == 0)
		{
			randomlyMoveWall();
		}
	}
	
	private void randomlyMoveWall()
	{
		//Pick a random wall and move it
		int wallNum = rng.Next()%380;
		int wallX = wallNum/20; // number between 0-18
		int wallY = wallNum%20; // number between 0-19
		WallScript ws;
		
		
		if(rng.Next()%2 == 0)
		{
			//Use xWalls	
			ws = XWallArray[wallY][wallX].GetComponent<WallScript>();
		}else{
			//Use zWalls
			ws = ZWallArray[wallX][wallY].GetComponent<WallScript>();
		}
		
		if(ws.GetHeight() == 1.0f)
		{
			ws.SetHeight(0.0f);	
		}
		else if(ws.GetHeight() == 0.0f)
		{
			ws.SetHeight(1.0f);	
		}
	}
	*/
	
	//Public Vars. These are modified in the unity editor.
	public GameObject WallGO;
	public int mazeSize; // Number of blocks to create on X and Z axis.
	public float wallSize; // How large should the cubes be?
	
	//Private Vars
	private GameObject[][] WallArray; // contains the actual walls.
 	private float[][] wallState; // Value can be 0.0-1.0. 0 being all the way to the ground, 1 being fully raised.
	private System.Random rng;
	
	private GameObject parent;
	
	public void MakeWalls() //change this later to something like MakeWalls() so the walls can be made when the server starts
	{
		rng = new System.Random();
		parent = GameObject.Find("WallManagerGO");
		
		//Wall generation
		WallArray = new GameObject[mazeSize][];
		wallState = new float[mazeSize][];
		for(int i = 0; i < mazeSize ; i++)
		{
			WallArray[i] = new GameObject[mazeSize];
			wallState[i] = new float[mazeSize];
			
			for(int j = 0; j < mazeSize; j++)
			{
				WallArray[i][j]  = (GameObject)Object.Instantiate
					(WallGO, new Vector3(i*wallSize,wallSize/2,j*wallSize),Quaternion.identity);
				WallArray[i][j].transform.parent = parent.transform; //sets the GO neatly in heiarchy. 
				
				//Decide if wall is raised or not.
				int rnd = rng.Next()%2;
				wallState[i][j] = (float)rnd;
				
				//Editing the script component
				WallScript currentWall = WallArray[i][j].GetComponent<WallScript>();
				currentWall.Setup(i,j,-wallSize/2,wallSize/2); //gives the wall info about itself, like its location, max/min height.
				currentWall.SetParent(WallArray[i][j]); // lets the object know about itself.
				currentWall.SetHeight((float)rnd); // sets height.
				currentWall.SetSize(wallSize); // sets the cube's size.
			}
		}
		
		
		//Outside wall Gen.
		GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
		GameObject zMaxWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
		GameObject zMinWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
		GameObject xMaxWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
		GameObject xMinWall = GameObject.CreatePrimitive(PrimitiveType.Cube);
		
		float fullDist = (mazeSize * wallSize);
		float halfDist = (fullDist) /2;
		float permWallHeight = wallSize*2;
		
		//floor - Might not even be necessary? - Wait yes it is. - WAIT NO ITS NOT.
		floor.transform.position = new Vector3(halfDist,-.5f,halfDist);
		floor.transform.localScale = new Vector3(fullDist,1,fullDist);
		
		//other walls - should be working!
		zMaxWall.transform.position = new Vector3(halfDist,permWallHeight/2,fullDist);
		zMaxWall.transform.localScale = new Vector3(fullDist+wallSize,permWallHeight,wallSize);
		
		zMinWall.transform.position = new Vector3(halfDist,permWallHeight/2,-wallSize);
		zMinWall.transform.localScale = new Vector3(fullDist+wallSize,permWallHeight,wallSize);
		
		xMaxWall.transform.position = new Vector3(fullDist,permWallHeight/2,halfDist);
		xMaxWall.transform.localScale = new Vector3(wallSize,permWallHeight,fullDist+wallSize);
		
		xMinWall.transform.position = new Vector3(-wallSize,permWallHeight/2,halfDist);
		xMinWall.transform.localScale = new Vector3(wallSize,permWallHeight,fullDist+wallSize);
		
		
		
	}
	
	
}
