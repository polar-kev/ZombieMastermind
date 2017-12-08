using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOverTime : MonoBehaviour {

	public float delay = 2f;

	private float elapsedTime;
	// Use this for initialization
	void Start () {
		elapsedTime = 0;
	}
	
	// Update is called once per frame
	void Update () {
		elapsedTime += Time.deltaTime;
		if(elapsedTime >= delay){
			Destroy (gameObject);
		}
	}
}
