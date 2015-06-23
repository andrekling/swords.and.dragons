using UnityEngine;
using System.Collections;

public class blink : MonoBehaviour {

	public int randomNum;
	public bool blinking = true;
	Animator anim;
	int blinkHash = Animator.StringToHash("blink");
	public int randMax = 20;
	public int blinkNum = 18;
	public float waitTimer = 0.3f;
	// Use this for initialization
	void Start () {
		anim = GetComponentInChildren<Animator> ();
		StartCoroutine (Blink ());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator Blink()	{
		while (blinking == true) {
			randomNum = Random.Range (0, randMax);
			if(randomNum >blinkNum){
				anim.SetTrigger(blinkHash);
			}
			yield return new WaitForSeconds(waitTimer); 
		}
	}
}
