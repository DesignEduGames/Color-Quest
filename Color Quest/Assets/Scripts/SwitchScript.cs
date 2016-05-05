using UnityEngine;
using System.Collections;

public class SwitchScript : MonoBehaviour {
	public Color myColor;
	public int [] cVals = new int[3];
	private SpriteRenderer mySprite;
	private LineRenderer myLine;
	private DoorScript myDoor;
	bool activated;
	public GameObject door;

	// Use this for initialization
	void Start () {
		mySprite = GetComponent<SpriteRenderer> ();
		myDoor = door.GetComponent<DoorScript> ();
		myLine = GetComponent<LineRenderer> ();
		//		myLines = GetComponent<LineRenderer> ();
		cVals [0] = Mathf.RoundToInt (myColor.r);
		cVals [1] = Mathf.RoundToInt (myColor.g);
		cVals [2] = Mathf.RoundToInt (myColor.b);


		Color tg = new Color (0f, 0f, 0f, 0.4f);
		myLine.SetColors (tg, tg);
		myLine.SetWidth (0.2f, 0.2f);
		myLine.SetVertexCount (2);
		Vector3[] arr = new Vector3[] { transform.position, door.transform.position };
		myLine.SetPositions(arr);
		myLine.enabled = false;


		refreshColor ();
	}

	// Update is called once per frame
	void Update () {
	
	}


	public void absorbColor (int[] inCVals) {

		for (int i = 0; i < 3; i++) {
			if (inCVals [i] != cVals [i]) {
				return;
			}
		}
		myDoor.moveAway ();
		activated = true;
		myLine.enabled = false;

	}

	public void refreshColor () {
		myColor = new Color (cVals [0], cVals [1], cVals [2]);
		mySprite.color = myColor;
	}

	void OnMouseEnter() {
		if (!activated) {
			Debug.Log ("MOUSE OVER" + Time.fixedTime);
			myLine.enabled = true;
		}
	}
	void OnMouseExit() {
		if (!activated) {
			myLine.enabled = false;
		}
	}
}
