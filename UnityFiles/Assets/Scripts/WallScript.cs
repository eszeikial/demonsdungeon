using UnityEngine;
using System.Collections;

public class WallScript : MonoBehaviour {
	
	//Array locations
	int row;
	int col;
	float maxHeight;
	float minHeight;
	
	//self
	GameObject thisGO;
	Transform trans;
	
	//Height
	float yLoc; // must be 0.0-1.0.
	float currYLoc;
	
	// Use this for initialization
	void Start () {
		
	}
	
	//Sets the initial values, is called right after being created by WallManager
	public void Setup(int r, int c, float maxHeight, float minHeight)
	{
		row = r;
		col = c;
		this.maxHeight = maxHeight;
		this.minHeight = minHeight;
	}
	
	//Sends the parent down as well.
	public void SetParent(GameObject go)
	{
		thisGO = go;
		trans = go.transform;
	}
	
	
	public void SetHeight(float val)
	{
		if(val >= 0.0f && val <= 1.0f)
			yLoc = val;
	}
	
	public float GetHeight()
	{
		return yLoc;	
	}
	
	public int getRow(){return row;}
	
	public int getCol(){return col;}
	
	public void SetSize(float val)
	{
		trans.localScale = new Vector3(val,val,val);
	}
	
	// Update is called once per frame
	void Update () {
		
		if(Mathf.Abs (currYLoc-yLoc) > .05f)
		{
			if (currYLoc > yLoc)
			{
				currYLoc -= .05f;
			}else{
				currYLoc += .05f;
			}
		}
		
		//Calculates the height based on currYLoc.
		float heightMod = maxHeight - minHeight;
		heightMod = heightMod*currYLoc;
		
		float yPos = heightMod + minHeight;
		float xPos = trans.position.x;
		float zPos = trans.position.z;
		
		trans.position = new Vector3(xPos,yPos,zPos);
	}
}
