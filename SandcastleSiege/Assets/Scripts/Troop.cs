using UnityEngine;
using System.Collections;

public class Troop : MonoBehaviour {

	private int hp = 100;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void TakeDmg(int dmg) {
		hp -= dmg;
	}

	public int GetHealth(){
		return hp;
	}
}
