using UnityEngine;
using System.Collections;

public class Player : Character {

	#region Variables

	public int xp;
	public int xpToNextLevel;
	public int attackCount = 0;
	public int energy;// This will determine how many actions can be done

	public bool isRecovering = false;// this is to control if the recovering energy coroutine should run or not
	private int maxEnergy;//The maximun energy the player can have
	public int recoveryEnergy = 1;// how many points we recovery each time the recovery time is reached
	public float recoveryTime = 0.5f; // how long until we add again the recovery energy value

	public int attackSkills = 1; // the total skill points
	public int levelAttackSkill = 1; //How many points he has in skill per level
	public int weaponProficiency = 1; // the level of proficiency on the specific weapon, it can be a specific long sword
	public int weaponClassProficiency = 1;// the category proficiecy, it can be long swords
	//will do a basic combat system in here, will change later on
	public int maxDamage = 10;
	public int charStrenght = 3;
	public int attackPoint = 5;//where we attacked

	public GameObject enemy; // later we will need to change this to a list of enemies and get one from the list, but lets do this later.

	public float playerActionTimer = 0.0f;//this variable to hold the timer since last action


	



	#endregion
	void Awake(){
		if (enemy == null) {
			enemy = GameObject.FindGameObjectWithTag("Enemy");
		}
	}

	// Use this for initialization
	void Start () {

		energy = 20;
		maxEnergy = 20;
		SetAttackSkills ();
	}
	void Update(){
		if (Input.GetKeyDown (KeyCode.A)) {
			Attack(1);
		}
		EnergyRecover ();
	}

	public int Damage(){
		int damage = Random.Range(1, maxDamage + 1);
		Debug.Log ("The damage withut the Quality of Attack is :" + damage);
		return damage ;
	}

	public float QualityOfAttack(){

		int attackNum = Random.Range (1, 101);
		if (attackNum == 100) {
			Debug.Log (attackNum);
			Debug.Log ("MAXIMUN Exelent Attack");
			return 4;
		}
		else if (attackNum == 1) {
			Debug.Log (attackNum);
			Debug.Log ("MAXIMUN Fail");
			return 0.1f;
		}
		else if (attackNum > (100 - (int)(attackSkills * 0.3))) {
			Debug.Log (attackNum);
			Debug.Log ("Exelent Attack");
			return 4;
		} else if (attackNum > (100 - attackSkills)) {
			Debug.Log (attackNum);
			Debug.Log ("Good Attack");
			return 2;
		} else if (attackNum < (int)(10 - (attackSkills * 0.5f))) {
			Debug.Log (attackNum);
			Debug.Log ("Very Bad");
			return 0.1f;
		} else if (attackNum < (int)((100 - attackSkills) * 0.5f)) {
			Debug.Log (attackNum);
			Debug.Log ("Bad");
			return 0.5f;
		} else {
			Debug.Log (attackNum);
			Debug.Log ("Normal Attack");
			return 1;
		}
	}

	void SetAttackSkills (){
	//will set the attack skills needed for the combat system, we will run this function when we update our character
		attackSkills =(int)( levelAttackSkill + (weaponClassProficiency + weaponProficiency) * 0.5);
		Debug.Log ("For maximun attack must be bigger than" + (100 - (int)(attackSkills * 0.3)));
		Debug.Log ("For good attack must be bigger than" + (100 - attackSkills ));
		Debug.Log ("For Bad attack must be smaller than" + ((int)((100 - attackSkills) * 0.5f)));
		Debug.Log ("For Very Bad attack must be smaller than" + ((int)(10 - (attackSkills * 0.5f))));
	}

