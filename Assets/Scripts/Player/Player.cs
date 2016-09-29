using UnityEngine;
using System.Collections;

[RequireComponent (typeof (PlayerController2D))]
public class Player : MonoBehaviour
{
	public int playerNumber;
	public float jumpHeight = 4f;
	public float timeToJumpApex = 0.4f;

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

	public GameObject projectile;
	private GameObject firedProjectile;

	public int health = 2;

	bool isFacingRight = true;

    PlayerController2D controller;
	// Use this for initialization
	void Start ()
    {
		jumpsLeft = maxJumps;
        controller = GetComponent<PlayerController2D>();

		gravity = -(2 * jumpHeight / Mathf.Pow(timeToJumpApex, 2));
		jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
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
			firedProjectile = Instantiate(projectile, transform.position, transform.rotation) as GameObject;
			firedProjectile.GetComponent<Rigidbody2D>().AddForce(new Vector2((isFacingRight?1:-1) * 1000, 0));
			Physics2D.IgnoreCollision(firedProjectile.GetComponent<Collider2D>(), GetComponent<Collider2D>());
		}
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
		GameObject[] spawners = GameObject.FindGameObjectsWithTag("Spawner");

		int i = Random.Range (0, spawners.Length);

		spawners[i].GetComponent<PlayerSpawner>().SpawnPlayer(playerNumber);

		Destroy(gameObject);
	}

	void OnBecameInvisible() {
		GameObject[] spawners = GameObject.FindGameObjectsWithTag("Spawner");

		int i = Random.Range (0, spawners.Length);

		spawners[i].GetComponent<PlayerSpawner>().SpawnPlayer(playerNumber);

		Destroy(gameObject);
	}


}
