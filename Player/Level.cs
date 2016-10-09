using UnityEngine;
using UnityEditor.SceneManagement;
using System.Collections.Generic;

public class Level : MonoBehaviour {

    public int numplayers;

    // Used by buttons to set the number of players in the game
    public void setNumPlayers(int num)
    {
        numplayers = num;
    }

    //Used by buttons to choose the arena or menu to load
    public void setScene(int scene)
    {
        EditorSceneManager.LoadScene(scene);
    }

    // Use this for initialization
    void Start () {
		Physics2D.IgnoreLayerCollision(8, 8);

        int players = numplayers;
		GameObject[] spawners = GameObject.FindGameObjectsWithTag("Spawner");

		List<int> spawnersUsed = new List<int>();
		int i;
		while (players > 0) {
			i = Random.Range(0, spawners.Length);

			if (!spawnersUsed.Contains(i)) {
				spawnersUsed.Add(i);
				spawners[i].GetComponent<PlayerSpawner>().SpawnPlayer(players);
				players--;
			}
			else {
				continue;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    
}
