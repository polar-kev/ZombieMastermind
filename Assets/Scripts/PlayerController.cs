using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	public GameObject flare;
	public Transform projectileSpawn;
	public static PlayerController instance;
	public bool flareReady;
	public float flareCooldown = 10f;
	public int flareCount = 3;
	public Radio radio;

	public Helicopter helicopter;
	public AudioClip heliCallMessage;
	public AudioClip heliResponseMessage;
	public AudioClip scream;

	public Text inventoryText;

	private float elapsedTime;
	private AudioSource audioSource2;
	public bool hasBattery;
	public bool hasRadio;
	public bool isAlive;
	private GameController gameController;
	private float waterHeight = -2.5f;

	void OnAwake(){
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
	}

	// Use this for initialization
	void Start () {
		gameController = GameController.instance;
		AudioSource[] audioList = gameObject.GetComponents<AudioSource> ();
		//audioSource1 managed my FPSController
		audioSource2 = audioList [1];
		flareReady = true;
		elapsedTime = 0;
		hasBattery = false;
		hasRadio = false;
		isAlive = true;
		inventoryText.text = "Flares -"+ flareCount+"-";
	}
	
	// Update is called once per frame
	void Update () {
		elapsedTime += Time.deltaTime;
		if(!flareReady && elapsedTime > flareCooldown){
			flareReady = true;
		}
		if (Input.GetKeyDown(KeyCode.F) && flareCount > 0 && flareReady)
		{
			dropFlare();
		}
		if (Input.GetKeyDown(KeyCode.R) && hasBattery && hasRadio)
		{
			CallHelicopter();
		}

		if(gameObject.transform.position.y <= waterHeight && isAlive){
			isAlive = false;
			DeathSequence (0.01f);
		}
		UpdateUI ();
	}

	void dropFlare()
	{
		flareReady = false;
		flareCount--;

		//Reset cooldown timer
		elapsedTime = 0;

		// Create the dlare from the dlare Prefab
		GameObject projectile = (GameObject)Instantiate (
			flare,
			projectileSpawn.position,
			Quaternion.identity);

		// Add velocity to the flare
		projectile.GetComponent<Rigidbody>().velocity = gameObject.GetComponent<Rigidbody>().velocity + projectile.transform.forward * 5f;

		// Destroy the flare after 30 seconds
		Destroy(projectile, 30f);
	}

	void CallHelicopter(){
		StartCoroutine("HelicopterRadioMessages");
	}


	IEnumerator HelicopterRadioMessages(){
		float heliMessageDelay = 7f;
		//Play heli call message
		audioSource2.clip = heliCallMessage;
		audioSource2.Play ();

		//Wait for audio to play
		yield return new WaitForSeconds (heliMessageDelay);

		//Play heli reply
		audioSource2.clip = heliResponseMessage;
		audioSource2.Play ();

		//Activate helicopter object in scene
		helicopter.gameObject.SetActive (true);
	}


	void OnTriggerEnter(Collider other){
		if(other.gameObject.CompareTag("Zombie") && isAlive){
			isAlive = false;
			DeathSequence (2f);
			print ("hit");
		}

		if(other.CompareTag("Battery")){
			//Instantiate (pickUpEffect, gameObject.transform);
			hasBattery = true;
		}

		if(other.CompareTag("Radio")){
			//Instantiate (pickUpEffect, gameObject.transform);
			hasRadio = true;
		}
	}

	void DeathSequence(float speed){
		audioSource2.clip = scream;
		audioSource2.Play ();
		gameController.playerDied (speed);
	}

	void UpdateUI(){
		inventoryText.text = "(F)lares -"+ flareCount+"-";
		if(hasBattery){
			inventoryText.text += "\nBattery";
		}
		if(hasRadio){
			inventoryText.text += "\n(R)adio";
		}
	}
	/*
	 * No longer being used
	 * 
	void DestroyRadio(){
		Destroy (gameObject);
	}
	*/
}
