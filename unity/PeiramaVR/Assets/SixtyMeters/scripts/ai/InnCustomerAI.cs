using System.Collections;
using System.Collections.Generic;
using HurricaneVR.Framework.Core;
using HurricaneVR.Framework.Core.Utils;
using SixtyMeters.scripts.helpers;
using SixtyMeters.scripts.items;
using UnityEngine;
using UnityEngine.AI;

namespace SixtyMeters.scripts.ai
{
    public class InnCustomerAI : MonoBehaviour
    {
        private Animator _animator;
        private NavMeshAgent _navMeshAgent;
        private IkControl _ikControl;
        
        private float _nextCheck;
        private WayPoint _nextTarget;
        private Dictionary<string, List<WayPoint>> _destinations = new Dictionary<string, List<WayPoint>>();

        private InnCustomerState _currentState;
        private InnCustomerState _nextState;
        private int _pathIndex;
        private string _destination;

        public List<WayPointPath> wayPointPaths;

        public GameObject dummyObject;

        //Temporary marker for sitting spot, should be replaced with logic to choose from available seats
        public WayPoint seatMarker;

        // Start is called before the first frame update
        void Start()
        {
            _animator = GetComponent<Animator>();
            _navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
            _ikControl = gameObject.GetComponent<IkControl>();
            
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

                if (WayPointReached(1))
                {
                    NextPathStep();
                }
            }

            if (_currentState == InnCustomerState.FindPlaceToSit)
            {
                if (WayPointReached(0.5f))
                {
                    //TODO: sit down logic
                    _animator.SetBool("Walk", false);
                    _navMeshAgent.enabled = false; //TODO: remember to re-enable when standing up
                    _animator.SetBool("SitOnBench", true);
                    _nextState = InnCustomerState.SittingInInn;
                }
            }

            if (_currentState == InnCustomerState.SittingInInn)
            {
                //TODO: reduce rate of searching for a mug
                var closestMug = gameObject.GetComponentInChildren<DetectItems>().GetClosestItemOfType<UsableByNpc>();
                if (closestMug != null && closestMug.GetComponent<UsableByNpc>().isEquipped == false)
                {
                    GetComponent<EquipmentManager>().EquipRightHand(closestMug);
                    StartCoroutine(StartDrinking());
                }
            }

            //Check if npc should do something new, can probably be moved into idle state later
            idleCheck();
        }
        
        IEnumerator StartDrinking()
        {
            yield return new WaitForSeconds(10);
            _animator.SetBool("Drink", true);
        }

        private void idleCheck()
        {
            if (Time.time > _nextCheck && _currentState == InnCustomerState.Idle)
            {
                NextCheckInSeconds(30);
                //FollowPath("ToCity");
                FindPlaceToSit();
            }
        }

        public void FindPlaceToSit()
        {
            //_navMeshAgent.enabled = false;
            _nextState = InnCustomerState.FindPlaceToSit;

            //TODO: logic to choose a seat
            WalkToTarget(seatMarker);
        }

        private void WalkToTarget(WayPoint wayPoint)
        {
            _nextTarget = wayPoint;
            _navMeshAgent.SetDestination(_nextTarget.transform.position);
            _animator.SetBool("Walk", true);
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

            WalkToTarget(_destinations[_destination][_pathIndex]);
        }

        private void NextPathStep()
        {
            _pathIndex += 1;
            Debug.Log("NextPathStep " + _pathIndex);

            if (_destinations[_destination].Count > _pathIndex)
            {
                WalkToTarget(_destinations[_destination][_pathIndex]);
            }
            else
            {
                _nextState = InnCustomerState.Idle;
            }
        }

        private bool WayPointReached(float distance)
        {
            if (_nextTarget != null)
            {
                return Vector3.Distance(transform.position, _nextTarget.transform.position) <= distance;
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