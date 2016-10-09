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
		Debug.Log("SpawnPlayer");
		GameObject spawnedPlayer = (GameObject)Instantiate(playerObject, transform.position, transform.rotation);
		spawnedPlayer.GetComponent<Player>().playerNumber = playerNumber;
		SpriteRenderer[] spriteRenderers = spawnedPlayer.GetComponentsInChildren<SpriteRenderer>();
		switch (playerNumber) {
		case 1: spriteRenderers[0].material.color  = Color.blue;
				spriteRenderers[1].material.color  = Color.blue;
				break;
		case 2: spriteRenderers[0].material.color  = Color.green;
				spriteRenderers[1].material.color  = Color.green;
				break;
		case 3: spriteRenderers[0].material.color  = Color.yellow;
				spriteRenderers[1].material.color  = Color.yellow;
				break;
		case 4: spriteRenderers[0].material.color  = Color.cyan;
				spriteRenderers[1].material.color  = Color.cyan;
				break;
		}	

	}
}
