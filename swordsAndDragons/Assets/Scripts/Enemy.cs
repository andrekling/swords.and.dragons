using UnityEngine;
using System.Collections;

public class Enemy : Character {


	public int defense = 5;

	public void Death(){
		if (life < 0) {
			Destroy (this.gameObject);
		}
	}
}