	public void Attack (int attackType){
		if(enemy.GetComponent<Enemy>().isRandomlyDefending == true ){
			enemy.GetComponent<Enemy>().RandomCounter();
		}
		if(enemy.GetComponent<Enemy>().isDefending == true ){
			enemy.GetComponent<Enemy> ().DefenseCounter ();
		}


		attackCount++;
		int damage;
		//This class we will use to attack a mob, we receive the attackType that is received from where the player touched the sceen
		switch (attackType) {
		case 1 : 
			attackPoint = 1;
			Debug.Log("Left Head Stab" );
			energy = energy - 3;
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			if(enemy.GetComponent<Enemy>().isDefending == false){
				enemy.GetComponent<Enemy>().Defend();
				if(attackPoint != enemy.GetComponent<Enemy>().defensePlace ){
					enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
					enemy.GetComponent<Enemy>().Death();
				}
			}
			break;
		case 2:
			attackPoint = 2;
			Debug.Log("Left Head to Up Torso Slash");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 3:
			attackPoint = 3;
			Debug.Log("Left Head to Lower Torso Slash");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 4:
			attackPoint = 4;
			Debug.Log("Left Head to Legs Long Slash");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 5:
			attackPoint = 1;
			Debug.Log("Left Head to Right Head Slah");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 6:
			attackPoint = 2;
			Debug.Log("Left Head to Right Up Torso Slash");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 7:
			attackPoint = 3;
			Debug.Log("Left Head to Right Lower Torso Slash");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 8:
			attackPoint = 4;
			Debug.Log("Left Head to Right Legs Long Slash");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 9:
			Debug.Log("Left Up Torso to Left Head Slash");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 10:
			Debug.Log("Left Up Torso Stab");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 11:
			Debug.Log("Left Up Torso to Lower Torso Slash");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 12:
			Debug.Log("Left Up Torso to Legs Slash");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 13:
			Debug.Log("Left Up Torso to Right Head Slah");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 14:
			Debug.Log("Left Up Torso to Right Up Torso Slash");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 15:
			Debug.Log("Left Up Torso to Right Lower Torso Slash");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 16:
			Debug.Log("Left Up Torso to Right Legs Slash");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 17:
			Debug.Log("Left Lower Torso to Left Head Slash");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 18:
			Debug.Log("Left Lower Torso to Left Up Torso Slash");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 19:
			Debug.Log("Lower Torso Stab");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 20:
			Debug.Log("Left Lower Torso to Legs Slash");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 21:
			Debug.Log("Left Lower Torso to Right Head Slah");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 22:
			Debug.Log("Left Lower Torso to Right Up Torso Slash");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 23:
			Debug.Log("Left Lower Torso to Right Lower Torso Slash");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 24:
			Debug.Log("Left Lower Torso to Right Legs Slash");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 25:
			Debug.Log("Left Lower Legs to Left Head Long Slash");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 26:
			Debug.Log("Left Lower Legs to Left Up Torso Slash");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 27:
			Debug.Log("Left Lower Legs to Lower Torso Slash");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 28:
			Debug.Log("Left Lower Legs Stab");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 29:
			Debug.Log("Left Lower Legs to Right Head Long Slah");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 30:
			Debug.Log("Left Lower Legs to Right Up Torso Slash");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 31:
			Debug.Log("Left Lower Legs to Right Lower Torso Slash");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 32:
			Debug.Log("Left Lower Legs to Right Legs Slash");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 33:
			Debug.Log("Right Head Stab");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 34:
			Debug.Log("Right Head to Up Torso Slash");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 35:
			Debug.Log("Right Head to Lower Torso Slash");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 36:
			Debug.Log("Right Head to Legs Long Slash");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 37:
			Debug.Log("Right Head to Right Head Slah");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 38:
			Debug.Log("Right Head to Right Up Torso Slash");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 39:
			Debug.Log("Right Head to Right Lower Torso Slash");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 40:
			Debug.Log("Right Head to Right Legs Long Slash");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 41:
			Debug.Log("Right Up Torso to Left Head Slash");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 42:
			Debug.Log("Right Up Torso to Left Up Torso Slash");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 43:
			Debug.Log("Right Up Torso to Lower Torso Slash");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 44:
			Debug.Log("Right Up Torso to Legs Slash");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 45:
			Debug.Log("Right Up Torso to Right Head Slah");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 46:
			Debug.Log("Right Up Torso Stab");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 47:
			Debug.Log("Right Up Torso to Right Lower Torso Slash");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 48:
			Debug.Log("Right Up Torso to Right Legs Slash");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 49:
			Debug.Log("Right Lower Torso to Left Head Slash");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 50:
			Debug.Log("Right Lower Torso to Left Up Torso Slash");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 51:
			Debug.Log("Right Lower Torso to Lower Torso Slash");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 52:
			Debug.Log("Right Lower Torso to Legs Slash");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 53:
			Debug.Log("Right Lower Torso to Right Head Slah");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 54:
			Debug.Log("Right Lower Torso to Right Up Torso Slash");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 55:
			Debug.Log("Right Lower Torso Stab");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 56:
			Debug.Log("Right Lower Torso to Right Legs Slash");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 57:
			Debug.Log("Right Lower Legs to Left Head Long Slash");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 58:
			Debug.Log("Right Lower Legs to Left Up Torso Slash");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 59:
			Debug.Log("Right Lower Legs to Lower Torso Slash");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 60:
			Debug.Log("Right Lower Legs to Left Lower Legs Slash");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 61:
			Debug.Log("Right Lower Legs to Right Head Long Slah");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 62:
			Debug.Log("Right Lower Legs to Right Up Torso Slash");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 63:
			Debug.Log("Right Lower Legs to Right Lower Torso Slash");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
		case 64:
			Debug.Log("Right Lower Legs Stab");
			damage = (int)(Damage() * QualityOfAttack());
			if(damage == 0){damage = 1;}
			Debug.Log("Damage is :" + damage);
			
			enemy.GetComponent<Enemy>().life = enemy.GetComponent<Enemy>().life - damage;
			enemy.GetComponent<Enemy>().Death();

			break;
			}

		}

	public void EnergyRecover(){
		if (isRecovering != true) {
			if (energy < maxEnergy) {
				isRecovering = true;
				StartCoroutine (RecoveryEnergy ());
			}
		}
	}
	void CalculateAttackSlills(){
		attackSkills = weaponProficiency + weaponClassProficiency + levelAttackSkill;
	}

	IEnumerator RecoveryEnergy(){

	
			// we loop this function until the energy is the same as the maximun energy
			while (energy != maxEnergy) {
				yield return new WaitForSeconds (recoveryTime);
				energy += recoveryEnergy;
				Debug.Log (energy);

			
			}
			yield return null;
			//Debug.Log("Energy recovered");
			isRecovering = false;

	}
}
