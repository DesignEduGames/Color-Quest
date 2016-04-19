using UnityEngine;
using System.Collections;

public class PlatformScript : MonoBehaviour {


	public Color myColor;
	public int [] cVals = new int[3];
	public int [] lcVals = new int[3];
	public bool passThroughAble;
//	private LineRenderer myLines;
	private SpriteRenderer mySprite;

//	public Color laserColor;

	// Use this for initialization
	void Start () {
		mySprite = GetComponent<SpriteRenderer> ();
//		myLines = GetComponent<LineRenderer> ();


		cVals [0] = Mathf.RoundToInt (myColor.r);
		cVals [1] = Mathf.RoundToInt (myColor.g);
		cVals [2] = Mathf.RoundToInt (myColor.b);


		refreshColor ();
//		laserColor = Color.black;

	}
	
	// Update is called once per frame
	void Update () {
//		myLines.enabled = false;
	}

	public void refreshColor () {
		myColor = new Color (cVals [0], cVals [1], cVals [2]);
		mySprite.color = myColor;
	}

	public int [] reflectColor (int[] inCVals) {
		int[] ret = new int[3];
		for (int i = 0; i < 3; i++) {
			ret [i] = inCVals [i] * cVals [i];
		}
//		Debug.Log (inCVals);
		return ret;
	}
//		
//
//	public void reflectLaser (Vector2 start, Vector2 dir, Color hitColor) {
//		myLines.SetPosition (0, start + dir * 0.01f);
//		myLines.SetVertexCount (2);
//		myLines.SetColors (hitColor, hitColor);
//		myLines.SetWidth (0.1f, 0.1f);
//		RaycastHit2D bounceHit = Physics2D.Raycast (start + dir * 0.01f, dir, 100f, ~(1 << LayerMask.NameToLayer("Player")));
//		if (bounceHit.collider == null) {
//			myLines.SetPosition (1, start + dir * 100f);
//		}
//		else {
//			myLines.SetPosition (1, bounceHit.point);
//			Vector3 nextDir = (Vector2.Reflect (dir, bounceHit.normal)).normalized;
//			bounceHit.collider.gameObject.GetComponent<platformScript> ().reflectLaser (bounceHit.point, nextDir, hitColor);
//		}
//		myLines.enabled = true;
//	}
}
