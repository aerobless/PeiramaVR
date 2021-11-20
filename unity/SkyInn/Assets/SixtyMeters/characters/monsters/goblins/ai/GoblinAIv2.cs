using System;
using SixtyMeters.scripts.helpers.waypoints;
using HurricaneVR.Framework.Core;
using HurricaneVR.Framework.Core.Grabbers;
using RootMotion.Dynamics;
using SixtyMeters.scripts.level;
using UnityEngine;
using UnityEngine.AI;

namespace SixtyMeters.characters.monsters.goblins.ai
{
    public class GoblinAIv2 : MonoBehaviour
    {
        
        //TODO: destroy when falling into void
        //TODO: spawn ragdoll on death
        //TODO: basic intent to go to inn
        //TODO: fight when in range of player
        //TODO: squad ai, goblins of same squad will stay together
        //TODO: can maybe be tamed when offering food instead of fight
        //TODO: disable navmesh when agent is pushed so they can be pushed off the islands
        
        public BehaviourPuppet behaviourPuppet;
        public HVRGrabbable headGrab;
        
        private NavMeshAgent _navMeshAgent;
        private Animator _animator;
        private InnLevelManager _innLevelManager;
        private GameObject _player;

        private WayPoint _targetWaypoint;
        
        private float _nextMovementCheck;
        
        private const float RateLimit = 1;
        private const float PlayerAggressionRange = 5;
        private const int levelDeathFloor = -50;
        
        
        // Start is called before the first frame update
        void Start()
        {
            _navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
            _animator = gameObject.GetComponent<Animator>();
            _player = GameObject.Find("PlayerController");
            SetupInnLevelManager();
            
            headGrab.HandGrabbed.AddListener(OnHandGrabbed);
            headGrab.HandReleased.AddListener(OnHandReleased);
        }

        void Update()
        {
            // Keep the agent disabled while the puppet is unbalanced.
            _navMeshAgent.enabled = behaviourPuppet.state == BehaviourPuppet.State.Puppet;

            // Update agent destination and Animator
            if (_navMeshAgent.enabled)
            {
                if (Time.time > _nextMovementCheck)
                {
                    // It may be more realistic to do a ray cast or similar in a cone from the goblins face so that it
                    // doesn't see the player if it's behind a goblin.
                    if (GetDistanceToPlayer() < PlayerAggressionRange)
                    {
                        _navMeshAgent.SetDestination(_player.transform.position);
                    }
                    else
                    {
                        GoToKitchen();
                    }
                    _nextMovementCheck = Time.time + RateLimit;
                }

                _animator.SetFloat("Forward", _navMeshAgent.velocity.magnitude * 0.25f);
            }

            DestroyAfterFallingOutOfWorld();
            
        }

        private void DestroyAfterFallingOutOfWorld()
        {
            if (transform.position.y < levelDeathFloor)
            {
                Destroy(transform.parent.gameObject);
            }
        }

        private void OnHandGrabbed(HVRHandGrabber arg0, HVRGrabbable arg1)
        {
            Debug.Log("grabbed");
            behaviourPuppet.SetState(BehaviourPuppet.State.Unpinned);
            behaviourPuppet.canGetUp = false;
        }
        
        private void OnHandReleased(HVRHandGrabber arg0, HVRGrabbable arg1)
        {
            Debug.Log("hand released");
            behaviourPuppet.canGetUp = true;
        }
        
        public void AnimatorMove ()
        {
            // Update position to agent position
            Vector3 position = _animator.rootPosition;
            position.y = _navMeshAgent.nextPosition.y;
            //transform.position = position;
            
        }

        private float GetDistanceToPlayer()
        {
            return Vector3.Distance(transform.position, _player.transform.position);
        }

        private void SetupInnLevelManager()
        {
            _innLevelManager = FindObjectOfType<InnLevelManager>();
            if (_innLevelManager == null)
            {
                Debug.Log("GoblinAI needs an InnLevelManager to work, it wasn't found in the scene!");
            }
        }

        private void GoToKitchen()
        {
            _targetWaypoint = _innLevelManager.kitchen;
            _navMeshAgent.SetDestination(_targetWaypoint.transform.position);
        }
    }
}
