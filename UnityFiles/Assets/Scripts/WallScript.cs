using UnityEngine;
using System.Collections;

public class WallScript : MonoBehaviour {
	
	//Array locations
	int row;
	int col;
	float maxHeight;
	float minHeight;
	
	//Height
	float yLoc; // must be 0.0-1.0.
	
	// Use this for initialization
	void Start () {
		
	}
	
	public void Setup(int r, int c, float maxHeight, float minHeight)
	{
		row = r;
		col = c;
		this.maxHeight = maxHeight;
		this.minHeight = minHeight;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
