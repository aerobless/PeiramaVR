using System;
using SixtyMeters.scripts.helpers.waypoints;
using SixtyMeters.scripts.level;
using UnityEngine;
using UnityEngine.AI;

namespace SixtyMeters.characters.monsters.goblins.ai
{
    public class GoblinAI : MonoBehaviour
    {
        
        //TODO: destroy when falling into void
        //TODO: spawn ragdoll on death
        //TODO: basic intent to go to inn
        //TODO: fight when in range of player
        //TODO: squad ai, goblins of same squad will stay together
        //TODO: can maybe be tamed when offering food instead of fight
        //TODO: disable navmesh when agent is pushed so they can be pushed off the islands
        
        private NavMeshAgent _navMeshAgent;
        private Animator _animator;
        private InnLevelManager _innLevelManager;
        private GameObject _player;

        private WayPoint _targetWaypoint;
        
        private float _nextMovementCheck;
        
        private const float RateLimit = 1;
        private const float PlayerAggressionRange = 5;
        
        Vector2 smoothDeltaPosition = Vector2.zero;
        Vector2 velocity = Vector2.zero;
        
        // Start is called before the first frame update
        void Start()
        {
            _navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
            _navMeshAgent.updatePosition = false;
            _animator = gameObject.GetComponentInChildren<Animator>();
            _player = GameObject.Find("PlayerController");
            SetupInnLevelManager();
            GoToKitchen();
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 worldDeltaPosition = _navMeshAgent.nextPosition - transform.position;

            // Map 'worldDeltaPosition' to local space
            float dx = Vector3.Dot (transform.right, worldDeltaPosition);
            float dy = Vector3.Dot (transform.forward, worldDeltaPosition);
            Vector2 deltaPosition = new Vector2 (dx, dy);

            // Low-pass filter the deltaMove
            float smooth = Mathf.Min(1.0f, Time.deltaTime/0.15f);
            smoothDeltaPosition = Vector2.Lerp (smoothDeltaPosition, deltaPosition, smooth);

            // Update velocity if time advances
            if (Time.deltaTime > 1e-5f)
                velocity = smoothDeltaPosition / Time.deltaTime;

            //bool shouldMove = velocity.magnitude > 0.5f && _navMeshAgent.remainingDistance > _navMeshAgent.radius;

            //GetComponent<LookAt>().lookAtTargetPosition = _navMeshAgent.steeringTarget + transform.forward;
            _animator.SetFloat("VelocityX", velocity.x/10);
            _animator.SetFloat("VelocityY", velocity.y/10);
            
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
            if (worldDeltaPosition.magnitude > _navMeshAgent.radius)
                _navMeshAgent.nextPosition = transform.position + 0.9f*worldDeltaPosition;
        }
        
        public void AnimatorMove ()
        {
            // Update position to agent position
            Vector3 position = _animator.rootPosition;
            position.y = _navMeshAgent.nextPosition.y;
            transform.position = position;
            
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
