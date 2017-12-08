using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helicopter : MonoBehaviour {

	public float speed = 50f;
	public float rotationSpeed = 0.5f;
	public Transform target;
	public Transform finalDestination;
	public float delay = 5f;

	private float elapsedTime;
	private Rigidbody rgbd;
	private GameController gameController;
	private PlayerController playerController;
	private Camera heliCam;

	// Use this for initialization
	void Start () {
		rgbd = gameObject.GetComponent<Rigidbody> ();
		gameController = GameController.instance;
		elapsedTime = Time.deltaTime;
		heliCam = gameObject.GetComponentInChildren<Camera> ();
		heliCam.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		elapsedTime+=Time.deltaTime;
		if(elapsedTime >= delay){
			//Move helicopter and slow down as helicopter moves towards target zone
			gameObject.transform.position = Vector3.MoveTowards(transform.position, target.position, Mathf.Lerp(speed,speed/4f,5f)*Time.deltaTime);

			//Rotate helicopter in direction of travel
			if (target.position - rgbd.position != Vector3.zero) {
				rgbd.transform.rotation = Quaternion.Slerp (
					rgbd.transform.rotation,

					Quaternion.LookRotation (target.position -
						new Vector3 (rgbd.position.x,
							target.position.y,
							rgbd.position.z)),
					rotationSpeed //* Time.deltaTime
				);
			} 
		}
			
	}//End of Update

	void OnTriggerEnter(Collider other){
		if(other.gameObject.CompareTag("Player")){
			gameController.GameIsWon ();
			target = finalDestination;

			speed = 20f;

			//Turn off Player Camera and Player Gravity
			other.gameObject.GetComponentInChildren<Camera> ().enabled = false;

			/*
			other.gameObject.GetComponent<Rigidbody> ().useGravity = false;

			//Set Parent to helicopter
			other.gameObject.transform.SetParent (gameObject.transform);
			other.gameObject.transform.position = Vector3.zero;

			//Hide player
			other.gameObject.GetComponentInChildren<MeshRenderer> ().enabled = false;
			*/

			Destroy (other.gameObject);
			//Set Main Camera to Heli Cam
			heliCam.enabled = true;
		}
	}

}
