using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShootingControlBad : MonoBehaviour {

	public GameObject source;
	public bool shooting;
	public LineRenderer[] myLines;
	public Color myColor;
	public int[] cVals = new int[3];
	public bool controlling;

	// Use this for initialization
	void Start () {
		shooting = false;
		controlling = true;
		myLines = GetComponentsInChildren<LineRenderer> ();
		cVals [0] = Mathf.RoundToInt (myColor.r);
		cVals [1] = Mathf.RoundToInt (myColor.g);
		cVals [2] = Mathf.RoundToInt (myColor.b);
		refreshColor ();

	}
	
	// Update is called once per frame
	void Update () {
		shooting = false;
		if (controlling) {
			if (Input.GetMouseButton (0)) {
				shooting = true;
			}
		}
		for (int i = 0; i < myLines.Length; i++) {
			myLines [i].enabled = false;
		}
		if (shooting) {
			if (myColor != Color.black) {
				bool bouncing = true;
				int lineNum = 0;

				Vector3 mousePos = Input.mousePosition;
				mousePos.z = 10f;
				mousePos = Camera.main.ScreenToWorldPoint (mousePos);
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
					RaycastHit2D bounceHit = Physics2D.Raycast (prevPos, nextDir, 100f, ~(1 << LayerMask.NameToLayer ("Player")));
					if (bounceHit.collider == null) {
						bouncing = false;
						Vector3 endPos = prevPos + (nextDir * 100f);
						myLines [i].SetPositions (new Vector3[] { prevPos, endPos });
					}
					else if (bounceHit.collider.tag == "Enemy") {
						EnemyScript es = bounceHit.collider.gameObject.GetComponent<EnemyScript> ();
						es.absorbColor (nextCVals);
						myLines [i].SetPositions (new Vector3[] { prevPos, bounceHit.point });
						Debug.DrawRay (bounceHit.point, Vector3.down * 50f, Color.yellow);
						prevPos = bounceHit.point + (bounceHit.normal.normalized * 0.01f);
						nextDir = (Vector2.Reflect (nextDir, bounceHit.normal)).normalized;
						bouncing = false;
					}
					else if (bounceHit.collider.tag == "Platform") {
						PlatformScript ps = bounceHit.collider.gameObject.GetComponent<PlatformScript> ();
						nextCVals = ps.reflectColor (nextCVals);
						nextColor = new Color (nextCVals [0], nextCVals [1], nextCVals [2]);
						Debug.DrawRay (prevPos, nextDir * 50f, Color.green);
						myLines [i].SetPositions (new Vector3[] { prevPos, bounceHit.point });
						Debug.DrawRay (bounceHit.point, Vector3.down * 50f, Color.yellow);
						prevPos = bounceHit.point + (bounceHit.normal.normalized * 0.01f);
						nextDir = (Vector2.Reflect (nextDir, bounceHit.normal)).normalized;
					}
					else {
						Debug.DrawRay (prevPos, nextDir * 50f, Color.green);
						myLines [i].SetPositions (new Vector3[] { prevPos, bounceHit.point });
						Debug.DrawRay (bounceHit.point, Vector3.down * 50f, Color.yellow);
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
