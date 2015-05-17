using UnityEngine;
using System.Collections;
using System.Collections.Generic; //must add to use a list

public class codeLearning : MonoBehaviour {

	public GameObject player;

	public float playerActionTimer = 0.0f;//this variable to calculate the time since last player action
	public float lastPlayerActionTimer = 0.0f;//This is where we will store the last action time

	public int divisionScreenX;
	public int divisionScreenY;
	public int swipeZone2Confort;
	public int yGameZones = 4;
	public int diagonalSafeZone;

	//Last Action from player timer
	public bool isRunningLastActionTimer;
	public float lastActionTimer = 0.0f;
	public float stoppedLastActionTimer;

	//Attack part check if the touch can be used or not
	public bool isRunningClock;
	public bool isRunningAttackCoroutine;
	public float coroutineTime;
	public float stoppedTime;

	public string playerName = "FAIL";
	private float screenDivisionUnitX;
	private float screenDivisionUnitY;
	//private int startZone1X;
	private int rand;
	private Vector2 zone2PreviousPos;
	private Vector2 zone2CurrentPos;
	private int screenDivY;
	private int posIni = 0;
	private int posFin = 0;

	Component playerComponent;

	//Defense part
	public bool isDefending = false;//for punishing if player attacks while defending
	public float defendingTime = 0.0f;// a timer for when player starts defending, after a while the absortion power of defense will lower, this value could be based upong player lvl or item quality??
	public float defenseStoppedTime = 0.0f;// so we check when player stop defending.

	public string theText = "TEXT";
	private float startTimer = 0;

	private LineRenderer lineRenderer;// we set a variable to hold the component, on the start we will set it up

	void Awake (){
		if (player == null) {
			player = GameObject.FindGameObjectWithTag ("Player");
		}
	}


	void Start () {
		playerComponent = player.GetComponent<Player>();
		playerName = player.GetComponent<Player>().characterName;
		screenDivisionUnitX = Screen.width / divisionScreenX;
		screenDivisionUnitY = Screen.height / divisionScreenY;
		screenDivY = Screen.height / yGameZones;

		//Lets call the player timer in here so we have an initial value and a coroutine running before we try to stop it later on
		StartCoroutine (PlayerLastActionTimer());
		//startZone1X = 0;
		//lineRenderer = GetComponent<LineRenderer> (); // we cache the lineRenderer component to the variable of type linerenderer we created
		//lineRenderer.SetWidth (.20f, .20f);
	}

	void OnGUI (){
		GUI.Box(new Rect(0,0,screenDivisionUnitX,Screen.height),"This is a box 1" );
		GUI.Box(new Rect(screenDivisionUnitX,screenDivisionUnitY,Screen.width - screenDivisionUnitX - screenDivisionUnitX,Screen.height - 2* screenDivisionUnitY),"This is a box 2" );
		GUI.Box(new Rect(Screen.width - screenDivisionUnitX,0,screenDivisionUnitX / 2,Screen.height),"This is a box 3" );
		GUI.Box(new Rect(Screen.width - screenDivisionUnitX/2,0,screenDivisionUnitX / 2,Screen.height),"This is a half box 4" );

		//lets add a Text
		GUI.TextArea(new Rect(10,10,200,100),theText);


		GUI.Box (new Rect (screenDivisionUnitX, 0, Screen.width - 2* screenDivisionUnitX, screenDivisionUnitY), playerName);
		// Y Divisions

		//GUI.Box (new Rect (0, 0, Screen.width, Screen.height / yGameZones), "Y1");
		//GUI.Box (new Rect (0, screenDivY, Screen.width, Screen.height / yGameZones), "Y2");
		//GUI.Box (new Rect (0, 2 * screenDivY, Screen.width, Screen.height / yGameZones), "Y3");
		//GUI.Box (new Rect (0, 3 * screenDivY, Screen.width, Screen.height / yGameZones), "Y4");
	}

