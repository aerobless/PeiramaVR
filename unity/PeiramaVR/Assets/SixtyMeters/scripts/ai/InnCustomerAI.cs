using System.Collections.Generic;
using SixtyMeters.scripts.helpers;
using UnityEngine;
using UnityEngine.AI;

namespace SixtyMeters.scripts.ai
{
    public class InnCustomerAI : MonoBehaviour
    {
        private Animator _animator;
        private NavMeshAgent _navMeshAgent;
        private float _nextCheck;
        private WayPoint _nextTarget;
        private Dictionary<string, List<WayPoint>> _destinations = new Dictionary<string, List<WayPoint>>();

        private InnCustomerState _currentState;
        private InnCustomerState _nextState;
        private int _pathIndex;
        private string _destination;

        public List<WayPointPath> wayPointPaths;

        // Start is called before the first frame update
        void Start()
        {
            _animator = GetComponent<Animator>();
            _navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
            _nextCheck = Time.time;
            _navMeshAgent.updatePosition = false;
            _navMeshAgent.updateRotation = true;

            // Initial state
            _currentState = InnCustomerState.Idle;
            _nextState = InnCustomerState.Idle;

            LoadWaypointPaths();
        }

        private void LoadWaypointPaths()
        {
            _destinations = new Dictionary<string, List<WayPoint>>();
            wayPointPaths.ForEach(path =>
            {
                _destinations.Add("To" + path.destination, path.GetWaypoints(false));
                _destinations.Add("From" + path.destination, path.GetWaypoints(true));
            });
        }

        // Update is called once per frame
        void Update()
        {
            //Navmesh Sync
            transform.position = new Vector3(transform.position.x, _navMeshAgent.nextPosition.y, transform.position.z);
            _navMeshAgent.nextPosition = transform.position;

            //Statemachine
            if (_currentState != _nextState)
            {
                //State transition;
                _currentState = _nextState;
            }

            if (_currentState == InnCustomerState.Idle)
            {
                _animator.SetBool("Walk", false);
            }

            if (_currentState == InnCustomerState.FollowPath)
            {
                _animator.SetBool("Walk", true);
                
                if (targetReached())
                {
                    NextPathStep();
                }
            }
            

            //Check if npc should do something new, can probably be moved into idle state later
            idleCheck();
        }

        private void idleCheck()
        {
            if (Time.time > _nextCheck && _currentState == InnCustomerState.Idle)
            {
                NextCheckInSeconds(30);
                FollowPath("ToCity");
            }
        }

        public void FollowPath(string destination)
        {
            Debug.Log("Following to " + destination);
            _nextState = InnCustomerState.FollowPath;
            _pathIndex = 0;
            _destination = destination;
            if (_destinations[_destination] == null)
            {
                Debug.Log("Destination " + destination + " does not exist!");
            }

            _nextTarget = _destinations[_destination][_pathIndex];
            _navMeshAgent.SetDestination(_nextTarget.transform.position);
        }

        private void NextPathStep()
        {
            _pathIndex += 1;
            Debug.Log("NextPathStep "+_pathIndex);
            
            if (_destinations[_destination].Count > _pathIndex)
            {
                _nextTarget = _destinations[_destination][_pathIndex];
                _navMeshAgent.SetDestination(_nextTarget.transform.position);
            }
            else
            {
                _nextState = InnCustomerState.Idle;
            }
        }

        private bool targetReached()
        {
            if (_nextTarget != null)
            {
                return Vector3.Distance(transform.position, _nextTarget.transform.position) <= 1;
            }
            else
            {
                return true;
            }
        }


        private void NextCheckInSeconds(float seconds)
        {
            _nextCheck = Time.time + seconds;
        }
    }
}