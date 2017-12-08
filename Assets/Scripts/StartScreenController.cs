using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class StartScreenController : MonoBehaviour {

	public AudioClip hoverSound;

	private AudioSource audioSource;

	void Start(){
		audioSource = gameObject.GetComponent<AudioSource> ();
		audioSource.clip = hoverSound;
		Cursor.visible = true;
	}

	public void startGame(){
		SceneManager.LoadScene ("main");
	}

	public void quitGame(){
		Application.Quit();
	}

	public void buttonHoverSound(){
		audioSource.Play ();
	}

}