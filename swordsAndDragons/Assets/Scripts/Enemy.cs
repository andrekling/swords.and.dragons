using UnityEngine;
using System.Collections;

public class Enemy : Character {

	public GameObject player; // reference to the player
	public int defense = 5;
	public int ia = 1;
	public bool isDefending = false;
	public int maxNumDefenses; // will use this to set the maximun number of the random for the defense 
	public int howManyDefenses; // this is the number sorted by the ia of how many times it will defend
	public int defenseCounter = 0; // this is the count of defenses made.

	void Awake(){
		if (player == null) {
			player = GameObject.FindGameObjectWithTag ("Player");
		}
	}

	void Start(){
		SetIA ();
	}

	void Update(){
		if (defenseCounter == howManyDefenses) {
			isDefending = false;
			defenseCounter = 0;
			Debug.Log("No longer defending");
		}
		if(isDefending == false){
			SetRandomDefenses(maxNumDefenses);

			isDefending = true;
			}

		DefenseCounter();
	}

	void DefenseCounter(){
		defenseCounter = player.GetComponent<Player>().attackCount;
	}

	void Defend(){
		while (defenseCounter != maxNumDefenses) {
		}
	}

	void SetIA(){
		switch (ia) {
		case 1:
			maxNumDefenses = 4;
			Debug.Log("IA is 1");
			break;
		case 2:
			maxNumDefenses = 15;
			Debug.Log("IA is 2");
			break;
		default:
			Debug.Log("IA ERROR");
			break;
		}
	}

	public int SetRandomDefenses(int maxNumDefenses){
		howManyDefenses = Random.Range (0, maxNumDefenses + 1);
		return howManyDefenses;
	}

	public void Death(){
		if (life < 0) {
			Destroy (this.gameObject);
		}
	}
}
