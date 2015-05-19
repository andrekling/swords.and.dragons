using UnityEngine;
using System.Collections;

public class Enemy : Character {


	public int defense = 5;
	public int ia = 1;

	void Start(){
		SetIA ();
	}

	void SetIA(){
		switch (ia) {
		case 1:
			Debug.Log("IA is 1");
			break;
		case 2:
			Debug.Log("IA is 2");
			break;
		default:
			Debug.Log("IA ERROR");
			break;
		}
	}

	public void Death(){
		if (life < 0) {
			Destroy (this.gameObject);
		}
	}
}
