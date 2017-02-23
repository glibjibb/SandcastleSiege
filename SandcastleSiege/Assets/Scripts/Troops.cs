using UnityEngine;
using System.Collections;

public class Troops : MonoBehaviour {

	public string enemyTag;
	private Vector3 target;
	private bool attacking;
	private GameObject attack;

	// Use this for initialization
	void Start () {
		target = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 move = target - this.transform.position;
		if (move.magnitude <= 1)
			move = new Vector3 (0, 0, 0);
		move.Normalize ();
		move /= 10;
		this.transform.Translate (move);
		if (attacking)
			Attack ();
	}

	void Attack() {
		foreach (Transform child in transform) {
			return;
		}
	}

	void OnCollisionEnter(Collision collide) {
		if (collide.gameObject.tag == enemyTag) {
			attacking = true;
			attack = collide.gameObject;
			Debug.Log ("attack");
		}

	}

	void OnCollisionExit(Collision collide) {
		attacking = false;
	}

	public void SetWayPoint(Vector3 point) {
		target = point;
	}

	public string GetHP() {
		string result = "";
		int i = 1;
		foreach (Transform child in transform) {
			result += "Troop " + i + ": " + child.gameObject.GetComponent<Troop> ().GetHealth() + "\n";
			i++;
		}
		return result;
	}
}
