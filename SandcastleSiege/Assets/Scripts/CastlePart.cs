using UnityEngine;
using System.Collections;


public class CastlePart : MonoBehaviour {

	public enum Parts { Wall, TowerR, TowerS};
	public Parts partType;
	public float health;


	// Use this for initialization
	void Start () {
		if (partType == Parts.Wall)
			health = 250;
		else
			health = 100;
	}
	
	// Update is called once per frame
	void Update () {
		if (health <= 0)
			Object.Destroy (this.gameObject);
	}

	public float GetHealth() {
		return health;
	}

	public void ChangeSize(float f) {
		if (partType == Parts.Wall && (transform.localScale.x + f > 0) && (transform.localScale.x + f < 25)) {
			transform.localScale += new Vector3 (f, 0, 0);
			health += f * 10;
		}
		if ((partType == Parts.TowerR || partType == Parts.TowerS) && (transform.localScale.x + f > 0) && (transform.localScale.x + f < 8)) {
			transform.localScale += new Vector3 (f, 0, f);
			health += f * 25;
		}
	}

	public void ChangeRotate(float f) {
		transform.Rotate (new Vector3 (0, f, 0));
	}


	public void TakeDamage(float dmg) {
		health -= dmg;
		AdjustHeight ();
		StartCoroutine ("FlashRed");
	}

	IEnumerator FlashRed() {
		Color df = this.GetComponent<Renderer>().material.color;
		this.GetComponent<Renderer> ().material.color = Color.red;
		yield return new WaitForSeconds (0.25f);
		this.GetComponent<Renderer> ().material.color = df;
	}

	void AdjustHeight() {
		if (partType == Parts.TowerR || partType == Parts.TowerS) {
			transform.position = (new Vector3 (transform.position.x, ((health / 100) * 6) - 3, transform.position.z));
		}
		if (partType == Parts.Wall) {
			transform.position = (new Vector3 (transform.position.x, ((health / 250) * 6) - 3, transform.position.z));
		}
	}
}
