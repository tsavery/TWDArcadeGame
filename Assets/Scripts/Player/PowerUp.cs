using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour {

	bool pickUp = false;
	PowerUpSpawner parentSpawner;
	public Player.PowerUp powerUpType;
	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		if(pickUp) {
			
			parentSpawner.SendMessage("OnPickedUpPowerUp");
			Destroy(this.gameObject);
		}

	}

	void OnTriggerEnter2D(Collider2D other) {
		
		if(other.tag == "PowerUpSpawner") {
			parentSpawner = other.GetComponentInParent<PowerUpSpawner>();
		}

		if (other.tag == "Player") {
			if (powerUpType == Player.PowerUp.Pistol) {
				other.GetComponentInParent<Player>().currentPowerUp = Player.PowerUp.Pistol;
				other.GetComponentInParent<Player>().powerUpUsesLeft = 1;
			}
			else if (powerUpType == Player.PowerUp.Crossbow) {
				other.GetComponentInParent<Player>().currentPowerUp = Player.PowerUp.Crossbow;
				other.GetComponentInParent<Player>().powerUpUsesLeft = 3;
			}
			else if(powerUpType == Player.PowerUp.Shield) {
				other.GetComponentInParent<Player>().currentPowerUp = Player.PowerUp.Shield;
				other.GetComponentInParent<Player>().powerUpUsesLeft = 3;
			}
			else if(powerUpType == Player.PowerUp.Blink) {
				other.GetComponentInParent<Player>().currentPowerUp = Player.PowerUp.Blink;
				other.GetComponentInParent<Player>().powerUpUsesLeft = 3;
			}

			pickUp = true;
		}



	}

}
