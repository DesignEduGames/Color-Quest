using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {

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

	public void absorbColor (int[] inCVals) {

		for (int i = 0; i < 3; i++) {
			if (inCVals [i] != cVals [i]) {
				return;
			}
		}
		Destroy(gameObject);
	}
}
