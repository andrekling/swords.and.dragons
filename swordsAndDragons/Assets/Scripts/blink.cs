using UnityEngine;
using System.Collections;

public class blink : MonoBehaviour {

	public int randomNum;
	public bool blinking = true;
	Animator anim;
	int blinkHash = Animator.StringToHash("blink");
	// Use this for initialization
	void Start () {
		StartCoroutine (Blink ());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator Blink()	{
		while (blinking == true) {
			randomNum = Random.Range (0, 21);
			if(randomNum > 18){
				anim.SetTrigger(blinkHash);
			}
			yield return new WaitForSeconds(0.3f); 
		}
	}
}
