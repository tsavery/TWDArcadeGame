﻿using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D collision) {
		if(collision.gameObject.tag.Equals("Level"))
			Destroy(gameObject);

		if(collision.gameObject.layer == 8 ) {
			Debug.Log("Hit");
			collision.gameObject.SendMessage("OnHit");
			Destroy(gameObject);
		}
	}
	void OnTriggerEnter2D(Collider2D other) {
		if((other.GetComponentInParent<Player>().shieldHitBox == other) && (other.GetComponentInParent<Player>().shieldOn == true)) {
			other.GetComponentInParent<Player>().powerUpUsesLeft -= 1;
			Destroy(gameObject);
		}
			
	}

	void OnBecameInvisible() {
		Destroy(gameObject);
	}
}
