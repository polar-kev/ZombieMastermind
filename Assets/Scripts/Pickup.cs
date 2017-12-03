using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {

	private Transform inHand;

	private float pickupDistance = 2f;
	private Rigidbody rgbd;
	private static bool isInHand;

	// Use this for initialization
	void Start () {
		rgbd = gameObject.GetComponent<Rigidbody> ();
		inHand = GameObject.Find ("InHand").transform;
		isInHand = false;
	}
	
	// Update is called once per frame
	void Update () {
		//print("Distance: "+ Vector3.Distance(rgbd.transform.position,GameObject.FindGameObjectWithTag("Player").transform.position));
	}

	void OnMouseDown(){
		
		//Make sure player is close enough to object to be able to pick it up
		if(!isInHand && Vector3.Distance(gameObject.transform.position,GameObject.FindGameObjectWithTag("Player").transform.position) < pickupDistance){
			rgbd.useGravity = false;
			rgbd.isKinematic = true;
			gameObject.transform.position = inHand.position;
			gameObject.transform.parent = GameObject.Find ("FirstPersonCharacter").transform;
			isInHand = true;
		}
	}

	void OnMouseUp(){
		rgbd.useGravity = true;
		rgbd.isKinematic = false;
		gameObject.transform.parent = null;
		isInHand = false;
	}

}//end of class
