using UnityEngine;
using System.Collections;

public class platformScript : MonoBehaviour {


	public Color myColor;
	public int [] cVals = new int[3];
	public bool passThroughAble;
	private SpriteRenderer mySprite;

	// Use this for initialization
	void Start () {
		mySprite = GetComponent<SpriteRenderer> ();
		cVals [0] = Mathf.RoundToInt (myColor.r);
		cVals [1] = Mathf.RoundToInt (myColor.g);
		cVals [2] = Mathf.RoundToInt (myColor.b);
		refreshColor ();


	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void refreshColor () {
		myColor = new Color (cVals [0], cVals [1], cVals [2]);
		mySprite.color = myColor;
	}
}
