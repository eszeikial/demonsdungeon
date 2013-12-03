using UnityEngine;
using System.Collections;

public class SpawnScript : MonoBehaviour {
	
	public GameObject MagePrefab; //reference to the mage player prefab
	public GameObject SkullPrefab; //reference the skull prefab
	private GameObject[] spawnPoints; //array of spawnPoints
	
	
	// Use this for initialization
	void Start () {
		spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
		
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public GameObject GetSpawnPoint()
	{
		int index = Random.Range(0 , spawnPoints.Length);
		return spawnPoints[index];
	}
	
	public GameObject SpawnPlayer()
	{
		GameObject sp = GetSpawnPoint();
		GameObject go = (GameObject)Network.Instantiate(MagePrefab, sp.transform.position, Quaternion.identity, 0);
		return go;
	}
	
	public GameObject SpawnServer()
	{
		GameObject sp = GetSpawnPoint();
		GameObject go = (GameObject)Network.Instantiate(SkullPrefab, sp.transform.position, Quaternion.identity, 0);
		return go;
	}
}
