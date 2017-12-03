using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum blockColour{red,blue,yellow,noColour};

public class DropZone : MonoBehaviour {
	public static int dropZoneID = 0;

	public int dropzone;

	private bool triggered;
	private blockColour blockType;
	private float dropzoneLength = 3f;
	private float scoringZoneLength;
	private float spacing = 3f;
	private float numberOfDropzones;
	private GameObject scoringPlatform;


	// Use this for initialization
	void Start () {
		triggered = false;
		//findDropzoneNumber ();
		dropzone = dropZoneID;


		///////////////////////////////////////////////////
		//Position dropzone platforms on scoring platform//
		///////////////////////////////////////////////////
		scoringPlatform = GameObject.Find ("ScoringPlatform").gameObject;
		gameObject.transform.parent = scoringPlatform.transform;
		numberOfDropzones = GameController.difficulty;

		//Calculate how much space is need to place all dropzones with appropriate spacing
		scoringZoneLength = (numberOfDropzones - 1) * (dropzoneLength + spacing);

		//Calculate the x and z values of the dropzones with respect to the center of the scoring platform
		float x = (scoringPlatform.GetComponent<BoxCollider> ().center.x - scoringZoneLength / 2f) + (dropzoneLength + spacing) * dropZoneID;
		float z = dropzoneLength / 2f + scoringPlatform.GetComponent<BoxCollider> ().center.z;
		gameObject.transform.position = new Vector3(x,gameObject.transform.position.y,z);

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
		}
		if(!triggered && other.gameObject.CompareTag("BlueBlock")){
			print ("Blue Block Entered Platform");
			blockType = blockColour.blue;
			triggered = true;
			GameController.instance.DropZoneActivated (dropzone, blockType);
		}
		if(!triggered && other.gameObject.CompareTag("YellowBlock")){
			print ("Yellow Block Entered Platform");
			blockType = blockColour.yellow;
			triggered = true;
			GameController.instance.DropZoneActivated (dropzone, blockType);
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

	/*void findDropzoneNumber(){
		if(gameObject.CompareTag("DropZone1")){
			dropzone = 0;
		}
		else if(gameObject.CompareTag("DropZone2")){
			dropzone = 1;
		}
		else if(gameObject.CompareTag("DropZone3")){
			dropzone = 2;
		}
		else if(gameObject.CompareTag("DropZone4")){
			dropzone = 3;
		} else{
			dropzone = -1;
		}
		print ("DropZone: " + dropzone);
	}*/

	public static void resetDropZoneID(){
		dropZoneID = 0;
		print ("DropZone ID Reset to: " + dropZoneID);
	}
}//end of class
