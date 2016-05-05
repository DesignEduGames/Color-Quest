using UnityEngine;
using System.Collections;

public class DoorScript : MonoBehaviour {
	public Vector3 originalLoc;
	public Vector3 targetLoc;
	public GameObject target;
	public bool activated;
	public float timeSinceActivation;


	// Use this for initialization
	void Start () {
		originalLoc = transform.position;
		targetLoc = target.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (activated) {
			timeSinceActivation += Time.deltaTime;
			transform.position = Vector3.Lerp (originalLoc, targetLoc, timeSinceActivation);
				
		}

	}

	public void moveAway () {
		activated = true;
	}
}
