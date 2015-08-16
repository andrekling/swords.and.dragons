using UnityEngine;
using System.Collections;

public class vushCaller : MonoBehaviour {

	public GameObject[] voosh;

	public void vushCall(int id){
		voosh [id].GetComponent<vush> ()._vush.enabled = true;
	}
}
