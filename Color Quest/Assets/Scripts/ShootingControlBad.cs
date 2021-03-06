﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShootingControlBad : MonoBehaviour {

	public GameObject source;
	public bool shooting;
	public LineRenderer[] myLines;
	public Color myColor;
	public int[] cVals = new int[3];
	public bool controlling;
	private SpriteRenderer mySprite;
	public Light lt;

	// Use this for initialization
	void Start () {
		shooting = false;
		controlling = true;
		myLines = GetComponentsInChildren<LineRenderer> ();
		cVals [0] = Mathf.RoundToInt (myColor.r);
		cVals [1] = Mathf.RoundToInt (myColor.g);
		cVals [2] = Mathf.RoundToInt (myColor.b);
		refreshColor ();
		mySprite = source.GetComponent<SpriteRenderer> ();

	}
	
	// Update is called once per frame
	void Update () {
		mySprite.color = myColor;
		Vector3 mousePos = Input.mousePosition;
		mousePos.z = 10f;
		mousePos = Camera.main.ScreenToWorldPoint (mousePos);


		shooting = false;
		if (controlling) {
			
			float zRot = Mathf.Rad2Deg * (Mathf.Atan2 (mousePos.y - transform.position.y, mousePos.x - transform.position.x));
			if (mousePos.x - transform.position.x < 0) {
				transform.localScale = new Vector3 (-1f, -1f, 1f);
				zRot = 360f - zRot;
			}
			else {
				transform.localScale = new Vector3 (1f, 1f, 1f);
			}
			transform.rotation = Quaternion.Euler (new Vector3 (0f, 0f, zRot));
			if (Input.GetMouseButton (0)) {
				shooting = true;
			}
		}
		for (int i = 0; i < myLines.Length; i++) {
			myLines [i].enabled = false;
		}
		lt.color = Color.black;
		if (shooting) {
			if (myColor != Color.black) {
				lt.color = myColor;
				bool bouncing = true;
				int lineNum = 0;


				Vector3 startPos = source.transform.position;
				Vector3 prevPos = startPos;
				Vector3 nextDir = (mousePos - startPos).normalized;

				int[] nextCVals = cVals;
				Color nextColor = new Color (nextCVals [0], nextCVals [1], nextCVals [2]);
				int i = 0;
				while (bouncing && i < myLines.Length) {
					myLines [i].enabled = true;
//					Debug.Log (nextColor);
					myLines [i].SetColors (nextColor, nextColor);
					myLines [i].SetWidth (0.2f, 0.2f);
					myLines [i].SetVertexCount (2);
					RaycastHit2D bounceHit = Physics2D.Raycast (prevPos, nextDir, 100f, ~(1 << LayerMask.NameToLayer ("Player") | 1 << LayerMask.NameToLayer ("Flag")));
					if (bounceHit.collider == null) {
						bouncing = false;
						Vector3 endPos = prevPos + (nextDir * 100f);
						myLines [i].SetPositions (new Vector3[] { prevPos, endPos });
					}
					else if (bounceHit.collider.tag == "Enemy") {
						EnemyScript es = bounceHit.collider.gameObject.GetComponent<EnemyScript> ();
						es.absorbColor (nextCVals);
						myLines [i].SetPositions (new Vector3[] { prevPos, bounceHit.point });
						prevPos = bounceHit.point + (bounceHit.normal.normalized * 0.01f);
						nextDir = (Vector2.Reflect (nextDir, bounceHit.normal)).normalized;
						bouncing = false;
					}
					else if (bounceHit.collider.tag == "Interactable") {
						SwitchScript es = bounceHit.collider.gameObject.GetComponent<SwitchScript> ();
						es.absorbColor (nextCVals);
						myLines [i].SetPositions (new Vector3[] { prevPos, bounceHit.point });
						prevPos = bounceHit.point + (bounceHit.normal.normalized * 0.01f);
						nextDir = (Vector2.Reflect (nextDir, bounceHit.normal)).normalized;
						bouncing = false;
					}
					else if (bounceHit.collider.tag == "Platform") {
						PlatformScript ps = bounceHit.collider.gameObject.GetComponent<PlatformScript> ();
						nextCVals = ps.reflectColor (nextCVals);
						nextColor = new Color (nextCVals [0], nextCVals [1], nextCVals [2]);
						myLines [i].SetPositions (new Vector3[] { prevPos, bounceHit.point });
						prevPos = bounceHit.point + (bounceHit.normal.normalized * 0.01f);
						nextDir = (Vector2.Reflect (nextDir, bounceHit.normal)).normalized;
					}
					else {
						myLines [i].SetPositions (new Vector3[] { prevPos, bounceHit.point });
						prevPos = bounceHit.point + (bounceHit.normal.normalized * 0.01f);
						nextDir = (Vector2.Reflect (nextDir, bounceHit.normal)).normalized;
					}
					if (nextColor == Color.black) {
						bouncing = false;
					}
					i++;
				}
			}
		}
	}

	public void refreshColor () {
		myColor = new Color (cVals [0], cVals [1], cVals [2]);
	}
}
