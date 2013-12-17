using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour
{
	//----------------- variables ------------------
	
	//network info
	private string connectionIP = "127.0.0.1";
	private int connectionPort = 25005;
	
	
	//player info
	private string playerName = "No Name";
	string tempName = "";
	private bool makeMeAClient = false;
	
	public string PlayerName { 
		get { return playerName;}
		set { playerName = value; }
	}
	
	private int numPlayersConnected = 0;
	
	//is the game going?
	private bool isGameActive = false;
	//private float gameStart;
	
	//connect window setup
	private Rect connectWindowRect;
	private int connectWindowWidth = 320;
	private int connectWindowHeight = 150;
	private int buttonHeight = 45;
	private int leftIndent;
	private int topIndent;
	private string titleMessage = "Connection Setup";

	//connected window setup
	private Rect infoWindowRect;
	private int infoWindowWidth = 180;
	private int infoWindowHeight = 60;

	public Material[] materials;	
	

	//----------------end variables -----------------

	void Start ()
	{
		//get locally stored name, if it exists
		playerName = PlayerPrefs.GetString("playerName");
		if (playerName == "" || playerName == null) 
		{
			//if it doesn't exist, use dummy name, later prompt for name
			playerName = "No Name";
		}
		
	}
	
	void Update()
	{
		if(!isGameActive)
		{
			//We want to make sure all participating players are connected before starting the game
			if(numPlayersConnected >= 4)
			{
				//call gameStart
				networkView.RPC("StartGame", RPCMode.AllBuffered); 
			}
		}
		
	}
	
	//check to see if the game is over
	bool isGameOver()
	{
		if(isGameActive)
		{
			//game can end by players collecting all the relics or the demon catching all the players
			
		}
		return false;
	}
	
	void startServer ()
	{
		Network.InitializeServer(32, connectionPort, Network.HavePublicAddress());
		Debug.Log("Starting the server");
	}
	
	//----------- handle incoming messages FROM THE SERVER ------------
	
	//-----these messages are sent to the server
	
	//we are informed that we were successful in initializing the server
	void OnServerInitialized ()
	{
		Debug.Log("Server is initialzed.");
		
		//Create Skull character
		GameObject spm = GameObject.Find("SpawnManagerGO");
		GameObject go = spm.GetComponent<SpawnScript>().SpawnServer();
		int matIndex = int.Parse(Network.player.ToString()) % 3; //increment through the materials
		Debug.Log("Material Index is " + matIndex);
		networkView.RPC("NewPlayer", RPCMode.AllBuffered, go.networkView.viewID, matIndex, playerName, Network.player); 
	}
	
	// we are informed that a player has just connected to us (the server)
	void OnPlayerConnected (NetworkPlayer player)
	{
		Debug.Log ("Player " + player + " connected from " + player.ipAddress + ":" + player.port);
		//GameObject.Find("WallManagerGO").GetComponent<WallManager>().MakeWalls(); //do this for now so we can try to display multiplayer
		//MakePlayer();
	}
	
	///-----these messages are sent to the CLIENT
	
	void OnConnectedToServer ()
	{
		Debug.Log ("I'm a client, connected to server");
		
		//Create player character
		GameObject spm = GameObject.Find("SpawnManagerGO");
		GameObject go = spm.GetComponent<SpawnScript>().SpawnPlayer();
		int matIndex = int.Parse(Network.player.ToString()) % 3; //increment through the materials
		Debug.Log("Material Index is " + matIndex);
		networkView.RPC("NewPlayer", RPCMode.AllBuffered, go.networkView.viewID, matIndex, playerName, Network.player); 
	}
	
	//called on both client AND server
	void OnDisconnectedFromServer (NetworkDisconnection info)
	{
		//reload the application so we can start over
		Application.LoadLevel (Application.loadedLevel);	
	}
	
	//some player has disconnected. 
	//We'd better clean up their stuff
	void OnPlayerDisconnected (NetworkPlayer player)
	{
		Debug.Log ("Clean up after player " + player);
		Network.RemoveRPCs (player);
		Network.DestroyPlayerObjects (player);
	}
	
	//----------- end incoming messages from SERVER ------------
	
	
	
	void ServerConnectWindow (int windowID)
	{
		//leave a gap after the header
		GUILayout.Space(15);
		
		//display user name
		GUILayout.Label("Player Name: " + playerName);
		
		//allow player to change name
		if (GUILayout.Button("Change Player Name"))
		{
			//set to default name, trigger a draw of the set name box
			playerName = "No Name";
		}
		
		if (GUILayout.Button("Start Server", GUILayout.Height(buttonHeight)))
		{
			startServer();
		}
		if (GUILayout.Button("Join the Game", GUILayout.Height(buttonHeight)))
		{
			Debug.Log("I wanna join as client");
			makeMeAClient = true;
		}
		
	
	}
	
	void ClientLoginWindow (int windowID)
	{
		GUILayout.Label("Enter Server IP");
		connectionIP = GUILayout.TextField(connectionIP);
		
		GUILayout.Label("Enter Server Port Number");
		connectionPort = int.Parse(GUILayout.TextField(connectionPort.ToString()));
		
		GUILayout.Space(20);
		
		if (GUILayout.Button("Login", GUILayout.Height(buttonHeight), GUILayout.Height(buttonHeight)))
		{
			//check to make sure the player name is valid before connecting
			if(playerName == "")
			{
				//trigger set name window if the name is not valid
				playerName = "No Name";
			}
			Network.Connect(connectionIP, connectionPort);
		}
		
		if (GUILayout.Button("Go Back"))
		{
			makeMeAClient = false;
		}
		
		
	}
	
	void ServerInfoWindow (int windowID)
	{
		GUILayout.Space(20);
		if (GUILayout.Button("Shut Down Server"))
		{
			Network.Disconnect();	
		}
	}

	void ClientInfoWindow (int windowID)
	{
		GUILayout.Space(20);
		if (GUILayout.Button("Disconnect"))
		{
			Network.Disconnect();	
		}
	}
	
	void SetNameWindow (int windowID)
	{
		GUILayout.Label("Enter Your Player Name: ");
		tempName = GUILayout.TextField(tempName, 25);
		if (GUILayout.Button("Set Name", GUILayout.Height (buttonHeight)))
		{
			if (playerName != "")
			{
				playerName = tempName;
				//Store the player name locally
				PlayerPrefs.SetString("playerName", playerName);
			}
		}
	}
	
	void OnGUI ()
	{
		if (playerName == "No Name" || playerName == "")
		{
			leftIndent = Screen.width / 2 - connectWindowWidth / 2;	
			topIndent = Screen.height / 3 - connectWindowHeight / 2;
			
			connectWindowRect = new Rect(leftIndent, topIndent, connectWindowWidth, connectWindowHeight);
			connectWindowRect = GUILayout.Window(3, connectWindowRect, SetNameWindow, "Player Name");
		}
		
		else 
		{
			//first - I'll need to be server
			if (Network.peerType == NetworkPeerType.Disconnected && !makeMeAClient) {
				//set up first connection window in center of screen 	
				leftIndent = Screen.width / 2 - connectWindowWidth / 2;	
				topIndent = Screen.height / 3 - connectWindowHeight / 2;
				
				connectWindowRect = new Rect (leftIndent, topIndent, connectWindowWidth, connectWindowHeight);
				//create the window
				connectWindowRect = GUILayout.Window (0, connectWindowRect, ServerConnectWindow, titleMessage + " - " + playerName);
				 
			}
			
			// I do wanna be client, show login dialog
			if (Network.peerType == NetworkPeerType.Disconnected && makeMeAClient) {
				leftIndent = Screen.width / 2 - connectWindowWidth / 2;	
				topIndent = Screen.height / 3 - connectWindowHeight / 2;
				connectWindowRect = new Rect (leftIndent, topIndent, connectWindowWidth, 
						connectWindowHeight);
				//create the window
				connectWindowRect = GUILayout.Window (1, connectWindowRect, ClientLoginWindow, "Login\n" + playerName);	
			}
			
			if (Network.peerType == NetworkPeerType.Server) {
				infoWindowRect = new Rect (20, 20, infoWindowWidth, infoWindowHeight);
				infoWindowRect = GUILayout.Window (2, infoWindowRect, ServerInfoWindow, "Connected as Server\n" + playerName);
				
			}
			
			if (Network.peerType == NetworkPeerType.Client) {
				
				infoWindowRect = new Rect (20, 20, infoWindowWidth, infoWindowHeight);
				infoWindowRect = GUILayout.Window (2, infoWindowRect, ClientInfoWindow, "Connected as Client\n" + playerName);
	
			}
		}
		
	}
	

	
	[RPC]
	void NewPlayer(NetworkViewID ID, int matIndex, string pName, NetworkPlayer player)
	{
		Debug.Log ("recieved rpc from " + pName + ", player " + player + ", go.ID:" + ID + ", MatIndex:" + matIndex);
		
		NetworkView view = NetworkView.Find(ID);
		GameObject go = view.observed.gameObject;
		go.name = pName;
		GameObject.Find(go.name + "MageCharacter/robe").renderer.material = materials[matIndex];
		GameObject.Find(go.name + "MageCharacter/hat").renderer.material = materials[matIndex];
		GameObject.Find(go.name + "MageCharacter/staff").renderer.material = materials[matIndex];
		//go.GetComponent<PlayerLabel>().PlayerName = pName;
	}
	
	
	[RPC]
	void StartGame()
	{
		isGameActive = true;
	}
	
	[RPC]
	void GameOver()
	{
		isGameActive = false;
		
	}
}
