using UnityEngine;
using System.Collections;

public class MoonControl : MonoBehaviour {

	private GameObject planet1;
	private GameObject planet2;
	private Rigidbody2D rb;
	private Vector2 vel;

	public float maxSpeed = 5.0f;
	public float initialSpeed = 5.0f;
	public float forceScale = 1.0f;
	private AudioSource pingSound;

	// Use this for initialization
	void Start () {
		planet1 = GameObject.Find ("Planet1");
		planet2 = GameObject.Find ("Planet2");

		rb = GetComponent<Rigidbody2D>();
		pingSound = GetComponent<AudioSource> ();
		Spawn ();
	}

	void ApplyGravity(GameObject planet) {
		float distance = Vector2.Distance (planet.transform.position, transform.position);
		Vector2 direction = (planet.transform.position - transform.position).normalized;
		Vector2 gravity = (rb.mass * direction * forceScale) / distance;
		rb.AddForce (gravity);	
	}

	// Update is called once per frame
	void Update () {

		ApplyGravity(planet1);
		ApplyGravity(planet2);

		// Clamp velocity
		if (Vector3.Magnitude(rb.velocity) > maxSpeed) {
			rb.velocity = rb.velocity.normalized * maxSpeed;
		}
	}

	public void Spawn () {
		float x = Random.Range(0.0f, 2.0f) - 1.0f;
		float y = Random.Range(0.0f, 2.0f) - 1.0f;
		Vector2 init_dir = new Vector2 (x, y);
		init_dir.Normalize ();

		rb.AddForce(init_dir * initialSpeed);
	}

	public void Reset () {
		gameObject.transform.position = new Vector2 (0.0f, 0.0f);
		vel.x = 0.0f;
		vel.y = 0.0f;
		rb.velocity = vel;
		Invoke("Spawn", 0.8f);
	}

	void OnTriggerEnter2D () {
		pingSound.Play ();
	}
}