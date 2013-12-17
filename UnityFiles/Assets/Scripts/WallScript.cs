using UnityEngine;
using System.Collections;

public class WallScript : MonoBehaviour {
        
    //Array locations
    public int row;
    public int col;
    float maxHeight;
    float minHeight;
    
    //self
    GameObject thisGO;
    Transform trans;
    
    //Height
    float yLoc; // must be 0.0-1.0.
    float currYLoc;
	float motion; // wall moving? Positive = Upwards / Negative = downwards     
	bool locked;

    // Use this for initialization
    void Start () {
		motion = 0;
		locked = false;
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
	
	public void setLockState(bool state)
	{
		locked = state;	
	}
	
	public bool getLockState(){return locked;}
	
    public int getRow(){return row;}
    
    public int getCol(){return col;}
    
    public void SetSize(float val)
    {
            trans.localScale = new Vector3(val,val,val);
    }
	
	public void modifyMotion(float val)
	{
		motion += val;
	}
    
    // Update is called once per frame
    void Update () 
	{
		if(!locked)
		{
			//modify the yLoc by motion. and keep in bounds.
	        yLoc += motion;
			if(yLoc > 1.0)
				yLoc = 1.0f;
			else if(yLoc < 0.0)
				yLoc = 0.0f;
			
			
			//Prevents jittering
	        if(Mathf.Abs (currYLoc-yLoc) > .05f)
			{
	                if (currYLoc > yLoc)
	                {
	                        currYLoc -= .05f;
	                }else{
	                        currYLoc += .05f;
	                }
	        }
			else
			{
				//the diff between currYLoc and yloc are 0,
				//is yLoc between 0.0 and 1.0 and not moving?
				//if so, move towards 0.0 or 1.0 slowly.
				if(yLoc > 0.0f && yLoc < 1.0f && motion == 0)
				{
					if (yLoc > .5)
					{
						yLoc += .01f;
					}
					else // yLoc < .5
		 			{
						yLoc -= .01f;
					}
				}
			}
	        
	        //Calculates the height based on currYLoc. //PROBABLY CAUSING 0-1 ISSUE
	        float heightMod = maxHeight - minHeight;
	        heightMod = heightMod*currYLoc;
	        
	        float yPos = heightMod + minHeight;
	        float xPos = trans.position.x;
	        float zPos = trans.position.z;
	        
	        trans.position = new Vector3(xPos,yPos,zPos);
		}
    }
	
	public void setLockedStateFIX()
	{
		currYLoc = 1; // down state? the numbers got mixed up somewhere.
		float heightMod = maxHeight - minHeight;
        heightMod = heightMod*currYLoc;
        
        float yPos = heightMod + minHeight;
        float xPos = trans.position.x;
        float zPos = trans.position.z;
        
        trans.position = new Vector3(xPos,yPos,zPos);
	}
}