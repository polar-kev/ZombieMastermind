using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieGroupController : MonoBehaviour {

	public void ResetZombieGroup(){
		BroadcastMessage("OnResetPosition");
	}
}
