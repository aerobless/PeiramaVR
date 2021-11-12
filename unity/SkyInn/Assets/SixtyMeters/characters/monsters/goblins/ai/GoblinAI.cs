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
        private InnLevelManager _innLevelManager;
        private GameObject _player;

        private WayPoint _targetWaypoint;
        
        private float _nextMovementCheck;
        
        private const float RateLimit = 1;
        private const float PlayerAggressionRange = 5;
        
        // Start is called before the first frame update
        void Start()
        {
            _navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
            _player = GameObject.Find("PlayerController");
            SetupInnLevelManager();
            GoToKitchen();
        }

        // Update is called once per frame
        void Update()
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
