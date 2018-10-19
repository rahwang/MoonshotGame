using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public static int player1_score;
	public static int player2_score;

	public float speed = 6;
	public GameObject moon_prefab; 
	public GUISkin layout;
	
	private GameObject moon;
	private static GameObject planet1;
	private static GameObject planet2;

	private Rigidbody2D rb1;
	private Rigidbody2D rb2;

	private AudioSource explosionfx;
	private bool isSoloGame = false;

	// Use this for initialization
	void Start () {
		explosionfx = gameObject.GetComponent<AudioSource> ();
		planet1 = GameObject.Find ("Planet1");
		planet2 = GameObject.Find ("Planet2");
		rb1 = planet1.GetComponent<Rigidbody2D>();
		rb2 = planet2.GetComponent<Rigidbody2D>();


		// Check if it's a solo game. If so, disable player 1.
		if (PlayerPrefs.GetString ("moonshot_num_players") == "solo") {
			planet2.GetComponent<PlanetAI> ().enabled = true;
			isSoloGame = true;
		}

		moon = (GameObject) Instantiate(moon_prefab, new Vector2(0, 0), Quaternion.identity);
		player1_score = 0;
		player2_score = 0;
	}

	public Vector2 GetMoonPos() {
		if (moon) {
			return moon.transform.position;
		} else {
			return Vector2.zero;
		}
	}

	public Vector2 GetMoonVel() {
		if (moon) {
			return moon.GetComponent<Rigidbody2D> ().velocity;
		} else {
			return Vector2.zero;
		}
	}

	public void Score (string goal_name) {
		if (goal_name == "Planet1" || goal_name == "Planet2") {
			explosionfx.Play ();
		}

		if (goal_name == "Goal1" || goal_name == "Planet1") {
			player2_score++;
		} else if (goal_name == "Goal2" || goal_name == "Planet2") {
			player1_score++;
		}

		if (player1_score >= 10 || player2_score >= 10) {
			GameObject.Destroy (moon);
			Invoke ("LoadMenu", 3.0f);
		} else {
			GameObject.Destroy (moon);
			Invoke ("Reset", 2.0f);
		}
	}

	void UpdatePlanets () {
		Vector2 player1_direction = new Vector2 (0.0f, 0.0f);
		Vector2 player2_direction = new Vector2 (0.0f, 0.0f);

		if (Input.GetKey ("up"))
			player1_direction += (new Vector2(0.0f, 1.0f));
		if (Input.GetKey ("down"))
			player1_direction += (new Vector2(0.0f, -1.0f));
		if (Input.GetKey ("left"))
			player1_direction += (new Vector2(-1.0f, 0.0f));
		if (Input.GetKey ("right"))
			player1_direction += (new Vector2(1.0f, 0.0f));

		if (!isSoloGame) {
			if (Input.GetKey ("w"))
				player2_direction += (new Vector2 (0.0f, 1.0f));
			if (Input.GetKey ("s"))
				player2_direction += (new Vector2 (0.0f, -1.0f));
			if (Input.GetKey ("a"))
				player2_direction += (new Vector2 (-1.0f, 0.0f));
			if (Input.GetKey ("d"))
				player2_direction += (new Vector2 (1.0f, 0.0f));
		}

		rb1.velocity = player1_direction.normalized * speed;
		rb2.velocity = player2_direction.normalized * speed;
	}

	public void LoadMenu() {
		Application.LoadLevel ("menu");
	}

	void Reset () {
		moon = (GameObject) Instantiate(moon_prefab, new Vector2(0, 0), Quaternion.identity);
		planet1.transform.position = new Vector2 (-4.75f, 0.0f);
		planet2.transform.position = new Vector2 (4.75f, 0.0f);
	}

	void OnGUI () {
		GUI.skin = layout;
		GUI.Label(new Rect(Screen.width / 2 - 150 - 12, 20, 100, 100), "" + player1_score);
		GUI.Label(new Rect(Screen.width / 2 + 150 + 12, 20, 100, 100), "" + player2_score);

		if (player1_score >= 10) {
			if (isSoloGame) {
				GUI.Label (new Rect (Screen.width / 2 - 170, Screen.height / 2 - 20, 2000, 1000), "THE COMPUTER WINS");
			} else {
				GUI.Label (new Rect (Screen.width / 2 - 170, Screen.height / 2 - 20, 2000, 1000), "PLAYER ONE WINS");
			}
		} else if (player2_score >= 10) {
			if (isSoloGame) {
				GUI.Label (new Rect (Screen.width / 2 - 170, Screen.height / 2 - 20, 2000, 1000), "THE HUMAN WINS");
			} else {
				GUI.Label (new Rect (Screen.width / 2 - 170, Screen.height / 2 - 20, 2000, 1000), "PLAYER TWO WINS");
			}
		}
	}

	void DetectQuit() {
		if (Input.GetKey("escape")) {
			GameObject.Find("GameManager").GetComponent<GameManager>().LoadMenu();
		}
	}

	void Update() {
		UpdatePlanets();
		DetectQuit();
	}
}
