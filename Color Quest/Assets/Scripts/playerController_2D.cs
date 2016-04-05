using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;

public class playerController_2D : MonoBehaviour {

	private Rigidbody2D myRig;
	private Collider2D myCollider;
	private SpriteRenderer mySprite;
	private bool canJump;
	private bool onPlatform;
	public float jumpPower;// = 40.0f;
	public float runSpeed;// = 20.0f;
	public float maxRunSpeed;// = 10.0f;
	public float hFirction;// = 0.02f;
	private float k_GroundedRadius = 0.3f;
	private Transform m_GroundCheck;
	private AudioSource myAudio;

	private bool shooting;

	public GameObject platforms;
	private string colorPlatTag;
	public String[] colorPlatTags;
	public Color[] colorPlatForPlayer;
	public int[] colorMasks; //r == 1, g == 2, b == 4
	private int currColorPlatMask;

	// Use this for initialization
	void Start () {
		myRig = gameObject.GetComponent<Rigidbody2D>();
		myCollider = gameObject.GetComponent<Collider2D>();
		mySprite = gameObject.GetComponent<SpriteRenderer>();
		myAudio = gameObject.GetComponent<AudioSource>();
		m_GroundCheck = transform.Find("GroundCheck");
		currColorPlatMask = 0; //white
	}

	void FixedUpdate(){
		checkShooting ();
	}

	private void checkShooting(){

	}
	
	// Update is called once per frame
	void Update () {
		changeSprite ();
		myRig.velocity = new Vector2 (myRig.velocity.x * hFirction, myRig.velocity.y);
		checkCanJump ();
		checkMovement ();
		checkChangingColor ();
	}

	private void checkChangingColor(){
		if (Input.GetKey ("left shift") || Input.GetKey("right shift")) {
			//subtractive color mixing, this is 
			bool changePlatActive = false;
			int maskIndex = -1;
			if (Input.GetKeyDown("1")){
				maskIndex = 0; //red
			} else if (Input.GetKeyDown("2")){
				maskIndex = 1; //green
			} else if (Input.GetKeyDown("3")){
				maskIndex = 2; //blue
			}
			if (maskIndex > -1) {
				determineSubColorMix (maskIndex);
				foreach (Transform colorPlat in platforms.transform) {
					colorPlatform cpScript = colorPlat.GetComponent<colorPlatform> ();
					if (colorPlat.tag == colorPlatTag) {
						if (cpScript.inactive) {
							cpScript.setActive ();
						}
					} else {
						if (!cpScript.inactive) {
							cpScript.setInactive ();
						}
					}
				}
			}
		}
	}

	private void determineSubColorMix(int maskIndex){
		int mask = colorMasks [maskIndex];
		currColorPlatMask ^= mask;
		mySprite.color = colorPlatForPlayer [currColorPlatMask];
		colorPlatTag = colorPlatTags [currColorPlatMask];
	}

	private void checkMovement(){
		if (Input.GetKeyDown ("up") || Input.GetKeyDown ("w") || Input.GetKeyDown ("space")) {
			if (canJump) {
				onPlatform = false;
				myRig.velocity = new Vector2 (myRig.velocity.x, jumpPower);
				canJump = false;
			}
		}
		if (Input.GetKey ("left") || Input.GetKey ("a")) {
			if (myRig.velocity.x > -maxRunSpeed) {
				myRig.velocity = new Vector2 (myRig.velocity.x - runSpeed, myRig.velocity.y);
			}
			if (myRig.velocity.x <= -maxRunSpeed) {
				myRig.velocity = new Vector2 (-maxRunSpeed, myRig.velocity.y);
			}
		} else if (Input.GetKey ("right") || Input.GetKey ("d")) {
			if (myRig.velocity.x < maxRunSpeed) {
				myRig.velocity = new Vector2 (myRig.velocity.x + runSpeed, myRig.velocity.y);
			}
			if (myRig.velocity.x >= maxRunSpeed) {
				myRig.velocity = new Vector2 (maxRunSpeed, myRig.velocity.y);
			}
		}
	}

	private void checkCanJump(){
		Collider2D[] colliders = Physics2D.OverlapCircleAll (m_GroundCheck.position, 
			k_GroundedRadius, 1 << LayerMask.NameToLayer ("Platforms"));
		if (colliders.Length < 1) {
			canJump = false; //colliding with nothing
			onPlatform = false;
		}
		bool canJumpOnOne = false;
		for (int i = 0; i < colliders.Length; i++) {
			try {
				colorPlatform platScript = colliders [i].GetComponent<colorPlatform> ();
				if (!platScript.inactive) {
					canJumpOnOne = true;
					break;
				}
			} catch (Exception e) {
				if (colliders [i].gameObject != gameObject) {
					canJumpOnOne = true;
					break;
				}
			}
		}
		if (canJumpOnOne) {
			canJump = true;
		}
	}

	private void changeSprite(){
		//use animator
	}

	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.tag == "Platforms" || other.gameObject.tag == colorPlatTag) {
			if (!other.gameObject.GetComponent<colorPlatform> ().inactive) {
				onPlatform = true;
				canJump = true;
			}
		}
	}

	void OnCollisionStay2D(Collision2D other){
		if (other.gameObject.tag == "Platforms" || other.gameObject.tag == colorPlatTag) {
			colorPlatform platScript = other.gameObject.GetComponent<colorPlatform> ();
			if (!platScript.inactive) {
				if (Input.GetKeyDown ("down") || Input.GetKeyDown ("s")) {
					platScript.letPlayerFallThrough ();
				}
				onPlatform = true;
			}
		}
	}
		
}
