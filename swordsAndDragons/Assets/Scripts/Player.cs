using UnityEngine;
using System.Collections;

public class Player : Character {

	#region Variables

	public int xp;
	public int xpToNextLevel;

	public int energy;// This will determine how many actions can be done
	private int maxEnergy;//The maximun energy the player can have
	public int recoveryEnergy;// how many points we recovery each time the recovery time is reached
	public float recoveryTime; // how long until we add again the recovery energy value

	public int attackSkills; // the total skill points
	public int levelAttackSkill; //How many points he has in skill per level
	public int weaponProficiency; // the level of proficiency on the specific weapon, it can be a specific long sword
	public int weaponClassProficiency;// the category proficiecy, it can be long swords
	//will do a basic combat system in here, will change later on
	public int maxDamage = 10;
	public int charStrenght = 3;
	

	private bool isRecovering = false;

	#endregion

	// Use this for initialization
	void Start () {

		energy = 20;
		maxEnergy = 20;
	}

	public int Damage(){
		int damage = Random.Range(1, maxDamage + 1);
		return damage ;
	}

	public void Attack (int attackType){
		//This class we will use to attack a mob, we receive the attackType that is received from where the player touched the sceen
		switch (attackType) {
		case 1 : 
			Damage();
			Debug.Log("Left Head Stab" );
			break;
		case 2:
			Debug.Log("Left Head to Up Torso Slash");
			break;
		case 3:
			Debug.Log("Left Head to Lower Torso Slash");
			break;
		case 4:
			Debug.Log("Left Head to Legs Long Slash");
			break;
		case 5:
			Debug.Log("Left Head to Right Head Slah");
			break;
		case 6:
			Debug.Log("Left Head to Right Up Torso Slash");
			break;
		case 7:
			Debug.Log("Left Head to Right Lower Torso Slash");
			break;
		case 8:
			Debug.Log("Left Head to Right Legs Long Slash");
			break;
		case 9:
			Debug.Log("Left Up Torso to Left Head Slash");
			break;
		case 10:
			Debug.Log("Left Up Torso Stab");
			break;
		case 11:
			Debug.Log("Left Up Torso to Lower Torso Slash");
			break;
		case 12:
			Debug.Log("Left Up Torso to Legs Slash");
			break;
		case 13:
			Debug.Log("Left Up Torso to Right Head Slah");
			break;
		case 14:
			Debug.Log("Left Up Torso to Right Up Torso Slash");
			break;
		case 15:
			Debug.Log("Left Up Torso to Right Lower Torso Slash");
			break;
		case 16:
			Debug.Log("Left Up Torso to Right Legs Slash");
			break;
		case 17:
			Debug.Log("Left Lower Torso to Left Head Slash");
			break;
		case 18:
			Debug.Log("Left Lower Torso to Left Up Torso Slash");
			break;
		case 19:
			Debug.Log("Lower Torso Stab");
			break;
		case 20:
			Debug.Log("Left Lower Torso to Legs Slash");
			break;
		case 21:
			Debug.Log("Left Lower Torso to Right Head Slah");
			break;
		case 22:
			Debug.Log("Left Lower Torso to Right Up Torso Slash");
			break;
		case 23:
			Debug.Log("Left Lower Torso to Right Lower Torso Slash");
			break;
		case 24:
			Debug.Log("Left Lower Torso to Right Legs Slash");
			break;
		case 25:
			Debug.Log("Left Lower Legs to Left Head Long Slash");
			break;
		case 26:
			Debug.Log("Left Lower Legs to Left Up Torso Slash");
			break;
		case 27:
			Debug.Log("Left Lower Legs to Lower Torso Slash");
			break;
		case 28:
			Debug.Log("Left Lower Legs Stab");
			break;
		case 29:
			Debug.Log("Left Lower Legs to Right Head Long Slah");
			break;
		case 30:
			Debug.Log("Left Lower Legs to Right Up Torso Slash");
			break;
		case 31:
			Debug.Log("Left Lower Legs to Right Lower Torso Slash");
			break;
		case 32:
			Debug.Log("Left Lower Legs to Right Legs Slash");
			break;
		case 33:
			Debug.Log("Right Head Stab");
			break;
		case 34:
			Debug.Log("Right Head to Up Torso Slash");
			break;
		case 35:
			Debug.Log("Right Head to Lower Torso Slash");
			break;
		case 36:
			Debug.Log("Right Head to Legs Long Slash");
			break;
		case 37:
			Debug.Log("Right Head to Right Head Slah");
			break;
		case 38:
			Debug.Log("Right Head to Right Up Torso Slash");
			break;
		case 39:
			Debug.Log("Right Head to Right Lower Torso Slash");
			break;
		case 40:
			Debug.Log("Right Head to Right Legs Long Slash");
			break;
		case 41:
			Debug.Log("Right Up Torso to Left Head Slash");
			break;
		case 42:
			Debug.Log("Right Up Torso to Left Up Torso Slash");
			break;
		case 43:
			Debug.Log("Right Up Torso to Lower Torso Slash");
			break;
		case 44:
			Debug.Log("Right Up Torso to Legs Slash");
			break;
		case 45:
			Debug.Log("Right Up Torso to Right Head Slah");
			break;
		case 46:
			Debug.Log("Right Up Torso Stab");
			break;
		case 47:
			Debug.Log("Right Up Torso to Right Lower Torso Slash");
			break;
		case 48:
			Debug.Log("Right Up Torso to Right Legs Slash");
			break;
		case 49:
			Debug.Log("Right Lower Torso to Left Head Slash");
			break;
		case 50:
			Debug.Log("Right Lower Torso to Left Up Torso Slash");
			break;
		case 51:
			Debug.Log("Right Lower Torso to Lower Torso Slash");
			break;
		case 52:
			Debug.Log("Right Lower Torso to Legs Slash");
			break;
		case 53:
			Debug.Log("Right Lower Torso to Right Head Slah");
			break;
		case 54:
			Debug.Log("Right Lower Torso to Right Up Torso Slash");
			break;
		case 55:
			Debug.Log("Right Lower Torso Stab");
			break;
		case 56:
			Debug.Log("Right Lower Torso to Right Legs Slash");
			break;
		case 57:
			Debug.Log("Right Lower Legs to Left Head Long Slash");
			break;
		case 58:
			Debug.Log("Right Lower Legs to Left Up Torso Slash");
			break;
		case 59:
			Debug.Log("Right Lower Legs to Lower Torso Slash");
			break;
		case 60:
			Debug.Log("Right Lower Legs to Left Lower Legs Slash");
			break;
		case 61:
			Debug.Log("Right Lower Legs to Right Head Long Slah");
			break;
		case 62:
			Debug.Log("Right Lower Legs to Right Up Torso Slash");
			break;
		case 63:
			Debug.Log("Right Lower Legs to Right Lower Torso Slash");
			break;
		case 64:
			Debug.Log("Right Lower Legs Stab");
			break;
			}

		}

	public void EnergyRecover(){

			StartCoroutine (RecoveryEnergy());

	}
	void CalculateAttackSlills(){
		attackSkills = weaponProficiency + weaponClassProficiency + levelAttackSkill;
	}

	IEnumerator RecoveryEnergy(){
		isRecovering = true;
	
		// we loop this function until the energy is the same as the maximun energy
		while( energy != maxEnergy ){
			yield return new WaitForSeconds(recoveryTime);
			energy += recoveryEnergy;
			Debug.Log(energy);

			
		}
		yield return null;
		Debug.Log("Energy recovered");
		isRecovering = false;
	}
}
