using UnityEngine;
using System.Collections;

public class toggleColor : MonoBehaviour {

	private SpriteRenderer mySprite;
	public bool inactive { get; set; }

	// Use this for initialization
	void Start () {
		mySprite = GetComponent<SpriteRenderer> ();
		inactive = true;
		if (inactive) {
			setInactive ();
		} else {
			setActive ();
		}
	}
	
	// Update is called once per frame
//	void Update () {
//	
//	}

	public void setActive(){
		mySprite.color = new Color(mySprite.color.r, mySprite.color.g, mySprite.color.b, 1.0f);
		inactive = false;
	}

	public void setInactive(){
		mySprite.color = new Color(mySprite.color.r, mySprite.color.g, mySprite.color.b, 0.2f);
		inactive = true;
	}
}
