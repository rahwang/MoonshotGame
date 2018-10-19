using UnityEngine;
using System.Collections;

public class DetectMoon : MonoBehaviour {

	private GameManager gameManager;

	// Use this for initialization
	void Start () {
		gameManager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
	}

	void OnCollisionEnter2D (Collision2D coll) {
		if(coll.collider.CompareTag("Ball")){
			ExplosionSpawn.Instance.Explosion (transform.position, transform);
			gameManager.Score (gameObject.name);
		}
	}
}
