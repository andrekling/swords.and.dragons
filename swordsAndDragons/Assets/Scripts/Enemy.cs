using UnityEngine;
using System.Collections;

public class Enemy : Character {

	private GameObject player; // reference to the player
	private GameObject rules; // reference to game rules

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
	public int defenseSpot;// this is where we will store the number off the % to chose where to defend
	public int defensePlace;// this is the number where we defended to compare against the player attack

	Animator anim;
	int attackHash = Animator.StringToHash("HeadAttack");



	//for the defense
	//public int head;
	private int upBody;
	private int loBody;
	private int legs;
	private int noDef;

	//for the enemy Attack
	public int aggressivity;//this is how aggressive the enemy will be, will use it to randomly attack the player

	public float minWaitToAttack; // minimun time the monster will wait until attack the player
	public float maxWaitToAttack; // maximun time we will wait
	public float waitToAttack; // a random value between the min and the maximun, we will attack when the time since last player action is bigger than this.

	public int numberOfAttacks; // how many attacks can the enemy do, if he can do combos
	public int whereToAttack; // will use this to randomly choose a number from 0 to 100 to decide where to attack,
	// to define how likely it is to attack any of this places
	public int attackLegs;
	public int attackLoBody;
	public int attackUpBody;
	public int attackHead;

	public int damage;

	void Awake(){
		if (player == null) {
			player = GameObject.FindGameObjectWithTag ("Player");
		}
		if (rules == null) {
			rules = GameObject.FindGameObjectWithTag ("Rules");
		}
		anim = GetComponentInChildren<Animator> ();
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
				Attack();
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
		AttackOverTime ();

	}
	public void RandomCounter(){
		randomCounter ++;
	}

	public void DefenseCounter(){
		defenseCounter ++;
	}

	public void CheckDefense(){
		if (defensePlace == player.GetComponent<Player> ().attackPoint) {
			Debug.Log ("DEFENDED");
		} else {
			Debug.Log ("HIT");
		}
	}

	public void Defend(){
		defenseSpot = Random.Range (1, 101);
		DefenseSpot (defenseSpot);
		CheckDefense ();
	}
	void DefenseSpot (int defenseSpot){
		if (defenseSpot <= noDef) {
			defensePlace = 5;
			Debug.Log("no defense");
		} else if (defenseSpot >= noDef && defenseSpot <= legs) {
			defensePlace = 4;
			Debug.Log("legs attack");
		} else if (defenseSpot >= legs && defenseSpot <= loBody) {
			defensePlace = 3;
			Debug.Log("lower body attack");
		} else if (defenseSpot >= loBody && defenseSpot <= upBody) {
			defensePlace = 2;
			Debug.Log("upper body attack");
		} else if (defenseSpot > upBody ) {
			defensePlace = 1;
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

	void Attack(){
		Debug.Log ("ATTACKED");
		whereToAttack = Random.Range (0, 101);
		if (whereToAttack >= attackHead) {
			anim.SetBool(attackHash, true);
			Debug.Log ("Head Attack");
			} else if (whereToAttack > attackLoBody && whereToAttack < attackHead) {
			Debug.Log ("Upper Body Attack");
		} else if (whereToAttack > attackLegs && whereToAttack < attackUpBody) {
			Debug.Log ("Lowe Body Attack");
		} else {
			Debug.Log("Legs Attack");
		} 
	}

	void AttackOverTime(){
		if (player.GetComponent<Player> ().playerActionTimer > waitToAttack) {
			Debug.Log("Attacked over time");
			//Need to restart the timer
			Attack();
			PlayerTimerStop();
			PlayerTimerRestart();
		}
	}

	void PlayerTimerStop(){
		Debug.Log ("Will Stop the player timer!");
		rules.GetComponent<Logic> ().isRunningLastActionTimer = false;
		NewWaitTime ();
		rules.GetComponent<Logic> ().lastActionTimer = 0;
		//Debug.Log ("Will Restart the player timer!");
		//rules.GetComponent<codeLearning> ().isRunningLastActionTimer = true;
	}

	void PlayerTimerRestart(){
		Debug.Log ("Will Start the player timer!");
		rules.GetComponent<Logic> ().isRunningLastActionTimer = true;
	}

	void NewWaitTime(){
		waitToAttack = Random.Range (minWaitToAttack, maxWaitToAttack);
	}

	void SetIA(){
		switch (ia) {
		case 1:
			maxNumRandomDefenses = 6;
			maxNumDefenses = 4;
			minWaitToAttack = 0.5f;
			maxWaitToAttack = 3.5f;
			NewWaitTime();

			Debug.Log("IA is 1");
			//defense
			noDef = 20;
			legs = 40;
			loBody = 60;
			upBody = 80;


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
