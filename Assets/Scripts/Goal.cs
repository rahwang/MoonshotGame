using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {

	private GameManager game_manager;

	// Use this for initialization
	void Start () {
		game_manager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnTriggerEnter2D (Collider2D hit_info) {
		if (hit_info.tag == "Ball") {
			string goal_name = transform.name;
			game_manager.Score (goal_name);
		}
	}
}
