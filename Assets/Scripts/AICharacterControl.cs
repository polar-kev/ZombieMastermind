using System;
using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (UnityEngine.AI.NavMeshAgent))]
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class AICharacterControl : MonoBehaviour

    {
        public UnityEngine.AI.NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
        public ThirdPersonCharacter character { get; private set; } // the character we are controlling
        public Transform target;                                    // target to aim for
		public bool startChase;
		public float initialDelay = 20f;
		public Transform spawn;

		private GameObject flare;//Next flare to be dropped by player
		private GameObject droppedFlare;//Flare already dropped by player
		private float elapsedTime;
		private float runTime;			//Amount of time zombie will spend sprinting

        private void Start()
        {
            // get the components on the object we need ( should not be null due to require component so no need to check )
            agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
            character = GetComponent<ThirdPersonCharacter>();

	        agent.updateRotation = false;
	        agent.updatePosition = true;
			elapsedTime = 0;
			runTime = UnityEngine.Random.Range (30f, 51f);
        }


        private void Update()
		{
			//Increase timer
			elapsedTime += Time.deltaTime;
			if(elapsedTime >= elapsedTime + runTime){
				elapsedTime = 0;
			}


			//Target flare if one has been dropped, otherwise target player
			if(GameObject.FindGameObjectWithTag ("flare") != null){
				droppedFlare = GameObject.FindGameObjectWithTag ("flare");
				agent.SetDestination (droppedFlare.transform.position);
				agent.speed = UnityEngine.Random.Range (5f, 10f);				//Zombies should always run to flare
			}else if(target != null){
				agent.SetDestination (target.position);

				//Zombies will run after player once initialDelay has passed, otherwise they will walk
				if(startChase && elapsedTime >= initialDelay){
					agent.speed = UnityEngine.Random.Range (5f, 10f);
				}else{
					agent.speed = UnityEngine.Random.Range (0.1f, 1f);
				}
			}

			//Move zombie
			if(startChase){
				character.Move (agent.desiredVelocity, false, false);			
			}
		}

        public void SetTarget(Transform target)
        {
            this.target = target;
        }

		//Used for Zombie Group message broadcasts
		public void OnResetPosition(){
			elapsedTime = 0;
			agent.nextPosition = spawn.transform.position;
			agent.updatePosition = true;
			runTime = UnityEngine.Random.Range (30f, 61f);

		}

		public void OnStartWalk(){
			elapsedTime = 0;
		}
    }
}
