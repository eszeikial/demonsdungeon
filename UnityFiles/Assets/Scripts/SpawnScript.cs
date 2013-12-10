using UnityEngine;
using System.Collections;

public class SpawnScript : MonoBehaviour {
	
	public GameObject MagePrefab; //reference to the mage player prefab
	public GameObject SkullPrefab; //reference the skull prefab
	private GameObject WallManagerGO;
	private WallManager wm;
	
	
	// Use this for initialization
	void Start () {
		//spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
		WallManagerGO = GameObject.Find ("WallManagerGO");
		wm = WallManagerGO.GetComponent<WallManager>();
	}
	
	public GameObject SpawnPlayer()
	{
		Vector3 xyz = wm.generatePlayerSpawnPoint(); 
		GameObject go = (GameObject)Network.Instantiate(MagePrefab, xyz, Quaternion.identity, 0);
		return go;
	}
	
	public GameObject SpawnServer()
	{
		Vector3 xyz = wm.generateServerSpawnPoint();
		GameObject go = (GameObject)Network.Instantiate(SkullPrefab, xyz, Quaternion.identity, 0);
		return go;
	}
}
