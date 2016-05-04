using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	public int underNum;
	public float velocity;
	public float jumpForce;
	private Rigidbody2D myRb;
	public bool canJump = false;
	public bool movingLeft;
	public bool movingRight;
	public bool jump = false;
	public bool controlling;
	public bool triggered;
	public GameObject armChild;
	private SpriteRenderer armSprite;
	public int health;
	private float stunTime;

	public Color myColor;
	public int [] cVals = new int[3];
	private SpriteRenderer mySprite;

	// Use this for initialization
	void Start () {
		underNum = 0;
		Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer ("Player"), LayerMask.NameToLayer ("Player"));
		Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer ("Player"), LayerMask.NameToLayer ("000"), true);
		Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer ("Player"), LayerMask.NameToLayer ("001"), true);
		Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer ("Player"), LayerMask.NameToLayer ("010"), true);
		Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer ("Player"), LayerMask.NameToLayer ("011"), true);
		Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer ("Player"), LayerMask.NameToLayer ("100"), true);
		Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer ("Player"), LayerMask.NameToLayer ("101"), true);
		Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer ("Player"), LayerMask.NameToLayer ("110"), true);
		Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer ("Player"), LayerMask.NameToLayer ("111"), true);

		velocity = 15f;
		jumpForce = 2000f;
		myRb = GetComponent<Rigidbody2D> ();
		myRb.freezeRotation = true;
		controlling = true;

		mySprite = GetComponent<SpriteRenderer> ();
		armSprite = armChild.GetComponent<SpriteRenderer> ();

		cVals [0] = Mathf.RoundToInt (myColor.r);
		cVals [1] = Mathf.RoundToInt (myColor.g);
		cVals [2] = Mathf.RoundToInt (myColor.b);

		refreshColor ();
		mySprite.color = myColor;
		armSprite.color = myColor;


	}

	void Update() {

		if (stunTime > 0f) {
			stunTime -= Time.deltaTime;
		}

		if (stunTime > 0f) {
			controlling = false;
		}
		else {
			controlling = true;
		}

		movingLeft = false;
		movingRight = false;
		canJump = underNum > 0;


		if (controlling) {
			if (Input.GetKey(KeyCode.A)) {
				movingLeft = true;
			}
			if (Input.GetKey(KeyCode.D)) {
				movingRight = true;
			}
				
			if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && canJump && Mathf.Abs(myRb.velocity.y) < 0.01f) {
				jump = true;
			}

			Vector3 mousePos = Input.mousePosition;
			mousePos.z = 10f;
			mousePos = Camera.main.ScreenToWorldPoint (mousePos);
			Vector3 s = transform.localScale;
			s.x = Mathf.Sign (mousePos.x - transform.position.x);
			transform.localScale = s;

		}
	}
		
	// Update is called once per frame
	void FixedUpdate () {
		transform.position = new Vector2 (transform.position.x, transform.position.y);

		if (movingLeft && canJump) {
			//restrict movement to one plane
			transform.position = new Vector2 (transform.position.x, transform.position.y);
			myRb.velocity = new Vector2 (-1 * velocity, myRb.velocity.y);
//			Vector3 s = transform.localScale;
//			s.x = -1;
//			transform.localScale = s;
		}
		if (movingLeft && !canJump && myRb.velocity.x > -1 * velocity) {
			myRb.AddForce (Vector3.left * 100f);
		}
		if (movingRight && canJump) {
			transform.position = new Vector2 (transform.position.x, transform.position.y);
			myRb.velocity = new Vector2 (velocity, myRb.velocity.y);
//			Vector3 s = transform.localScale;
//			s.x = 1;
//			transform.localScale = s;
		}
		if (movingRight && !canJump && myRb.velocity.x < velocity) {
			myRb.AddForce (Vector3.right * 100f);
		}
		if (jump) {
			myRb.AddForce (Vector2.up * jumpForce);
			jump = false;
		}
		if (!movingRight && !movingLeft && canJump) {
			myRb.velocity = new Vector2(0f, myRb.velocity.y);
		}
	}

	void OnTriggerEnter2D (Collider2D other) {
		underNum++;
	}
	void OnTriggerExit2D (Collider2D other) {
		underNum--;
	}

	public void refreshColor () {
		myColor = new Color (cVals [0], cVals [1], cVals [2]);
		mySprite.color = myColor;
		armSprite.color = myColor;
	}

	public void takeDamage (int dm) {
		health -= dm;
	}

	public void knockBack (Vector3 kf) {
		myRb.velocity = Vector3.zero;
		myRb.AddForce (kf);
		stunTime = 0.5f;
	}

}