using UnityEngine;
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
			collision.gameObject.SendMessage("OnHit");
			Destroy(gameObject);
		}
	}

	void OnBecameInvisible() {
		Destroy(gameObject);
	}
}
