/*
 * No longer being used
 * 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour {

	private GameController gameController;

	// Use this for initialization
	void Start () {
		gameController = GameController.instance;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other){
		if(other.gameObject.CompareTag("Player")){
			gameController.playerDied ();
		}
	}
}
*/