using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShootingControl : MonoBehaviour {
	
	public GameObject source;
	public bool shooting;
	public LineRenderer myLines;
	public Color myColor;
	public int [] cVals = new int[3];
	public bool controlling;


	// Use this for initialization
	void Start () {
		shooting = false;
		controlling = true;
		myLines = GetComponent<LineRenderer> ();
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
		myLines.enabled = false;
		if (shooting) {
			if (myColor != Color.black) {
				bool bouncing = true;
				myLines.SetColors (myColor, myColor);
				myLines.SetWidth (0.1f, 0.1f);
				Vector3 mousePos = Input.mousePosition;
				mousePos.z = 10f;
				mousePos = Camera.main.ScreenToWorldPoint (mousePos);
				Vector3 startPos = source.transform.position;
				Vector3 prevPos = startPos;
				Vector3 nextDir = (mousePos - startPos).normalized;
				List<Vector3> points = new List<Vector3> ();
				points.Add (startPos);
				int totalBounces = 0;
				while (bouncing && totalBounces < 10) {
//					Debug.Log (totalBounces);
					RaycastHit2D bounceHit = Physics2D.Raycast (prevPos, nextDir, 100f, ~(1 << LayerMask.NameToLayer("Player")));
					if (bounceHit.collider == null) {
						bouncing = false;
						Vector3 endPos = prevPos + (nextDir * 100f);
						points.Add (endPos);
					}
					else {
						Debug.DrawRay (prevPos, nextDir * 50f, Color.green);
						totalBounces++;
						points.Add (bounceHit.point);
						Debug.DrawRay (bounceHit.point, Vector3.down * 50f, Color.yellow);
						prevPos = bounceHit.point + (bounceHit.normal.normalized * 0.01f);
						nextDir = (Vector2.Reflect (nextDir, bounceHit.normal)).normalized;
					}
				}
				myLines.SetVertexCount (points.Count * 2);

				Vector3 [] pointArray = new Vector3[points.Count * 2];
				for (int i = 0; i < points.Count; i++) {
					pointArray [i] = points [i];
					pointArray [points.Count * 2 - 1 - i] = points [i];
				}
//				myLines.SetPositions (points.ToArray());
				myLines.SetPositions (pointArray);
				myLines.enabled = true;
			}
		}
	}
	public void refreshColor () {
		myColor = new Color (cVals [0], cVals [1], cVals [2]);
	}
}
