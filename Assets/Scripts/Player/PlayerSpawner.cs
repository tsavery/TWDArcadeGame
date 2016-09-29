using UnityEngine;
using System.Collections;

public class PlayerSpawner : MonoBehaviour {


	public GameObject playerObject;
	// Use this for initialization
	void Start () {
		
	}
	// Update is called once per frame	
	void Update () {
	
	}

	public void SpawnPlayer(int playerNumber) {
		
		GameObject spawnedPlayer = (GameObject)Instantiate(playerObject, transform.position, transform.rotation);
		spawnedPlayer.GetComponent<Player>().playerNumber = playerNumber;
		switch (playerNumber) {
		case 1: spawnedPlayer.GetComponent<MeshRenderer>().material.color  = Color.blue;
			break;
		case 2: spawnedPlayer.GetComponent<MeshRenderer>().material.color = Color.green;
			break;
		case 3: spawnedPlayer.GetComponent<MeshRenderer>().material.color = Color.yellow;
			break;
		case 4: spawnedPlayer.GetComponent<MeshRenderer>().material.color = Color.cyan;
			break;
		}	

	}
}
