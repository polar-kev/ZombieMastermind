using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum blockColour{red,blue,yellow,noColour};

public class DropZone : MonoBehaviour {
	public static int dropZoneID = 0;

	public int dropzone;
	public float dropzoneHeight = -1.9f;
	public AudioClip dropzoneEnterSound;

	private bool triggered;
	private blockColour blockType;
	private float dropzoneLength = 3f;
	private float scoringZoneLength;
	private float spacing = 3f;
	private float numberOfDropzones;
	private GameObject scoringPlatform;
	private AudioSource audioSource;


	// Use this for initialization
	void Start () {
		triggered = false;
		//findDropzoneNumber ();
		dropzone = dropZoneID;

		audioSource = gameObject.GetComponent<AudioSource> ();

		///////////////////////////////////////////////////
		//Position dropzone platforms on scoring platform//
		///////////////////////////////////////////////////
		scoringPlatform = GameObject.Find ("ScoringPlatform").gameObject;
		//gameObject.transform.parent = scoringPlatform.transform;
		numberOfDropzones = GameController.difficulty;

		//Calculate how much space is need to place all dropzones with appropriate spacing
		scoringZoneLength = (numberOfDropzones - 1) * (dropzoneLength + spacing);

		//Calculate the x and z values of the dropzones with respect to the center of the scoring platform
		float x = (scoringPlatform.GetComponent<BoxCollider> ().center.x - scoringZoneLength / 2f) + (dropzoneLength + spacing) * dropZoneID;
		float y = dropzoneHeight;
		float z = scoringPlatform.transform.position.z;//dropzoneLength / 2f + scoringPlatform.GetComponent<BoxCollider> ().center.z;
		gameObject.transform.position = new Vector3(x,y,z);

		//gameObject.transform.position = new Vector3(6f * dropzone,gameObject.transform.position.y,gameObject.transform.position.z);
		print ("DropZone: " + dropzone);

		dropZoneID++;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerStay(Collider other){
		if(!triggered && other.gameObject.CompareTag("RedBlock")){
			print ("Red Block Entered Platform");
			blockType = blockColour.red;
			triggered = true;
			GameController.instance.DropZoneActivated (dropzone, blockType);
			PlayDropzoneEnterSound ();
			audioSource.clip = dropzoneEnterSound;
			audioSource.Play ();

		}
		if(!triggered && other.gameObject.CompareTag("BlueBlock")){
			print ("Blue Block Entered Platform");
			blockType = blockColour.blue;
			triggered = true;
			GameController.instance.DropZoneActivated (dropzone, blockType);
			PlayDropzoneEnterSound ();
		}
		if(!triggered && other.gameObject.CompareTag("YellowBlock")){
			print ("Yellow Block Entered Platform");
			blockType = blockColour.yellow;
			triggered = true;
			GameController.instance.DropZoneActivated (dropzone, blockType);
			PlayDropzoneEnterSound ();
		}
	}

	void OnTriggerExit(Collider other){
		if(triggered && other.gameObject.CompareTag("RedBlock")){
			print ("Red Block left the platform");
			triggered = false;
			GameController.instance.DropZoneDeactivated (dropzone); 
		}
		if(triggered && other.gameObject.CompareTag("BlueBlock")){
			print ("Blue Block left the platform");
			triggered = false;
			GameController.instance.DropZoneDeactivated (dropzone); 
		}
		if(triggered && other.gameObject.CompareTag("YellowBlock")){
			print ("Yellow Block left the platform");
			triggered = false;
			GameController.instance.DropZoneDeactivated (dropzone); 
		}

	}

	public static void resetDropZoneID(){
		dropZoneID = 0;
		print ("DropZone ID Reset to: " + dropZoneID);
	}

	void PlayDropzoneEnterSound(){
		audioSource.clip = dropzoneEnterSound;
		audioSource.Play ();
	}
}//end of class