	void Update () {
		theText = "The timer now is " + (int)GameTimer();



		List<Touch> touches = new List<Touch> ();
		List<Touch> touches2Zone = new List<Touch> ();
		for (int i = 0; i < Input.touches.Length; i++) {
			Touch touch = Input.touches [i];
			Touch[] myTouch = Input.touches;
			if (myTouch != null) {
				rand = Random.Range (0, 100);//This way we chose between a value between 0 and 99
				#region Zone 1

				///////            ZONE 1 // Shield Zone
				/// 
				if (Input.GetTouch (i).position.x < screenDivisionUnitX) {
					//Debug.Log ("Zone1");
					switch (touch.phase) {
					case TouchPhase.Began:
						isDefending = true; // we flagg it as defending
						StartCoroutine (DefenseCoroutine(5.0f));

						if(Input.GetTouch (i).position.y < screenDivY){
							Debug.Log("Defending foot");
						}else if (Input.GetTouch (i).position.y < 2 * screenDivY ){
							Debug.Log("Defending Lower Chest");
						}else if (Input.GetTouch (i).position.y < 3 *screenDivY){
							Debug.Log("Defending Upper Chest");
						}else{
							Debug.Log("Defending Head");
						}

						break;
					case TouchPhase.Moved:
						if(Input.GetTouch (i).position.y < screenDivY){
							Debug.Log("Defending foot");
						}else if (Input.GetTouch (i).position.y < 2 * screenDivY ){
							Debug.Log("Defending Lower Chest");
						}else if (Input.GetTouch (i).position.y < 3 *screenDivY){
							Debug.Log("Defending Upper Chest");
						}else{
							Debug.Log("Defending Head");
						}
						break;
					case TouchPhase.Ended:
						isDefending = false;// unflag the defense
						defenseStoppedTime = defendingTime; // we set the stopped time why i need i stopped time for defense im not sure yet, might  get rid of this later
						defendingTime = 0.0f;// and clear the clock.
						break;
					}
				
				#endregion
					#region Zone 2

					///////            ZONE 2 // Move and collect zone

				
				} else if (Input.GetTouch (i).position.x > screenDivisionUnitX && Input.GetTouch (i).position.x < Screen.width - screenDivisionUnitX 
					&& Input.GetTouch (i).position.y > screenDivisionUnitY && Input.GetTouch (i).position.y < Screen.height - screenDivisionUnitY) {
					//Debug.Log("Zone2");
					touches2Zone.Add (Input.GetTouch (i));
					for (int r = 0; r > touches2Zone.Count; r++) {

					}
					//Debug.Log(touches2Zone.Count);
					switch (touch.phase) {
					case TouchPhase.Began:

						//lets set the vector 2 positions zone2previousPos to the position of the finger (i)
						zone2PreviousPos = Input.GetTouch (i).position;
						Vector3 linePos = new Vector3 (zone2PreviousPos.x, zone2PreviousPos.y, 0);
						//lineRenderer.SetPosition (0, linePos);//WE dont have a lin e renderer component yet
						break;
					case TouchPhase.Moved:
						break;
					case TouchPhase.Ended:

						zone2CurrentPos = Input.GetTouch (i).position;
						Vector3 line2Pos = new Vector3 (zone2CurrentPos.x, zone2CurrentPos.y, 0);
						//lineRenderer.SetPosition (1, line2Pos);
						//lets call our function

						switch (Direction (zone2PreviousPos, zone2CurrentPos, swipeZone2Confort)) {
						case 1:
							Debug.Log ("Jump up");
							break;
						case 2:
							Debug.Log ("Jump forward");
							break;
						case 3:
							Debug.Log ("Move forward");
							break;
						case 4:
							Debug.Log ("Leam down and move");
							break;
						case 5:
							Debug.Log ("Kneed");
							break;
						case 6:
							Debug.Log ("Lean down move backwards");
							break;
						case 7:
							Debug.Log ("Move Backwards");
							break;
						case 8:
							Debug.Log ("Jump Backwards");
							break;
						default:
							Debug.Log ("raycast");
							break;
						}




							//Debug.Log("Swiped");


						//Debug.Log("The Random Number is:" + rand);
						break;
					}
				#endregion
					#region Zone3
				///////            ZONE 3 // Combat zone
				
				} else if (Input.GetTouch (i).position.x > Screen.width - screenDivisionUnitX) {
					//Debug.Log ("Zone3");
					//We check if the clock of the coroutine is running, this way we can disable to touch devices
					if(isRunningClock == false){
						//we free the last action timer
						isRunningLastActionTimer = false;
					switch (touch.phase) {
					case TouchPhase.Began:
							if(isRunningAttackCoroutine == true){
								isRunningAttackCoroutine = false;//This should end the coroutine
								Debug.Log("The time stopped at :" + stoppedTime);
							}
						//Lets create some if statements to get the position
						if(Input.GetTouch (i).position.x < Screen.width - screenDivisionUnitX/2 ){
							//It can be 1 , 2, 3 , 4
							//Debug.Log ("It can be 1 , 2, 3 , 4");
							if(Input.GetTouch (i).position.y > 0 && Input.GetTouch (i).position.y < screenDivY ){
								//Debug.Log("4");
								posIni = 4;
							}else if(Input.GetTouch (i).position.y > screenDivY && Input.GetTouch (i).position.y < 2 * screenDivY ){
								//Debug.Log("3");
								posIni = 3;
							}else if(Input.GetTouch (i).position.y > 2 * screenDivY && Input.GetTouch (i).position.y < 3 *screenDivY ){
								//Debug.Log("2");
								posIni = 2;
							}else{
								//Debug.Log("1");
								posIni = 1;
							}

						}else{
							//It can be 5 , 6, 7 , 8
							//Debug.Log ("It can be 5 , 6, 7 , 8");
							if(Input.GetTouch (i).position.y > 0 && Input.GetTouch (i).position.y < screenDivY ){
								//Debug.Log("8");
								posIni = 8;
							}else if(Input.GetTouch (i).position.y > screenDivY && Input.GetTouch (i).position.y < 2 * screenDivY ){
								//Debug.Log("7");
								posIni = 7;
							}else if(Input.GetTouch (i).position.y > 2 * screenDivY && Input.GetTouch (i).position.y < 3 *screenDivY ){
								//Debug.Log("6");
								posIni = 6;
							}else{
								//Debug.Log("5");
								posIni = 5;
							}
						}
						//Debug.Log ("Zone3");
						//Debug.Log ("the X position is " + Input.GetTouch (i).position.x + "the Y position is " + Input.GetTouch (i).position.y  );
						break;
					case TouchPhase.Moved:
						break;
					case TouchPhase.Ended:
						//will run the Clock Coroutine
							StartCoroutine (PlayerLastActionTimer());

							StartCoroutine (AttackTimer(5, 0.5f));//we might have to call it inside each zone se we can pass different timer deppending on where we attacked

						//Lets create some if statements to get the position
						if(Input.GetTouch (i).position.x < Screen.width - screenDivisionUnitX/2 ){
							//It can be 1 , 2, 3 , 4
							//Debug.Log ("It can be 1 , 2, 3 , 4");
							if(Input.GetTouch (i).position.y > 0 && Input.GetTouch (i).position.y < screenDivY ){
								//Debug.Log("4");
								posFin = 4;
							}else if(Input.GetTouch (i).position.y > screenDivY && Input.GetTouch (i).position.y < 2 * screenDivY ){
								//Debug.Log("3");
								posFin = 3;
							}else if(Input.GetTouch (i).position.y > 2 * screenDivY && Input.GetTouch (i).position.y < 3 *screenDivY ){
								//Debug.Log("2");
								posFin = 2;
							}else{
								//Debug.Log("1");
								posFin = 1;
							}
							
						}else{
							//It can be 5 , 6, 7 , 8
							//Debug.Log ("It can be 5 , 6, 7 , 8");
							if(Input.GetTouch (i).position.y > 0 && Input.GetTouch (i).position.y < screenDivY ){
								//Debug.Log("8");
								posFin = 8;
							}else if(Input.GetTouch (i).position.y > screenDivY && Input.GetTouch (i).position.y < 2 * screenDivY ){
								//Debug.Log("7");
								posFin = 7;
							}else if(Input.GetTouch (i).position.y > 2 * screenDivY && Input.GetTouch (i).position.y < 3 *screenDivY ){
								//Debug.Log("6");
								posFin = 6;
							}else{
								//Debug.Log("5");
								posFin = 5;
							}
						}
						//Check attack 
						////// 1
						if(posIni == 1 && posFin == 1){
								player.GetComponent<Player>().Attack(1);
							//We call the player Attack 1
							//Debug.Log("Left Head Stab");
						}
						else if(posIni == 1 && posFin == 2){
								player.GetComponent<Player>().Attack(2);
							//We call the player Attack 2
								//Debug.Log("Left Head to Up Torso Slash");
						}
						else if(posIni == 1 && posFin == 3){
								player.GetComponent<Player>().Attack(3);
							//We call the player Attack 3
								//Debug.Log("Left Head to Lower Torso Slash");
						}
						else if(posIni == 1 && posFin == 4){
								player.GetComponent<Player>().Attack(4);
							//We call the player Attack 4
								//Debug.Log("Left Head to Legs Long Slash");
						}
						else if(posIni == 1 && posFin == 5){
								player.GetComponent<Player>().Attack(5);
							//We call the player Attack 5
								//Debug.Log("Left Head to Right Head Slah");
						}
						else if(posIni == 1 && posFin == 6){
								player.GetComponent<Player>().Attack(6);
							//We call the player Attack 6
								//Debug.Log("Left Head to Right Up Torso Slash");
						}
						else if(posIni == 1 && posFin == 7){
								player.GetComponent<Player>().Attack(7);
							//We call the player Attack 7
								//Debug.Log("Left Head to Right Lower Torso Slash");
						}else if(posIni == 1 && posFin == 8){
								player.GetComponent<Player>().Attack(8);
							//We call the player Attack 8
								//Debug.Log("Left Head to Right Legs Long Slash");
						}//////// 1 done

						else if(posIni == 2 && posFin == 1){
								player.GetComponent<Player>().Attack(9);
							//We call the player Attack 1
								//Debug.Log("Left Up Torso to Left Head Slash");
						}
						else if(posIni == 2 && posFin == 2){
								player.GetComponent<Player>().Attack(10);
							//We call the player Attack 2
								//Debug.Log("Left Up Torso Stab");
						}
						else if(posIni == 2 && posFin == 3){
								player.GetComponent<Player>().Attack(11);
							//We call the player Attack 3
								//Debug.Log("Left Up Torso to Lower Torso Slash");
						}
						else if(posIni == 2 && posFin == 4){
								player.GetComponent<Player>().Attack(12);
							//We call the player Attack 4
								//Debug.Log("Left Up Torso to Legs Slash");
						}
						else if(posIni == 2 && posFin == 5){
								player.GetComponent<Player>().Attack(13);
							//We call the player Attack 5
								//Debug.Log("Left Up Torso to Right Head Slah");
						}
						else if(posIni == 2 && posFin == 6){
								player.GetComponent<Player>().Attack(14);
							//We call the player Attack 6
								//Debug.Log("Left Up Torso to Right Up Torso Slash");
						}
						else if(posIni == 2 && posFin == 7){
								player.GetComponent<Player>().Attack(15);
							//We call the player Attack 7
								//Debug.Log("Left Up Torso to Right Lower Torso Slash");
						}else if(posIni == 2 && posFin == 8){
								player.GetComponent<Player>().Attack(16);
							//We call the player Attack 8
								//Debug.Log("Left Up Torso to Right Legs Slash");
						}//////// 2 done

						else if(posIni == 3 && posFin == 1){
								player.GetComponent<Player>().Attack(17);
							//We call the player Attack 1
								//Debug.Log("Left Lower Torso to Left Head Slash");
						}
						else if(posIni == 3 && posFin == 2){
								player.GetComponent<Player>().Attack(18);
							//We call the player Attack 2
								//Debug.Log("Left Lower Torso to Left Up Torso Slash");
						}
						else if(posIni == 3 && posFin == 3){
								player.GetComponent<Player>().Attack(19);
							//We call the player Attack 3
								//Debug.Log("Lower Torso Stab");
						}
						else if(posIni == 3 && posFin == 4){
								player.GetComponent<Player>().Attack(20);
							//We call the player Attack 4
								//Debug.Log("Left Lower Torso to Legs Slash");
						}
						else if(posIni == 3 && posFin == 5){
								player.GetComponent<Player>().Attack(21);
							//We call the player Attack 5
								//Debug.Log("Left Lower Torso to Right Head Slah");
						}
						else if(posIni == 3 && posFin == 6){
								player.GetComponent<Player>().Attack(22);
							//We call the player Attack 6
								//Debug.Log("Left Lower Torso to Right Up Torso Slash");
						}
						else if(posIni == 3 && posFin == 7){
								player.GetComponent<Player>().Attack(23);
							//We call the player Attack 7
								//Debug.Log("Left Lower Torso to Right Lower Torso Slash");
						}else if(posIni == 3 && posFin == 8){
								player.GetComponent<Player>().Attack(24);
							//We call the player Attack 8
								//Debug.Log("Left Lower Torso to Right Legs Slash");
						}//////// 3 done

						else if(posIni == 4 && posFin == 1){
								player.GetComponent<Player>().Attack(25);
							//We call the player Attack 1
								//Debug.Log("Left Lower Legs to Left Head Long Slash");
						}
						else if(posIni == 4 && posFin == 2){
								player.GetComponent<Player>().Attack(26);
							//We call the player Attack 2
								//Debug.Log("Left Lower Legs to Left Up Torso Slash");
						}
						else if(posIni == 4 && posFin == 3){
								player.GetComponent<Player>().Attack(27);
							//We call the player Attack 3
								//Debug.Log("Left Lower Legs to Lower Torso Slash");
						}
						else if(posIni == 4 && posFin == 4){
								player.GetComponent<Player>().Attack(28);
							//We call the player Attack 4
								//Debug.Log("Left Lower Legs Stab");
						}
						else if(posIni == 4 && posFin == 5){
								player.GetComponent<Player>().Attack(29);
							//We call the player Attack 5
								//Debug.Log("Left Lower Legs to Right Head Long Slah");
						}
						else if(posIni == 4 && posFin == 6){
								player.GetComponent<Player>().Attack(30);
							//We call the player Attack 6
								//Debug.Log("Left Lower Legs to Right Up Torso Slash");
						}
						else if(posIni == 4 && posFin == 7){
								player.GetComponent<Player>().Attack(31);
							//We call the player Attack 7
								//Debug.Log("Left Lower Legs to Right Lower Torso Slash");
						}else if(posIni == 4 && posFin == 8){
								player.GetComponent<Player>().Attack(32);
							//We call the player Attack 8
								//Debug.Log("Left Lower Legs to Right Legs Slash");
						}//////// 4 done

						else if(posIni == 5 && posFin == 1){
								player.GetComponent<Player>().Attack(33);
							//We call the player Attack 1
								//Debug.Log("Right Head Stab");
						}
						else if(posIni == 5 && posFin == 2){
								player.GetComponent<Player>().Attack(34);
							//We call the player Attack 2
								//Debug.Log("Right Head to Up Torso Slash");
						}
						else if(posIni == 5 && posFin == 3){
								player.GetComponent<Player>().Attack(35);
							//We call the player Attack 3
								//Debug.Log("Right Head to Lower Torso Slash");
						}
						else if(posIni == 5 && posFin == 4){
								player.GetComponent<Player>().Attack(36);
							//We call the player Attack 4
								//Debug.Log("Right Head to Legs Long Slash");
						}
						else if(posIni == 5 && posFin == 5){
								player.GetComponent<Player>().Attack(37);
							//We call the player Attack 5
								//Debug.Log("Right Head to Right Head Slah");
						}
						else if(posIni == 5 && posFin == 6){
								player.GetComponent<Player>().Attack(38);
							//We call the player Attack 6
								//Debug.Log("Right Head to Right Up Torso Slash");
						}
						else if(posIni == 5 && posFin == 7){
								player.GetComponent<Player>().Attack(39);
							//We call the player Attack 7
								//Debug.Log("Right Head to Right Lower Torso Slash");
						}else if(posIni == 5 && posFin == 8){
								player.GetComponent<Player>().Attack(40);
							//We call the player Attack 8
								//Debug.Log("Right Head to Right Legs Long Slash");
						}//////// 5 done

						else if(posIni == 6 && posFin == 1){
								player.GetComponent<Player>().Attack(41);
							//We call the player Attack 1
								//Debug.Log("Right Up Torso to Left Head Slash");
						}
						else if(posIni == 6 && posFin == 2){
								player.GetComponent<Player>().Attack(42);
							//We call the player Attack 2
								//Debug.Log("Right Up Torso to Left Up Torso Slash");
						}
						else if(posIni == 6 && posFin == 3){
								player.GetComponent<Player>().Attack(43);
							//We call the player Attack 3
								//Debug.Log("Right Up Torso to Lower Torso Slash");
						}
						else if(posIni == 6 && posFin == 4){
								player.GetComponent<Player>().Attack(44);
							//We call the player Attack 4
								//Debug.Log("Right Up Torso to Legs Slash");
						}
						else if(posIni == 6 && posFin == 5){
								player.GetComponent<Player>().Attack(45);
							//We call the player Attack 5
								//Debug.Log("Right Up Torso to Right Head Slah");
						}
						else if(posIni == 6 && posFin == 6){
								player.GetComponent<Player>().Attack(46);
							//We call the player Attack 6
								//Debug.Log("Right Up Torso Stab");
						}
						else if(posIni == 6 && posFin == 7){
								player.GetComponent<Player>().Attack(47);
							//We call the player Attack 7
								//Debug.Log("Right Up Torso to Right Lower Torso Slash");
						}else if(posIni == 6 && posFin == 8){
								player.GetComponent<Player>().Attack(48);
							//We call the player Attack 8
								//Debug.Log("Right Up Torso to Right Legs Slash");
						}//////// 6 done
						
						else if(posIni == 7 && posFin == 1){
								player.GetComponent<Player>().Attack(49);
							//We call the player Attack 1
								//Debug.Log("Right Lower Torso to Left Head Slash");
						}
						else if(posIni == 7 && posFin == 2){
								player.GetComponent<Player>().Attack(50);
							//We call the player Attack 2
								//Debug.Log("Right Lower Torso to Left Up Torso Slash");
						}
						else if(posIni == 7 && posFin == 3){
								player.GetComponent<Player>().Attack(51);
							//We call the player Attack 3
								//Debug.Log("Right Lower Torso to Lower Torso Slash");
						}
						else if(posIni == 7 && posFin == 4){
								player.GetComponent<Player>().Attack(52);
							//We call the player Attack 4
								//Debug.Log("Right Lower Torso to Legs Slash");
						}
						else if(posIni == 7 && posFin == 5){
								player.GetComponent<Player>().Attack(53);
							//We call the player Attack 5
								//Debug.Log("Right Lower Torso to Right Head Slah");
						}
						else if(posIni == 7 && posFin == 6){
								player.GetComponent<Player>().Attack(54);
							//We call the player Attack 6
								//Debug.Log("Right Lower Torso to Right Up Torso Slash");
						}
						else if(posIni == 7 && posFin == 7){
								player.GetComponent<Player>().Attack(55);
							//We call the player Attack 7
								//Debug.Log("Right Lower Torso Stab");
						}else if(posIni == 7 && posFin == 8){
								player.GetComponent<Player>().Attack(56);
							//We call the player Attack 8
								//Debug.Log("Right Lower Torso to Right Legs Slash");
						}//////// 7 done
												
						else if(posIni == 8 && posFin == 1){
								player.GetComponent<Player>().Attack(57);
							//We call the player Attack 1
								//Debug.Log("Right Lower Legs to Left Head Long Slash");
						}
						else if(posIni == 8 && posFin == 2){
								player.GetComponent<Player>().Attack(58);
							//We call the player Attack 2
								//Debug.Log("Right Lower Legs to Left Up Torso Slash");
						}
						else if(posIni == 8 && posFin == 3){
								player.GetComponent<Player>().Attack(59);
							//We call the player Attack 3
								//Debug.Log("Right Lower Legs to Lower Torso Slash");
						}
						else if(posIni == 8 && posFin == 4){
								player.GetComponent<Player>().Attack(60);
							//We call the player Attack 4
							//Debug.Log("Right Lower Legs to Left Lower Legs Slash");
						}
						else if(posIni == 8 && posFin == 5){
								player.GetComponent<Player>().Attack(61);
							//We call the player Attack 5
								//Debug.Log("Right Lower Legs to Right Head Long Slah");
						}
						else if(posIni == 8 && posFin == 6){
								player.GetComponent<Player>().Attack(62);
							//We call the player Attack 6
								//Debug.Log("Right Lower Legs to Right Up Torso Slash");
						}
						else if(posIni == 8 && posFin == 7){
								player.GetComponent<Player>().Attack(63);
							//We call the player Attack 7
								//Debug.Log("Right Lower Legs to Right Lower Torso Slash");
						}else if(posIni == 8 && posFin == 8){
								player.GetComponent<Player>().Attack(64);
							//We call the player Attack 8
								//Debug.Log("Right Lower Legs Stab");
						}


						//Debug.Log("The pos ini is " + posIni + " The pos fini is " + posFin);
						break;
						}
					}
				#endregion
					#region Zone4
				///////            ZONE 4 // Quick items
				
				
				} else if(Input.GetTouch (i).position.x > screenDivisionUnitX && Input.GetTouch (i).position.x < Screen.width - screenDivisionUnitX 
				          && Input.GetTouch (i).position.y < screenDivisionUnitY){
					Debug.Log ("Zone4");
					switch (touch.phase) {
					case TouchPhase.Began:
					
						break;
					case TouchPhase.Moved:
						break;
					case TouchPhase.Ended:
						break;
					}
				}
				#endregion
					#region Zone5
					///////            ZONE 5 // player name
					else {
						Debug.Log ("Zone5");
						switch (touch.phase) {
						case TouchPhase.Began:
							
							break;
						case TouchPhase.Moved:
							break;
						case TouchPhase.Ended:
							break;
						}
				}
			}
		}
	}
	#endregion
	#region Other Classes Definition

//lets try to call a class, in this class i will define the direction of the finger swipe.
	int Direction (Vector2 previousPos, Vector2 currentPos, int swipeConfort){
		//lets calculate the diference in magnitude of the vector from the start of position and end of position
		int ret = 9;//will set the variable according with the direction the finger swipe, we will use this value in a case switcg to decide what to do.
		float xDelta;
		float yDelta;
		float deltaPos;
		bool xMove = false;
		bool yMove = false;
		bool xNegMove = false;
		bool yNegMove = false;

		xDelta = previousPos.x - currentPos.x;
		yDelta = previousPos.y - currentPos.y;
		if (Mathf.Abs (xDelta) > swipeConfort) {
			xMove = true;
			//Debug.Log("Swiped in x");
			if (xDelta < 0 ){
				//Debug.Log(" x positive");
			}else {
				xNegMove = true;
				//Debug.Log("x negative");
			}
		}
		if (Mathf.Abs (yDelta) > swipeConfort) {
			yMove = true;
			//Debug.Log("Swiped in y");
			if (yDelta < 0 ){
				//Debug.Log(" y positive");
			}else {
				yNegMove = true;
				//Debug.Log("y negative");
			}
		}
		if (yMove == true && yNegMove == false && xMove == false) {
			Debug.Log("Up");
			ret = 1;
		}
		if (yMove == true && yNegMove == false && xMove == true && xNegMove == false) {
			Debug.Log("Up Right");
			ret = 2;
		}

		if (yMove == false && xMove == true && xNegMove == false) {
			Debug.Log("Right");
			ret = 3;
		}
		if (yMove == true && yNegMove == true && xMove == true && xNegMove == false) {
			Debug.Log("Right Down");
			ret = 6;
		}
		if (yMove == true && yNegMove == true && xMove == false) {
			Debug.Log("down");
			ret = 5;
		}
		if (yMove == true && yNegMove == true && xMove == true && xNegMove == true) {
			Debug.Log("Down Left");
			ret = 6;
		}
		if (yMove == false && xMove == true && xNegMove == true) {
			Debug.Log("Left");
			ret = 7;
		}
		if (yMove == true && yNegMove == false && xMove == true && xNegMove == true) {
			Debug.Log("Left Up");
			ret = 8;
		}

	
		//Now we compare the magnitudes to find out the direction of swipe
		/*if(zone2DeltaPos < 0){
			//Debug.Log("Left and Bottom");
			if (Mathf.Abs(zone2CurrentPos.x - zone2PreviousPos.x ) > Mathf.Abs(zone2CurrentPos.y - zone2PreviousPos.y )&& (Mathf.Abs(zone2CurrentPos.y - zone2PreviousPos.y )) > diagonalSafeZone){
				
				if(zone2CurrentPos.y - zone2PreviousPos.y > 0 ){
					Debug.Log("Diagonal LEFT UP");
				}else {
					Debug.Log("Diagonal LEFT DOWN");
				}
			} else if(Mathf.Abs(zone2CurrentPos.x - zone2PreviousPos.x ) > Mathf.Abs(zone2CurrentPos.y - zone2PreviousPos.y )){
				Debug.Log("Left");
			}else{
				Debug.Log("Bottom");
			}
		}else{
			//Debug.Log("Right and Up");
			if (Mathf.Abs(zone2CurrentPos.x - zone2PreviousPos.x ) < Mathf.Abs(zone2CurrentPos.y - zone2PreviousPos.y )&& (Mathf.Abs(zone2CurrentPos.y - zone2PreviousPos.y )) > diagonalSafeZone){
				
				if(zone2CurrentPos.y - zone2PreviousPos.y > 0 ){
					Debug.Log("Diagonal RIGHT UP");
				}else {
					Debug.Log("Diagonal RIGHT DOWN");
				}
			} else if(Mathf.Abs(zone2CurrentPos.x - zone2PreviousPos.x ) < Mathf.Abs(zone2CurrentPos.y - zone2PreviousPos.y )){
				Debug.Log("RIGHT");
			}else{
				Debug.Log("UP");
			}
			*/


		return ret;
	}
	//A clock code, it calculates the time of the game is running, just keeps increasing, might be usefull in the future
	float GameTimer(){
		startTimer += Time.deltaTime;
		return startTimer;
	}
	//a coroutine timer, we call it passing the original value and the final value, maybe we can pass more specific times depending on what we want later on, example best time for attack, too fast and so on...
	IEnumerator AttackTimer(float endTimer, float freeTimer){// we pass the end timer and the free timer
		isRunningAttackCoroutine = true;
		isRunningClock = true;
		while(isRunningAttackCoroutine == true){
			coroutineTime += 0.1f;//we add time to it
			yield return new WaitForSeconds(0.1f);

			if(coroutineTime > freeTimer){// when the timer pass the free timer mark we liberate for acction
					isRunningClock = false;
				}
			if(coroutineTime > endTimer){ // we finish the loop when pass the end timer
				isRunningAttackCoroutine = false;
				}
			}
		stoppedTime = coroutineTime;// we set the stopped time to be the same as the coroutine timer
		coroutineTime = 0.0f;// we reset the timer
		Debug.Log ("Finish Attack Coroutine");
			yield return null;

		}
		
	IEnumerator DefenseCoroutine(float halfDefense){

		bool hasChecked = false;
		while(isDefending == true){
			defendingTime += 0.1f;
			if(defendingTime < halfDefense && hasChecked == false){
				Debug.Log("Half Defense");
				hasChecked = true;
			}
			yield return new WaitForSeconds (0.1f);
			//yield return null;
		}
		Debug.Log("Stop Defense");
		//yield return new WaitForSeconds (0.1f);
		yield return null;
	}

	IEnumerator PlayerLastActionTimer(){
		isRunningLastActionTimer = true;
		while(isRunningLastActionTimer == true){
			lastActionTimer += 0.1f;
			yield return new WaitForSeconds(0.1f);
		}
		stoppedLastActionTimer = lastActionTimer;
		lastActionTimer = 0.0f;
		yield return null;
	}

	IEnumerator PlayerTimer(){
		playerActionTimer += 0.1f;
		Debug.Log("Coroutine Timer Running");
		yield return new WaitForSeconds (0.1f);
	}

	#endregion
}
