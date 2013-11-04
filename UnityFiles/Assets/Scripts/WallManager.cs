using UnityEngine;
using System.Collections;

public class WallManager: MonoBehaviour {
	
	//public variables
	public float mapSize;
	public float scale;
	public GameObject XWall;
	public GameObject ZWall;
	public GameObject Pillar;
	
	//private variables
	GameObject[][] pillarArray;
	GameObject[][] XWallArray;
	GameObject[][] ZWallArray;
	float[][] XWallState; //value is 0.0 - 1.0
	float[][] ZWallState; //0 being down, 1 being entirely raised.
 	
	int numPieces;
	
	
	// Use this for initialization
	void Start () {
		
		numPieces = (int)(mapSize/scale) - 1;
		
		//Creating pillars
		pillarArray = new GameObject[numPieces][];
		for(int i = 0; i < numPieces ; i++)
		{
			pillarArray[i] = new GameObject[numPieces];
			
			for(int j = 0; j < numPieces; j++)
			{
				pillarArray[i][j]  = (GameObject)Object.Instantiate(Pillar, new Vector3(i*scale - (mapSize/2) + scale , 1, j*scale -(mapSize/2) + scale),Quaternion.identity);
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
				XWallArray[i][j]  = (GameObject)Object.Instantiate(XWall, new Vector3(i*scale - (mapSize/2) + .5f * scale , 1, j*scale -(mapSize/2) + scale),Quaternion.identity);
				XWallState[i][j] = 1.0f;
				
				XWallArray[i][j].GetComponent<WallScript>().Setup(i,j,1.0f,-.75f); //gives the wall info about itself, like its location, max/min height.
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
				ZWallArray[i][j] = (GameObject)Object.Instantiate(ZWall, new Vector3(i*scale - (mapSize/2) + scale, 1, j*scale -(mapSize/2) + .5f * scale ),Quaternion.identity);
				ZWallState[i][j] = 1.0f; 
				
				ZWallArray[i][j].GetComponent<WallScript>().Setup(i,j,1.0f,-.75f); //gives the wall info about itself, like its location, max/min height.
			}
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
