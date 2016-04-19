using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameObject playerObj;
	public GameObject shootObj;

	public PlayerController player;
//	public ShootingControl shooter;
	public ShootingControlBad shooter;

	public bool inColorMenu;
	public bool inPauseMenu;

	private float timeSinceSlowmoToggle;


	// Use this for initialization
	void Start () {
		player = playerObj.GetComponent<PlayerController> ();
		shooter = shootObj.GetComponent<ShootingControlBad> ();
		inColorMenu = false;
		inPauseMenu = false;

	}

	// Update is called once per frame
	void Update () {
		timeSinceSlowmoToggle += Time.deltaTime;
		if (Input.GetKey (KeyCode.Tab)) {
			if (!inColorMenu) {
				timeSinceSlowmoToggle = 0f;
			}
			inColorMenu = true;
		}
		else {
			if (inColorMenu) {
				timeSinceSlowmoToggle = 0f;
			}
			inColorMenu = false;
		}
		if (inColorMenu) {
			player.controlling = false;
			shooter.controlling = false;
			Time.timeScale = Mathf.SmoothStep (1f, 0.1f, timeSinceSlowmoToggle / 0.2f);
			Time.fixedDeltaTime = Time.timeScale * 0.02f;
		}
		else {
			player.controlling = true;
			shooter.controlling = true;
			Time.timeScale = Mathf.SmoothStep (Time.timeScale, 1f, timeSinceSlowmoToggle / 0.1f);
			Time.fixedDeltaTime = Time.timeScale * 0.02f;
		}
//		Debug.Log (timeSinceSlowmoToggle);

//		if (inColorMenu) {
//			if (Input.GetKeyDown (KeyCode.Alpha1)) {
//				player.cVals [1] ^= 1;
////				player.cVals [2] ^= 1;
//				player.refreshColor ();
//			}
//			if (Input.GetKeyDown (KeyCode.Alpha2)) {
////				player.cVals [0] ^= 1;
//				player.cVals [2] ^= 1;
//				player.refreshColor ();
//			}
//			if (Input.GetKeyDown (KeyCode.Alpha3)) {
//				player.cVals [0] ^= 1;
////				player.cVals [1] ^= 1;
//				player.refreshColor ();
//			}
//			if (Input.GetKeyDown (KeyCode.Alpha4)) {
//				shooter.cVals [0] ^= 1;
//				shooter.refreshColor ();
//			}
//			if (Input.GetKeyDown (KeyCode.Alpha5)) {
//				shooter.cVals [1] ^= 1;
//				shooter.refreshColor ();
//			}
//			if (Input.GetKeyDown (KeyCode.Alpha6)) {
//				shooter.cVals [2] ^= 1;
//				shooter.refreshColor ();
//			}
//		}
	}

	void OnGUI () {
		if (inColorMenu) {
			//			if (Input.GetKeyDown (KeyCode.Alpha1)) {
			GUI.color = Color.magenta;
			if (GUILayout.Button("MAGENTA", GUILayout.Width(150f), GUILayout.Height(50f))) {
				Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer ("Player"), LayerMask.NameToLayer ("" + player.cVals [0] + player.cVals [1] + player.cVals [2]), true);
				player.cVals [1] ^= 1;
				player.refreshColor ();
				Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer ("Player"), LayerMask.NameToLayer ("" + player.cVals [0] + player.cVals [1] + player.cVals [2]), false);
			}
			GUI.color = Color.yellow;
			if (GUILayout.Button("YELLOW", GUILayout.Width(150f), GUILayout.Height(50f))) {
				Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer ("Player"), LayerMask.NameToLayer ("" + player.cVals [0] + player.cVals [1] + player.cVals [2]), true);
				player.cVals [2] ^= 1;
				player.refreshColor ();
				Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer ("Player"), LayerMask.NameToLayer ("" + player.cVals [0] + player.cVals [1] + player.cVals [2]), false);
			}
			GUI.color = Color.cyan;
			if (GUILayout.Button("CYAN", GUILayout.Width(150f), GUILayout.Height(50f))) {
				Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer ("Player"), LayerMask.NameToLayer ("" + player.cVals [0] + player.cVals [1] + player.cVals [2]), true);
				player.cVals [0] ^= 1;
				player.refreshColor ();
				Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer ("Player"), LayerMask.NameToLayer ("" + player.cVals [0] + player.cVals [1] + player.cVals [2]), false);
			}
			GUI.color = Color.red;
			if (GUILayout.Button("RED", GUILayout.Width(150f), GUILayout.Height(50f))) {
				shooter.cVals [0] ^= 1;
				shooter.refreshColor ();
			}
			GUI.color = Color.green;
			if (GUILayout.Button("GREEN", GUILayout.Width(150f), GUILayout.Height(50f))) {
				shooter.cVals [1] ^= 1;
				shooter.refreshColor ();
			}
			GUI.color = Color.blue;
			if (GUILayout.Button("BLUE", GUILayout.Width(150f), GUILayout.Height(50f))) {
				shooter.cVals [2] ^= 1;
				shooter.refreshColor ();
			}
		}
	}


}
