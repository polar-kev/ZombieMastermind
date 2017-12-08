using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fading : MonoBehaviour {

	public Texture2D fadeOutTexture;		//Texture that will overlay the screen
	public float fadeSpeed = 2.0f;			//Speed of fade

	private int drawDepth = -1000;			//Texture's order in the draw hierarchy
	private float alpha = 1.0f;				//Texture alpha
	private int fadeDir = -1;			//direction to fade: in =-1, out=1

	void OnAwake(){
		SceneManager.sceneLoaded += this.OnLoadCallback;
	}
	void OnGUI(){
		alpha += fadeDir * fadeSpeed * Time.deltaTime;
		alpha = Mathf.Clamp01 (alpha); 														//force alpha to a value between 0 and 1

		GUI.color = new Color (GUI.color.r, GUI.color.g, GUI.color.b, alpha);
		GUI.depth = drawDepth;
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), fadeOutTexture);		//Overlay fadeout texture on entire screen
	}

	public float BeginFade(int direction, float speed){
		speed = fadeSpeed;
		fadeDir = direction;
		return fadeSpeed;
	} 

	void OnLoadCallback(Scene scene, LoadSceneMode sceneMode)
	{
		alpha = 1;
		BeginFade (-1,2f);
	}
}
