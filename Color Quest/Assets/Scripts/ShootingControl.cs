using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShootingControl : MonoBehaviour {
	
	public GameObject source;
	public bool shooting;
	public LineRenderer myLines;
	public Color laserColor;


	// Use this for initialization
	void Start () {
		shooting = false;
		myLines = GetComponent<LineRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		shooting = false;
		if (Input.GetMouseButton (0)) {
			shooting = true;
		}
	}

	void FixedUpdate () {
		myLines.enabled = false;
		if (shooting) {
			if (laserColor != Color.black) {
				bool bouncing = true;
				myLines.SetColors (Color.red, Color.red);
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
				while (bouncing && totalBounces < 50) {
					Debug.Log (totalBounces);
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
				myLines.SetVertexCount (points.Count);
				myLines.SetPositions (points.ToArray());
				myLines.enabled = true;
			}
		}
	}
}
