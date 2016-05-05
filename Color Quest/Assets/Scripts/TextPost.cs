using UnityEngine;
using System.Collections;

public class TextPost : MonoBehaviour {

	public GameObject textObj;
	public GameObject bubbleObj;
	// Use this for initialization
	void Start () {
		textObj.SetActive (false);
		bubbleObj.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D (Collider2D collider) {
		if (collider.tag == "Player") {
			textObj.SetActive (true);
			bubbleObj.SetActive (true);
		}
	}
	void OnTriggerExit2D (Collider2D collider) {
		if (collider.tag == "Player") {
			textObj.SetActive (false);
			bubbleObj.SetActive (false);
		}
	}
}
