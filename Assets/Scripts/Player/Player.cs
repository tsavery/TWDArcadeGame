using UnityEngine;
using System.Collections;

[RequireComponent (typeof (PlayerController2D))]
public class Player : MonoBehaviour
{
	public int playerNumber;
	public float jumpHeight = 4f;
	public float timeToJumpApex = 0.4f;

	public enum PowerUp {
		Pistol, Crossbow, Shield, Blink, None
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

	public int meleeTimer;

	bool isFacingRight = true;
	bool oldDirection;
	bool isJumping = false;

	Collider2D meleeHitbox;
	public CircleCollider2D shieldHitBox;

	public bool shieldOn {get; set;}

	private Animator upperAnimator;
	private Animator lowerAnimator;

	public float blinkDistance = 500f;
	Vector2 knockbackBuffer;

    PlayerController2D controller;
	// Use this for initialization
	void Start ()
    {
		//meleeHitbox = gameObject.GetComponentsInChildren<BoxCollider2D>()[1];
		upperAnimator = gameObject.GetComponentsInChildren<Animator>()[0];
		lowerAnimator = gameObject.GetComponentsInChildren<Animator>()[1];
//		meleeHitbox.enabled = false;
		jumpsLeft = maxJumps;
        controller = GetComponent<PlayerController2D>();

		gravity = -(2 * jumpHeight / Mathf.Pow(timeToJumpApex, 2));
		jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;

		powerUpUsesLeft = 0;
		currentPowerUp = PowerUp.None;
		knockbackBuffer = new Vector2(0,0);

		shieldHitBox = GetComponent<CircleCollider2D>();
	}

	void FixedUpdate() {
		



	}
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetButtonDown("Melee" + playerNumber) && meleeTimer == 0) {
			upperAnimator.SetTrigger("MeleeAttack");
//			meleeHitbox.enabled = true;
		}
			
		if(controller.collisions.above || controller.collisions.below) {
			velocity.y = 0;
		}

		if(controller.collisions.below) {
			lowerAnimator.SetBool("isJumping", false);
			isJumping = false;
		}
		//left and right movement input
		Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal" + playerNumber), Input.GetAxisRaw("Vertical" + playerNumber));
		//jump input
		if (Input.GetButtonDown("Jump" + playerNumber) && (jumpsLeft != 0)) {
			velocity.y = jumpVelocity;
			jumpsLeft--;
			lowerAnimator.SetBool("isJumping", true);
			isJumping = true;

		}
		// apply gravity & movement

		float targetVelocityX = input.x * moveSpeed;
		velocity.x = targetVelocityX;
		//velocity.x = Mathf.SmoothDamp( velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborn);
		velocity.y += gravity * Time.deltaTime;



		if (Input.GetButtonDown("Fire" + playerNumber)) {
			switch (currentPowerUp) {
			case PowerUp.Pistol : FirePistol();
								  upperAnimator.SetTrigger("ShootPistol");
							  	  break;
			case PowerUp.Crossbow : FireArrow();
									upperAnimator.SetTrigger("ShootCrossbow");
									break;
			case PowerUp.Blink: Blink();
								break;
			
			default: break;
			}

		}
		if(shieldOn == true && currentPowerUp == PowerUp.Shield && powerUpUsesLeft == 0) {
			ToggleShield();
		}

		if(shieldOn == true && currentPowerUp != PowerUp.Shield) {
			ToggleShield();
		}

		if(currentPowerUp == PowerUp.Shield && shieldOn == false && powerUpUsesLeft == 3) {
			ToggleShield();
		}

		if (velocity.x != 0) {
			if (velocity.x > 0) {
				if(isFacingRight != true) {
					oldDirection = isFacingRight;
					isFacingRight = true;
					flip();
				}


			}
			else if(velocity.x < 0) {
				if(isFacingRight != false) {
					oldDirection = isFacingRight;
					isFacingRight = false;
					flip();
				}


			}
				
		}

