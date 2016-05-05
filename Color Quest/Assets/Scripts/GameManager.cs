using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public GameObject playerObj;
	public GameObject shootObj;

	public PlayerController player;
//	public ShootingControl shooter;
	public ShootingControlBad shooter;

	public bool inColorMenu;
	public bool inPauseMenu;

	private float timeSinceSlowmoToggle;
	public Texture tex;
	private Texture2D pauseTex;
	private Texture2D healthTex;
	float timeSinceDeath;

	GUIStyle titleStyle;
	GUIStyle buttonStyle;

	public Font fontThing;



	private Color transparentWhite;


	// Use this for initialization
	void Start () {
		player = playerObj.GetComponent<PlayerController> ();
		shooter = shootObj.GetComponent<ShootingControlBad> ();
		inColorMenu = false;
		inPauseMenu = false;
		pauseTex = new Texture2D(1, 1);
		Color gray = new Color (0.1f, 0.1f, 0.1f, 0.7f);
		transparentWhite = new Color (1f, 1f, 1f, 0.5f);
		pauseTex.SetPixel(0,0,gray);
		pauseTex.Apply();

		healthTex = new Texture2D(1, 1);
		healthTex.SetPixel(0,0,Color.green);
		healthTex.Apply();



	}

	// Update is called once per frame
	void Update () {
		timeSinceSlowmoToggle += Time.deltaTime;
		if (Input.GetKeyDown (KeyCode.R)) {
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
		if ((Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.Tab)) && player.health > 0) {
			if (!inColorMenu) {
				timeSinceSlowmoToggle = 0f;
				shooter.cVals [0] = 0;
				shooter.cVals [1] = 0;
				shooter.cVals [2] = 0;
				shooter.refreshColor ();
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

	}

	void OnGUI () {
		if (titleStyle == null) {
			titleStyle = new GUIStyle (GUI.skin.label);
			titleStyle.fontSize = 25;
			titleStyle.font = fontThing;
		}

		if (player.health <= 0) {
			shooter.controlling = false;
			player.controlling = false;
			timeSinceDeath += Time.deltaTime;
			float trans = Mathf.Lerp (0f, 1f, timeSinceDeath / 4f);
			Color darker = new Color (0f, 0f, 0f, trans);
			pauseTex.SetPixel(0,0, darker);
			pauseTex.Apply();
			GUI.DrawTexture(new Rect(0,0,Screen.width, Screen.height), pauseTex);
			return;
			
		}

		if (inColorMenu) {

			GUILayout.BeginHorizontal ();

			GUILayout.BeginHorizontal (GUILayout.Width (Screen.width / 4));
			GUILayout.Label ("");
			GUILayout.EndHorizontal ();
			GUILayout.BeginVertical (GUILayout.Width (Screen.width / 4));

			GUILayout.BeginHorizontal (GUILayout.Height (Screen.height / 4));
			GUILayout.Label ("");
			GUILayout.EndHorizontal ();

			GUI.DrawTexture(new Rect(0,0,Screen.width, Screen.height), pauseTex);
			GUI.color = Color.white;
			GUILayout.Label ("PLAYER COLORS", titleStyle);
			if (player.cVals [1] == 0) {
				GUI.color = Color.magenta;
			}
			else {
				GUI.color = transparentWhite;
			}
			if (GUILayout.Button("MAGENTA", GUILayout.Width(150f), GUILayout.Height(50f))) {
				Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer ("Player"), LayerMask.NameToLayer ("" + player.cVals [0] + player.cVals [1] + player.cVals [2]), true);
				player.cVals [1] ^= 1;
				player.refreshColor ();
				Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer ("Player"), LayerMask.NameToLayer ("" + player.cVals [0] + player.cVals [1] + player.cVals [2]), false);
			}
			if (player.cVals [2] == 0) {
				GUI.color = Color.yellow;
			}
			else {
				GUI.color = transparentWhite;
			}
			if (GUILayout.Button("YELLOW", GUILayout.Width(150f), GUILayout.Height(50f))) {
				Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer ("Player"), LayerMask.NameToLayer ("" + player.cVals [0] + player.cVals [1] + player.cVals [2]), true);
				player.cVals [2] ^= 1;
				player.refreshColor ();
				Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer ("Player"), LayerMask.NameToLayer ("" + player.cVals [0] + player.cVals [1] + player.cVals [2]), false);
			}
			if (player.cVals [0] == 0) {
				GUI.color = Color.cyan;
			}
			else {
				GUI.color = transparentWhite;
			}
			if (GUILayout.Button("CYAN", GUILayout.Width(150f), GUILayout.Height(50f))) {
				Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer ("Player"), LayerMask.NameToLayer ("" + player.cVals [0] + player.cVals [1] + player.cVals [2]), true);
				player.cVals [0] ^= 1;
				player.refreshColor ();
				Physics2D.IgnoreLayerCollision (LayerMask.NameToLayer ("Player"), LayerMask.NameToLayer ("" + player.cVals [0] + player.cVals [1] + player.cVals [2]), false);
			}

			GUILayout.EndVertical ();
			GUILayout.BeginVertical (GUILayout.Width (Screen.width / 4));

			GUILayout.BeginHorizontal (GUILayout.Height (Screen.height / 4));
			GUILayout.Label ("");
			GUILayout.EndHorizontal ();
			GUI.color = Color.white;
			GUILayout.Label ("LASER COLORS", titleStyle);

			if (shooter.cVals [0] == 1) {
				GUI.color = Color.red;
			}
			else {
				GUI.color = transparentWhite;
			}
			if (GUILayout.Button("RED", GUILayout.Width(150f), GUILayout.Height(50f))) {
				shooter.cVals [0] ^= 1;
				shooter.refreshColor ();
			}
			if (shooter.cVals [1] == 1) {
				GUI.color = Color.green;
			}
			else {
				GUI.color = transparentWhite;
			}			
			if (GUILayout.Button("GREEN", GUILayout.Width(150f), GUILayout.Height(50f))) {
				shooter.cVals [1] ^= 1;
				shooter.refreshColor ();
			}
			if (shooter.cVals [2] == 1) {
				GUI.color = Color.blue;
			}
			else {
				GUI.color = transparentWhite;
			}			
			if (GUILayout.Button("BLUE", GUILayout.Width(150f), GUILayout.Height(50f))) {
				shooter.cVals [2] ^= 1;
				shooter.refreshColor ();
			}
			GUILayout.EndVertical ();
			GUILayout.EndHorizontal ();
			GUI.color = transparentWhite;

		}


		for (int i = 0; i < player.health; i++) {
			DrawQuad (new Rect(15 + 20*i, Screen.height - 50, 0.5f, 30.0f));
		}

	}

	void DrawQuad(Rect position) {
		GUI.skin.box.normal.background = healthTex;
		GUI.Box(position, GUIContent.none);
	}


}
