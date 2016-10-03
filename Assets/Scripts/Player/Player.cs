using UnityEngine;
using System.Collections;

[RequireComponent (typeof (PlayerController2D))]
public class Player : MonoBehaviour
{
	public int playerNumber;
	public float jumpHeight = 4f;
	public float timeToJumpApex = 0.4f;

	public enum PowerUp {
		Pistol, Crossbow, Shield, None
	}

	public PowerUp currentPowerUp{get; set;}
	public int powerUpUsesLeft{get; set;}

	public int maxJumps = 2;
	int jumpsLeft;
	//how much gravity is applied to the character
	float gravity;

	//movement Speed of the Character
	public float moveSpeed = 6f;
	float jumpVelocity;
	//velocity of the character;
	Vector3 velocity;

	float velocityXSmoothing;

	float accelerationTimeAirborn = 0;
	float accelerationTimeGrounded = 0;

	float fireSpeed = 2;
	float fireRate;

	public GameObject bulletProjectile;
	public GameObject arrowProjectile;
	private GameObject firedProjectile;

	public int health = 2;

	bool isFacingRight = true;

	Collider2D meleeHitbox;
	public CircleCollider2D shieldHitBox;

	public bool shieldOn {get; set;}

    PlayerController2D controller;
	// Use this for initialization
	void Start ()
    {
		meleeHitbox = GetComponentInChildren<BoxCollider2D>();
		jumpsLeft = maxJumps;
        controller = GetComponent<PlayerController2D>();

		gravity = -(2 * jumpHeight / Mathf.Pow(timeToJumpApex, 2));
		jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;

		powerUpUsesLeft = 0;
		currentPowerUp = PowerUp.None;
	}

	void FixedUpdate() {
		
	}
	// Update is called once per frame
	void Update ()
    {
		if(controller.collisions.above || controller.collisions.below) {
			velocity.y = 0;
		}
		//left and right movement input
		Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal" + playerNumber), Input.GetAxisRaw("Vertical" + playerNumber));
		//jump input
		if (Input.GetButtonDown("Jump" + playerNumber) && (jumpsLeft != 0)) {
			velocity.y = jumpVelocity;
			jumpsLeft--;
		}
		// apply gravity & movement

		float targetVelocityX = input.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp( velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborn);
		velocity.y += gravity * Time.deltaTime;

		//apply velocity to the character
		controller.Move(velocity * Time.deltaTime);


		if(controller.collisions.below) {
			jumpsLeft = maxJumps;
		}

		if (Input.GetButtonDown("Fire" + playerNumber)) {
			switch (currentPowerUp) {
			case PowerUp.Pistol : FirePistol();
				break;
			case PowerUp.Crossbow : FireArrow();
				break;
			}
		}
//		if(Input.GetButtonDown("Melee" + playerNumber)) {
			
	//	}

		if (velocity.x != 0) {
			if (Mathf.Sign(velocity.x) == 1) {
				isFacingRight = true;
			}
			else {
				isFacingRight = false;
			}
		}

	}

	void OnHit () {

		Debug.Log("OnHit");
		GameObject[] spawners = GameObject.FindGameObjectsWithTag("PlayerSpawner");

		int i = Random.Range (0, spawners.Length);

		spawners[i].GetComponent<PlayerSpawner>().SpawnPlayer(playerNumber);

		Destroy(gameObject);
	}

	void OnBecameInvisible() {
		//GameObject[] spawners = GameObject.FindGameObjectsWithTag("Spawner");

		//int i = Random.Range (0, spawners.Length);

		//spawners[i].GetComponent<PlayerSpawner>().SpawnPlayer(playerNumber);

		//Destroy(gameObject);
	}

	void FirePistol() {

		if (powerUpUsesLeft > 0) {
			firedProjectile = Instantiate(bulletProjectile, transform.position, transform.rotation) as GameObject;
			firedProjectile.GetComponent<Rigidbody2D>().AddForce(new Vector2((isFacingRight?1:-1) * 2000, 0));
			Physics2D.IgnoreCollision(firedProjectile.GetComponent<Collider2D>(), GetComponent<Collider2D>());	
			powerUpUsesLeft -= 1;
		}

	}

	void FireArrow() {
		if (powerUpUsesLeft > 0) {
			firedProjectile = Instantiate(arrowProjectile, transform.position, transform.rotation) as GameObject;
			firedProjectile.GetComponent<Rigidbody2D>().AddForce(new Vector2((isFacingRight?1:-1) * 1000, 250));
			Physics2D.IgnoreCollision(firedProjectile.GetComponent<Collider2D>(), GetComponent<Collider2D>());	
			powerUpUsesLeft -= 1;
		}
	}

}
