
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : MonoBehaviour {

	public Helicopter helicopter;
	public AudioClip heliCallMessage;
	public AudioClip pickUp;
	//public GameObject pickUpEffect;

	public Transform spawn;

	//private GameController gameController;
	private AudioSource audioSource;

	// Use this for initialization
	void Start () {
		audioSource = gameObject.GetComponent<AudioSource> ();
		spawn = gameObject.transform;
	}

	void Reset(){
		gameObject.transform.position = spawn.position;
	}

	void OnTriggerEnter(Collider other){
		if(other.CompareTag("Player")){
			audioSource.clip = pickUp;
			audioSource.Play ();
			gameObject.transform.position = new Vector3 (-100f, -100f, -100f);
		}
	}
}