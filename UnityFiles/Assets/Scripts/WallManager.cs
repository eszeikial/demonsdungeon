using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WallManager: MonoBehaviour {
        
    struct space{
    public int row;
    public int col;
    public int weight;
    public int numAdjacentInPath;
    public bool partOfPath;
    };
    
    //Public Vars. These are modified in the unity editor.
    public GameObject WallGO;
    public int mazeSize; // Number of blocks to create on X and Z axis.
    public float wallSize; // How large should the cubes be?
    
    //Private Vars
    public GameObject[][] WallArray; // contains the actual walls.
     private float[][] wallState; // Value can be 0.0-1.0. 0 being all the way to the ground, 1 being fully raised.
    private System.Random rng;
    private GameObject parent;
    bool requestingData = false;
    
    void Start()
	{
		
        rng = new System.Random();
        parent = GameObject.Find("WallManagerGO");
        
        //------------------------------------------//
        //-------------Wall generation--------------//
        //------------------------------------------//
        
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
                        
                        
                        //Editing the script component
                        WallScript currentWall = WallArray[i][j].GetComponent<WallScript>();
                        currentWall.Setup(i,j,-wallSize/2,wallSize/2); //gives the wall info about itself, like its location, max/min height.
                        currentWall.SetParent(WallArray[i][j]); // lets the object know about itself.
                        currentWall.SetHeight(1); //All walls start raised.
                        wallState[i][j] = 1; // keep track of the value in wallState.
                        currentWall.SetSize(wallSize); // sets the cube's size.
                }
        }
	
		
		//---------------------------------------//
        //-------------Outside Walls-------------//
        //---------------------------------------//
        
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
        zMaxWall.name = "zMaxWall";
        
        zMinWall.transform.position = new Vector3(halfDist,permWallHeight/2,-wallSize);
        zMinWall.transform.localScale = new Vector3(fullDist+wallSize,permWallHeight,wallSize);
        zMinWall.name = "zMinWall";
        
        xMaxWall.transform.position = new Vector3(fullDist,permWallHeight/2,halfDist);
        xMaxWall.transform.localScale = new Vector3(wallSize,permWallHeight,fullDist+wallSize);
        xMaxWall.name = "xMaxWall";
        
        xMinWall.transform.position = new Vector3(-wallSize,permWallHeight/2,halfDist);
        xMinWall.transform.localScale = new Vector3(wallSize,permWallHeight,fullDist+wallSize);
        xMinWall.name = "xMinWall";
	}  

	void OnServerInitialized()
	{
	    //---------------------------------------//
        //-----------------Prims-----------------//
        //---------------------------------------//
        
        //Everything in here is only done by the server.
        
       
        
        //Having some problems, this was to work-around some dumb error.
        space fakeSpace;
        fakeSpace.col = 1000;
        fakeSpace.row = 1000;
        fakeSpace.weight = 1000000;
        fakeSpace.numAdjacentInPath = 0;
        fakeSpace.partOfPath = false;
        
        //all spaces and possible starting Loc for algorithm.
        space[][] allSpaces;
        List<space> possibleStarts = new List<space>(); // List containing all the nodes that can be started from.
        
        
        //create a randomly generated weighted graph.
        allSpaces = new space[mazeSize][];
        for(int i = 0; i < mazeSize; i++)
        {
                allSpaces[i] = new space[mazeSize];
                for(int j = 0; j < mazeSize; j++)
                {
                        allSpaces[i][j].weight = rng.Next()%100;
                        allSpaces[i][j].numAdjacentInPath = 0;
                        allSpaces[i][j].partOfPath = false;
                        allSpaces[i][j].row = i;
                        allSpaces[i][j].col = j;
                }
        }
        
        //Generation Loop.
        bool isGenerated = false;
        int numRuns = 0;
        int curRow = 0;
        int curCol = 0;
        
        List<space> avaliableMoves = new List<space>(); //list for possible moves THIS turn. (used later).
        
        //Kinda cheating I guess. First move is made automatically.
        //Add the starting move to the list of starting moves.
        possibleStarts.Add(allSpaces[0][0]);

        allSpaces[0][0].partOfPath = true;
        allSpaces[0][1].numAdjacentInPath++;
        allSpaces[1][0].numAdjacentInPath++;
        
        while(!isGenerated)
        {
                numRuns++; //Debug
                
                //Debug.Log("running loop number " + numRuns );+
                //Debug.Log("++++++ running loop number " + numRuns + ", avaliable starts: " + possibleStarts.Count+" ++++++");
                
                
                //Choose a starting space.
                int choice = rng.Next()%possibleStarts.Count;
                curRow = possibleStarts[choice].row;
                curCol = possibleStarts[choice].col;
                
                
                //Debug.Log("starting at row "+ curRow + ", col "+ curCol);
                
                //Add all possible points to path.
                for(int i = 1; i < 5; i++) // this for loop gets all nearby points.
                {
                        int row = 0;
                        int col = 0;
                        
                        //Sloppy I know.
                        switch(i)
                        {
                        case 1: row = curRow; col = curCol + 1; break;
                        case 2: row = curRow; col = curCol - 1; break;
                        case 3: row = curRow + 1; col = curCol; break;
                        case 4: row = curRow - 1; col = curCol; break;
                        }
                        
                        if(row >= 0 && row < mazeSize && col >= 0 && col < mazeSize)//make sure its in bounds.
                        {
                                if(allSpaces[row][col].numAdjacentInPath == 1 && !allSpaces[row][col].partOfPath) //This means it is near only one other used space! (which is what we want)
                                {
                                        //Debug.Log("adding space at row" + row + ", col "+ col + "to possible moves.");
                                        avaliableMoves.Add(allSpaces[row][col]); // adds the space onto a list of possible spaces
                                }
                        }
                        
                }//All possible moves have been added.
                
                
                //dumb error work-around.
                space moveChoice = fakeSpace;
                
                //If there are moves avaliable...
                if(avaliableMoves.Count != 0)
                {
                        //Debug.Log ("There are moves avaliable");
                        //choose one by weight and make the move.
                        for(int i = 0; i < avaliableMoves.Count; i++) // count should be 1-3
                        {
                                if(avaliableMoves[i].weight < moveChoice.weight)
                                {
                                        moveChoice = avaliableMoves[i];
                                }
                        }
                        
                        //Debug.Log("chose to move to space at row "+moveChoice.row + "and col "+moveChoice.col + ", added it to graph");
                        //Move has been chosen. Add it to graph.
                        possibleStarts.Add(moveChoice);
                        
                        
                        for(int i = 1; i < 5; i++) // this for loop gets all nearby points.
                        {
                                
                                int r = moveChoice.row;
                                int c = moveChoice.col;
                                allSpaces[r][c].partOfPath = true; // move choice becomes part of graph
                                
                                switch(i)
                                {
                                case 1: r++; break;
                                case 2: r--; break;
                                case 3: c++; break;
                                case 4: c--; break;
                                }
                                
                                //Not going out of bounds...
                                if(c>=0 && c<mazeSize && r>=0 && r<mazeSize)
                                {
                                        allSpaces[r][c].numAdjacentInPath++; // spaces near choice have numAdjacent incremented.
                                }
                                
                        }
                        
                        if((avaliableMoves.Count-1) == 0)
                        {
                                //Debug.Log("after moving there were no more moves, removing row " + curRow +", col "+curCol +" from possible starts");
                                //that was our last move from this spot.        
                                //Remove used space from starting list.
                                possibleStarts.RemoveAt(choice);
                        }
                }
                else // No moves?
                {
                        //Debug.Log("no moves, removing space at row "+curRow + ",col " + curCol+ " from possible starts");
                        //Remove used space from starting list.
                        possibleStarts.RemoveAt(choice);
                }
                
                
                //If there are no more moves, end generation.
                if(possibleStarts.Count == 0) //possibleStarts.Count == 0
                {
                        //GGWP generation
                        //Debug.Log("finished with " + numRuns + "runs!");
                        isGenerated = true; // ends loop.
                }
                
                avaliableMoves.Clear();
                
        }//End while. At this point, all spaces that are used have the "partofpath" bool set to true.
        
        
        
        foreach(space[] ss in allSpaces)
        {
                foreach(space s in ss)
                {
                        if(s.partOfPath)
                        {
                                WallScript ws = WallArray[s.row][s.col].GetComponent<WallScript>();
                                wallState[s.row][s.col] = 1; // keep track of state.
                                ws.SetHeight(1); //Set wall down.
                        }
                        else{
                                WallScript ws = WallArray[s.row][s.col].GetComponent<WallScript>();
                                wallState[s.row][s.col] = 0; // keep track of state.
                                ws.SetHeight(0); //Set wall down.
                        }
                }
        }

	}
     
    
    void Update()
    {
            if(Network.isServer)
            {
                    //Every X frames 
                    if(Time.frameCount%15 == 0)
                    {
                            //pick a random wall
                            int r = rng.Next()%mazeSize;
                            int c = rng.Next()%mazeSize;
                            
                            //get the wallscript
                            WallScript ws = WallArray[r][c].GetComponent<WallScript>();
                            float height = ws.GetHeight();
                            
                            if(height == 1.0f)
                            {
                                    ws.SetHeight(0.0f);        
                                    //Debug.Log("raising wall at row " + r + ", col "+c);
                            }
                            else if(height == 0.0f)
                            {
                                    ws.SetHeight(1.0f);        
                                    //Debug.Log("lowering wall at row " + r + ", col "+c);
                            }        
                            
                            networkView.RPC("moveWall",RPCMode.Others,r,c,(int)ws.GetHeight()); 
                    }
            }
    }
    
    void OnConnectedToServer()
    {
            Debug.Log("I am a client and have connected to the server.");        
            requestingData = true;
            networkView.RPC("requestData",RPCMode.Server); //send a data request to server.
    }
    
    void OnPlayerConnected()
    {
            Debug.Log("I am the server and a client has just connected");        
    }
    
    void SendDataToPlayers()
    {
            string dataToSend = "";
            foreach(GameObject[] GOarray in WallArray)
            {
                    foreach(GameObject GO in GOarray)
                    {
                            WallScript ws = GO.GetComponent<WallScript>();
                            int r = ws.getRow();
                            int c = ws.getCol();
                            dataToSend += r+","+c+","+ws.GetHeight()+"|";
                    }
			
				 dataToSend = dataToSend.Substring(0,dataToSend.Length-1);
				 //Debug.Log("Sending data: "+ dataToSend);
            	 networkView.RPC("setMaze",RPCMode.Others,dataToSend);
				 dataToSend = "";
				
				 //Had to send the data at the end of every Row, turns out
				 //that 4080 something characters was the maximum string allowed
				 //to be sent through RPC calls.
		
            }
		
			networkView.RPC("setMaze",RPCMode.Others,"DONE");
    }
    
    
    //---------------------------------------//
    //-----------------RPC's-----------------//
    //---------------------------------------//
    
    [RPC]
    void setMaze(string data)
    {
		
		//Debug.Log ("setmaze called");
		
		//This will be sent by the server when it has sent all its data.
		if(data.Equals("DONE"))
		{
			requestingData = false;
		}
		else
		{
			
			//This prevents unnecessary calculations for players who already have map data.
	        if(requestingData)
	        {
				//Debug.Log("Recieving data: " + data);
			
	                string[] allDataSplit = data.Split('|');
	                foreach(string str in allDataSplit)
	                {
	                        string[] finalSplit = str.Split(','); //0 = row, 1 = col, 2 = raised/lowered.
	                        int r = int.Parse(finalSplit[0]);
	                        int c = int.Parse(finalSplit[1]);
	                        int val = int.Parse(finalSplit[2]);
						    
	                        
	                        WallScript ws =  WallArray[r][c].GetComponent<WallScript>();
	                        ws.SetHeight(val);
	                }
	        }	
		}
    }
    
    [RPC]
    void requestData()
    {
        SendDataToPlayers();        
    }
    
    
    
    [RPC]
    void moveWall(int r, int c, int val)
    {
		WallScript ws = WallArray[r][c].GetComponent<WallScript>();
		ws.SetHeight(val);
    }
}