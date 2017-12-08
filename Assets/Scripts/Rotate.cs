using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {

	public float speed = 40f;

	private Rigidbody rgbd;


	// Use this for initialization
	void Start () {
		rgbd = gameObject.GetComponent<Rigidbody> ();
	}

	// Update is called once per frame
	void Update () {
		rgbd.transform.Rotate (new Vector3 (0,speed,0) * Time.deltaTime);
	}
}
