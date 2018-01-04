using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
	//public enum blockColour{red,blue,yellow,noColour};
	public static int difficulty = 3;
	public blockColour[] mysterySequence; 
	public blockColour[] tempArray; 
	public GameObject redBlock;
	public GameObject blueBlock;
	public GameObject yellowBlock;
	public GameObject dropZone;
	public Transform radioSpawn;
	public Transform batterySpawn;
	public Transform playerSpawn;
	public Radio radio;
	public Battery battery;

	public static GameController instance;
	public bool isGameWon;
	public blockColour[] guessedSequence;

	//UI
	public Text bullseyeText;
	public Text nearMissText;
	public GameObject WinText;

	private int dropZonesActivated;
	private int numberOfUniqueBlocks;
	private int bullseyes;
	private int nearMisses;
	private float countdownTimer = 5f;
	private GameObject ground;
	private float gameBoardMinX;
	private float gameBoardMaxX;
	private float gameBoardMinZ;
	private float gameBoardMaxZ;
	private GameObject player;
	public GameObject zombieGroup;
	public Transform zombieSpawn;
	public ZombieGroupController zombieGroupController;

	public AudioClip bullseyeSound;
	private AudioSource audioSource;


	//Singleton pattern
	void Awake(){
		if(instance == null){
			instance = this;
		} else if(instance != this){
			Destroy (gameObject);
		}
	}

	// initialization
	void Start () {
		audioSource = gameObject.GetComponent<AudioSource> ();
		initializeGame ();
	}

	void initializeGame(){
		isGameWon = false;
		bullseyes = 0;
		nearMisses = 0;
		numberOfUniqueBlocks = 3;
		dropZonesActivated = 0;
		updateUI ();
		guessedSequence = new blockColour[difficulty];
		mysterySequence = new blockColour[difficulty];
		tempArray = new blockColour[difficulty];

		//initialize game boundaries
		ground = GameObject.FindGameObjectWithTag ("Ground").gameObject;

		//resize gameboard where blocks will randomly be generated based on difficulty level
		if(difficulty > 2){
			//ground.transform.localScale += new Vector3 (3f*(2f*difficulty-1f),0,0);
			//One size fits all gameboard
			//ground.transform.localScale = new Vector3 (3f*(2f*9),0,0);
		}
		float padding = 2f;
		gameBoardMinX = ground.GetComponent<BoxCollider> ().bounds.min.x + padding;
		gameBoardMaxX = ground.GetComponent<BoxCollider> ().bounds.max.x - padding;
		gameBoardMinZ = ground.GetComponent<BoxCollider> ().bounds.min.z + padding;
		gameBoardMaxZ = ground.GetComponent<BoxCollider> ().bounds.max.z - padding;

		//Initialize Pickups
		radio.gameObject.transform.position = radioSpawn.transform.position;
		battery.gameObject.transform.position = batterySpawn.transform.position;

		//Initialize player
		player = GameObject.FindGameObjectWithTag ("Player");
		SetPlayerPosition ();
		player.GetComponent<PlayerController> ().isAlive = true;
		player.GetComponent<PlayerController> ().ResetFlareCount ();

		//Initialize Zombies
		SetZombiePosition ();

		//game starts with a mystery sequence that the player must guess
		GenerateMysterySequence ();
		SpawnDropZones();
		SpawnBlocks();

		print ("difficulty: " + difficulty);
		/* Use to create custom sequence for testing
		mysterySequence [0] = blockColour.red;
		mysterySequence [1] = blockColour.red;
		mysterySequence [2] = blockColour.blue;
		mysterySequence [3] = blockColour.yellow;
		*/
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void SpawnDropZones(){
		for(int i=0;i<difficulty;i++){
			Instantiate (dropZone, Vector3.zero, Quaternion.identity);
		}	
	}

	void SpawnBlocks(){
		int reds = 0;
		int blues = 0;
		int yellows = 0;

		for(int i=0;i<mysterySequence.Length;i++){
			if(mysterySequence[i] == blockColour.red){reds++;}
			if(mysterySequence[i] == blockColour.blue){blues++;}
			if(mysterySequence[i] == blockColour.yellow){yellows++;}
		}
		//print("reds: "+reds + ", blues: "+blues+", yellows: "+yellows);
		float blockSpawnHeight = 3f;

		for (int j = 0; j < Random.Range (1 + reds, (reds * 2)+1); j++) {
			Instantiate (redBlock, new Vector3(Random.Range(gameBoardMinX,gameBoardMaxX),blockSpawnHeight,Random.Range(gameBoardMinZ,gameBoardMaxZ)), Quaternion.identity);
		}
		for(int k=0; k < Random.Range(1 + blues, (blues * 2)+1);k++){
			Instantiate (blueBlock, new Vector3(Random.Range(gameBoardMinX,gameBoardMaxX),blockSpawnHeight,Random.Range(gameBoardMinZ,gameBoardMaxZ)), Quaternion.identity);
		}
		for(int m=0; m < Random.Range(1 + yellows, (yellows * 2)+1);m++){
			Instantiate (yellowBlock, new Vector3(Random.Range(gameBoardMinX,gameBoardMaxX),blockSpawnHeight,Random.Range(gameBoardMinZ,gameBoardMaxZ)), Quaternion.identity);
		}
	}

	public void DropZoneActivated(int dropZone, blockColour blockType){
		guessedSequence [dropZone] = blockType;
		dropZonesActivated++;

		if(dropZonesActivated == difficulty){
			evaluateBullseyes ();
			evaluateNearMisses ();
			updateUI ();
		}
	}

	public void DropZoneDeactivated(int dropZone){
		guessedSequence [dropZone] = 0;
		dropZonesActivated--;
		clearUI ();
	}

	void evaluateBullseyes(){
		bullseyes = 0;
		for(int i=0;i<mysterySequence.Length;i++){
			//duplicate mystery sequence into tempArray to make nearMiss check easier
			tempArray [i] = mysterySequence [i];
			if(guessedSequence[i] == mysterySequence[i]){
				bullseyes++;
				tempArray [i] = blockColour.noColour;
			}
		}
		print ("Bullseyes: " + bullseyes);
		if(bullseyes == difficulty){
			//Player can now pick up radio to call helicopter
			audioSource.clip = bullseyeSound;
			audioSource.Play ();
			radio.gameObject.SetActive (true);
		}
	}

	void evaluateNearMisses(){
		nearMisses = 0;
		//Bullseyes will be regarded as -1 in tempArray
		//Once a near miss has been evaluated, it becomes -1 in tempArray
		for(int i=0;i<tempArray.Length;i++){
			for(int j=0;j < guessedSequence.Length;j++){
				if(tempArray[i] == blockColour.noColour){break;}
				if(tempArray[i] == guessedSequence[j]){
					nearMisses++;
					tempArray [i] = blockColour.noColour;
				}
			}
		}
		print ("Near Misses: " + nearMisses);
	}

	void updateUI(){
		if(isGameWon){
			WinText.gameObject.SetActive (true);
			clearUI ();

		}else{
			WinText.gameObject.SetActive (false);
			bullseyeText.text = "BULLSEYES: " + bullseyes;
			//nearMissText.text = "NICE TRY: " + nearMisses;
		}

	}

	public void clearUI(){
		bullseyeText.text = "";
		nearMissText.text = "";
	}

	void GenerateMysterySequence(){
		for(int i=0;i<mysterySequence.Length;i++){
			//a value of 0 will be reserved for error handling
			mysterySequence [i] = (blockColour)Random.Range (0, numberOfUniqueBlocks);
			print ("Mystery Sequence " + i + ":" + mysterySequence [i]);
		}
	}

	void resetGame(){
		DropZone[] dropZones = GameObject.FindObjectsOfType<DropZone>();
		Pickup[] blocksList = GameObject.FindObjectsOfType<Pickup> ();
		PlayerFlare[] playerFlareList = GameObject.FindObjectsOfType<PlayerFlare> ();

		//Destroy all Dropzones
		foreach(DropZone dz in dropZones){
			Destroy (dz.gameObject);
		}
		//Destroy all blocks
		foreach(Pickup block in blocksList){
			Destroy (block.gameObject);
		}
		//Destroy all flares
		foreach(PlayerFlare pf in playerFlareList){
			Destroy (pf.gameObject);
		}
			
		DropZone.resetDropZoneID();
	}

	IEnumerator nextLevel(){
		yield return new WaitForSeconds(countdownTimer);
		resetGame ();
		difficulty++;
		initializeGame ();
	}

	void SetPlayerPosition(){
		//GameObject player = GameObject.Find ("Player");
		player.transform.position = playerSpawn.transform.position;
	}

	void SetZombiePosition(){
		zombieGroupController.ResetZombieGroup ();
	}

	public void playerDied(float speed){
		StartCoroutine ("FadeSequence",speed);
	}

	public void GameIsWon(){
		isGameWon = true;
		updateUI ();
		StartCoroutine("LoadNextLevel");
	}

	IEnumerator LoadNextLevel(){
		//Wait x=waitTime seconds and then fade out to next level
		float waitTime = 5f;
		yield return new WaitForSeconds (waitTime);
		float fadeTime = GameObject.Find ("Fading").GetComponent<Fading> ().BeginFade (1,2f);
		yield return new WaitForSeconds (fadeTime);
		SceneManager.LoadScene("start_screen");
	}

	IEnumerator FadeSequence(float speed){
		float waitTime = 1f;
		float fadeTime = GameObject.Find ("Fading").GetComponent<Fading> ().BeginFade (1,speed);
		yield return new WaitForSeconds (fadeTime);
		resetGame ();
		initializeGame ();
		yield return new WaitForSeconds (waitTime);
		GameObject.Find ("Fading").GetComponent<Fading> ().BeginFade (-1,speed);
	}
}//end of class
