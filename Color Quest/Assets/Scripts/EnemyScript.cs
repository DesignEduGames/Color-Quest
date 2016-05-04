using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {

	public Color myColor;
	public int [] cVals = new int[3];
	public int [] lcVals = new int[3];
	//	private LineRenderer myLines;
	private SpriteRenderer mySprite;


	public GameObject playerObj;
	public PlayerController player;

	public int state;

	private Rigidbody2D myRb;

	//	public Color laserColor;

	// Use this for initialization
	void Start () {


		mySprite = GetComponent<SpriteRenderer> ();
		//		myLines = GetComponent<LineRenderer> ();
		myRb = GetComponent<Rigidbody2D> ();

		cVals [0] = Mathf.RoundToInt (myColor.r);
		cVals [1] = Mathf.RoundToInt (myColor.g);
		cVals [2] = Mathf.RoundToInt (myColor.b);

		player = playerObj.GetComponent<PlayerController> ();


		refreshColor ();
		//		laserColor = Color.black;

	}

	// Update is called once per frame
	void Update () {

		transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler (Vector3.zero), 0.05f);

		float dist = Vector3.Distance (transform.position, player.transform.position);
//		~(1 << LayerMask.NameToLayer ("Player") | 1 << LayerMask.NameToLayer ("Enemy"))
//		Debug.Log (Physics2D.Raycast (transform.position, player.transform.position - transform.position, 
//			dist, ~(1 << LayerMask.NameToLayer ("Player") | 1 << LayerMask.NameToLayer ("Enemy"))).collider == null);

	
		if (dist < 50f && Physics2D.Raycast (transform.position, player.transform.position - transform.position, 
			dist, ~(1 << LayerMask.NameToLayer ("Player") | 1 << LayerMask.NameToLayer ("Enemy"))).collider == null) {
//			myRb.velocity = (player.transform.position - transform.position).normalized * 3f;
			myRb.AddForce ((player.transform.position - transform.position).normalized * 10f);
			myRb.velocity = myRb.velocity.normalized * 3f;
		}
		else {
			myRb.velocity = Vector3.zero;
		}

//		Debug.DrawRay (transform.position, (player.transform.position - transform.position).normalized * dist, Color.red);
	}

	public void refreshColor () {
		myColor = new Color (cVals [0], cVals [1], cVals [2]);
		mySprite.color = myColor;
	}

	public void absorbColor (int[] inCVals) {

		for (int i = 0; i < 3; i++) {
			if (inCVals [i] != cVals [i]) {
				return;
			}
		}
		Destroy(gameObject);
	}

	void OnCollisionStay2D (Collision2D collision) {
		myRb.AddForce ((collision.contacts [0].normal) * 100f);
		Debug.Log ("collided");
		if (collision.collider.tag == "Player") {
			player.takeDamage (1);
			Vector3 kvec = new Vector3 (Mathf.Sign (player.transform.position.x - transform.position.x), 2f);
			player.knockBack (kvec.normalized * 2000f);
		}
	}
		
}
