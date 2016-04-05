using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {


	public float velocity;
	public float jumpForce;
	private Rigidbody myRb;
	private bool onSomething = false;
	private bool underSomething = false;
	public bool movingLeft;
	public bool movingRight;
	public bool jump = false;
	private bool crouching = false;
	private bool shooting = false;

	public bool controlling;

	public CapsuleCollider myCollider;

	private float fullHeight;

	private Vector3 right;
	private Vector3 left;
	public Color laserColor;
	private LineRenderer myLines;

	// Use this for initialization
	void Start () {
		laserColor = Color.red;

		myCollider = this.gameObject.GetComponent<CapsuleCollider> ();
		myLines = GetComponent<LineRenderer> ();
		fullHeight = myCollider.bounds.extents.y;
	

		velocity = 10f;
		jumpForce = 1000;
		myRb = GetComponent<Rigidbody> ();
		myRb.freezeRotation = true;
		Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Interactable"));
		Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Player"));
		controlling = true;

	}

	void Update() {
		Vector3 colliderCenter = myCollider.bounds.center;
		Vector3 right = colliderCenter + Vector3.right * myCollider.bounds.extents.x * 0.95f;
		Vector3 left = colliderCenter - Vector3.right * myCollider.bounds.extents.x * 0.95f;

		Debug.DrawLine (right, right + (Vector3.down * myCollider.bounds.extents.y * 1.001f));
		Debug.DrawLine (left, left + (Vector3.down * myCollider.bounds.extents.y * 1.001f));
		Debug.DrawLine (right, right + (Vector3.up * fullHeight * 1.5f));
		Debug.DrawLine (left, left + (Vector3.up * fullHeight * 1.5f));

		onSomething = Physics.Linecast (right, right + (Vector3.down * myCollider.bounds.extents.y * 1.001f), 1 << LayerMask.NameToLayer ("Obstacle")) 
			|| Physics.Linecast (left, left + (Vector3.down * myCollider.bounds.extents.y * 1.001f), 1 << LayerMask.NameToLayer ("Obstacle"));

		underSomething = Physics.Linecast (right, right + (Vector3.up * fullHeight * 1.5f), 1 << LayerMask.NameToLayer ("Obstacle")) 
			|| Physics.Linecast (left, left + (Vector3.up * fullHeight * 1.5f), 1 << LayerMask.NameToLayer ("Obstacle"));

		movingLeft = false;
		movingRight = false;
		shooting = false;
		//		Camera.main.transform.position = new Vector3 (transform.position.x, transform.position.y + 3f, -10f);
		if (controlling) {
			//			Camera.main.transform.position = new Vector3 (transform.position.x, transform.position.y, -10f);
			if (Input.GetKey(KeyCode.A)) {
				movingLeft = true;
			}
			if (Input.GetKey(KeyCode.D)) {
				movingRight = true;
			}
				
			if (Input.GetKeyDown(KeyCode.Space) && onSomething && !crouching) {
				jump = true;
			}
			if (Input.GetKey(KeyCode.S)) {
				crouching = true;
			}
			if (!Input.GetKey(KeyCode.S) && !underSomething) {
				crouching = false;
			}
			if (Input.GetMouseButton (0)) {
				shooting = true;
			}
		}
			



	}

	// Update is called once per frame
	void FixedUpdate () {
		transform.position = new Vector3 (transform.position.x, transform.position.y, 0f);

		if (movingLeft) {
			//restrict movement to one plane
			transform.position = new Vector3 (transform.position.x, transform.position.y, 0f);
			myRb.velocity = new Vector3 (-1 * velocity, myRb.velocity.y, myRb.velocity.z);
			Vector3 s = transform.localScale;
			s.x = -1;
			transform.localScale = s;
		}
		if (movingRight) {
			transform.position = new Vector3 (transform.position.x, transform.position.y, 0f);
			myRb.velocity = new Vector3 (velocity, myRb.velocity.y, myRb.velocity.z);
			Vector3 s = transform.localScale;
			s.x = 1;
			transform.localScale = s;
		}

		if (crouching) {
			transform.localScale = new Vector3 (transform.localScale.x, 0.5f, 1f);
			velocity = 5f;
		}
		else {
			transform.localScale = new Vector3 (transform.localScale.x, 1f, 1f);
			velocity = 10f;
		}

		if (jump && onSomething && Mathf.Abs(myRb.velocity.y) < 0.01f) {
			myRb.AddForce (Vector3.up * jumpForce);
			jump = false;
		}
		if (!movingRight && !movingLeft) {
			myRb.velocity = new Vector3(0f, myRb.velocity.y, myRb.velocity.z);
		}

		myLines.enabled = false;
		if (shooting) {
			if (laserColor != Color.black) {
				myLines.SetColors (Color.red, Color.red);
				myLines.SetWidth (0.1f, 0.1f);
				Vector3 mousePos = Input.mousePosition;
				mousePos.z = 10f;
				mousePos = Camera.main.ScreenToWorldPoint (mousePos);
				Vector3 startPos = transform.position;
				Vector3 endPos = startPos + ((mousePos - startPos) * 50f);
				List<Vector3> points = new List<Vector3> ();
				points.Add (startPos);
				points.Add (endPos);
				myLines.SetPositions (points.ToArray());
				myLines.enabled = true;
			}
		}

		//transform.rotation = Quaternion.Euler (Vector3.zero);
	}
}