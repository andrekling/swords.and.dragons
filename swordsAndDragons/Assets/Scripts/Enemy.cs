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

	// for the random defenses
	public bool isRandomlyDefending = true;
	public int maxNumRandomDefenses; // gave by IA
	public int howManyRandom; //How many times it will be random
	public int randomCounter = 0; //The counter
	public int defenseSpot;

	//for the defense
	//public int head;
	public int upBody;
	public int loBody;
	public int legs;
	public int noDef;

	void Awake(){
		if (player == null) {
			player = GameObject.FindGameObjectWithTag ("Player");
		}
	}

	void Start(){
		SetIA ();
		SetRandomlyDefenses(maxNumRandomDefenses);
		}

	void Update(){
		//For the random Loop
		if(isRandomlyDefending == true && isDefending == false){
			//Debug.Log("Randoming");
			if(randomCounter == howManyRandom){
				SetRandomDefenses(maxNumDefenses);
				randomCounter = 0;
				Debug.Log("No longer RandomlyDefending");
				isDefending = true;
				isRandomlyDefending = false;
			}

		}
		if(isRandomlyDefending == false && isDefending == true){
			//Debug.Log("Defending");
			if (defenseCounter == howManyDefenses) {
				SetRandomlyDefenses(maxNumRandomDefenses);
				defenseCounter = 0;
				Debug.Log("No longer defending");
				isDefending = false;
				isRandomlyDefending = true;

			}
				
		}

	}
	public void RandomCounter(){
		randomCounter ++;
	}

	public void DefenseCounter(){
		defenseCounter ++;
	}

	public void Defend(){
		defenseSpot = Random.Range (1, 101);
		DefenseSpot (defenseSpot);
	}
	void DefenseSpot (int defenseSpot){
		if (defenseSpot <= noDef) {
			Debug.Log("no defense");
		} else if (defenseSpot >= noDef && defenseSpot <= legs) {
			Debug.Log("legs attack");
		} else if (defenseSpot >= legs && defenseSpot <= loBody) {
			Debug.Log("lower body attack");
		} else if (defenseSpot >= loBody && defenseSpot <= upBody) {
			Debug.Log("upper body attack");
		} else if (defenseSpot > upBody ) {
			Debug.Log("head attack");
		}
	}


	public int SetRandomDefenses(int maxNumDefenses){
		howManyDefenses = Random.Range (0, maxNumDefenses + 1);
		return howManyDefenses;
	}

	public int SetRandomlyDefenses(int maxNumRandomDefenses ){
		howManyRandom = Random.Range (0, maxNumRandomDefenses + 1);
		return howManyRandom;
	}

	void SetIA(){
		switch (ia) {
		case 1:
			maxNumRandomDefenses = 6;
			maxNumDefenses = 4;
			Debug.Log("IA is 1");
			//defense
			noDef = 20;
			legs = 40;
			loBody = 60;
			upBody = 80;
			//head = 100;

			break;
		case 2:
			maxNumRandomDefenses = 4;
			maxNumDefenses = 15;
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