		if(knockbackBuffer.x != 0 && knockbackBuffer.y != 0) {
			velocity.x += knockbackBuffer.x / 10;
			velocity.y += knockbackBuffer.y /10;
			knockbackBuffer.x -= knockbackBuffer.x / 10;
			knockbackBuffer.y -= knockbackBuffer.y / 10;
		}

		if(transform.localScale.x > 0) {
			if(controller.collisions.right == false) {
				controller.Move(velocity * Time.deltaTime, transform.localScale.x);
			}
			else {

				velocity.x = 0;
				controller.Move(velocity * Time.deltaTime, transform.localScale.x);
			}

		}
		else if (transform.localScale.x < 0) {
			if(controller.collisions.left == false) {
				
				velocity.x *= -1;
				controller.Move(velocity * Time.deltaTime, transform.localScale.x);
			}
			else {

				velocity.x = 0;
				controller.Move(velocity * Time.deltaTime, transform.localScale.x);
			}

		}
		else {
			controller.Move(velocity * Time.deltaTime, 1);
		}
		if(velocity.x != 0 && isJumping == false) {
			lowerAnimator.SetBool("isWalking", true);
			//Debug.Log("yo");
		}
		else {
			lowerAnimator.SetBool("isWalking", false);
			//Debug.Log("yo2");
		}
		//apply velocity to the character
		//controller.Move(velocity * Time.deltaTime);




		if(controller.collisions.below) {
			jumpsLeft = maxJumps;
		}






	}

	void OnHit (GameObject source) {
		Debug.Log("Hello");
		health--;
		if(health == 0 || source.tag == "Bullet") {
			GameObject[] spawners = GameObject.FindGameObjectsWithTag("PlayerSpawner");

			int i = Random.Range (0, spawners.Length);

			spawners[i].GetComponent<PlayerSpawner>().SpawnPlayer(playerNumber);

			Destroy(gameObject);
		}
		else {
			if(source.GetComponentInParent<Player>().gameObject.tag == "Player") {
				if (source.GetComponentInParent<Player>().isFacingRight) {
					knockbackBuffer.x = 300;
				}
				else {
					knockbackBuffer.x = -300;
				}
				knockbackBuffer.y = 15;
			}
			else {
				if (isFacingRight) {
					knockbackBuffer.x = -300;
				}
				else {
					knockbackBuffer.x = 300;
				}
				knockbackBuffer.y = 15;
			}

		}

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
			Physics2D.IgnoreCollision(firedProjectile.GetComponent<Collider2D>(), GetComponent<BoxCollider2D>());	
			powerUpUsesLeft -= 1;
			if(powerUpUsesLeft == 0) {
				currentPowerUp = PowerUp.None;
			}
		}

	}

	void FireArrow() {
		if (powerUpUsesLeft > 0) {
			firedProjectile = Instantiate(arrowProjectile, transform.position, transform.rotation) as GameObject;
			firedProjectile.GetComponent<Rigidbody2D>().AddForce(new Vector2((isFacingRight?1:-1) * 1000, 250));
			Physics2D.IgnoreCollision(firedProjectile.GetComponent<Collider2D>(), GetComponent<BoxCollider2D>());	
			powerUpUsesLeft -= 1;
			if(powerUpUsesLeft == 0) {
				currentPowerUp = PowerUp.None;
			}
		}
	}
	void Blink() {
		if(powerUpUsesLeft > 0) {
			if (isFacingRight) {
				velocity.x += blinkDistance * 1;
			}
			else {
				velocity.x += blinkDistance * -1;
			}
			powerUpUsesLeft -= 1;
			if(powerUpUsesLeft == 0){
				currentPowerUp = PowerUp.None;
			}
		}

	}

	void ToggleShield() {
		shieldHitBox.enabled = !shieldHitBox.enabled;
		shieldOn = !shieldOn;
	}

	void flip() {

		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}

}
