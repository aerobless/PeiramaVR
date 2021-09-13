using System.Collections;
using System.Collections.Generic;
using HurricaneVR.Framework.Core;
using HurricaneVR.Framework.Core.Utils;
using SixtyMeters.scripts.helpers;
using SixtyMeters.scripts.items;
using Unity.VisualScripting;
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

            if (_currentState == InnCustomerState.SittingInInn && NextCheck())
            {
                var nearbyUsableItem =
                    gameObject.GetComponentInChildren<DetectItems>().GetClosestItemOfType<UsableByNpc>();
                if (nearbyUsableItem != null && nearbyUsableItem.GetComponent<UsableByNpc>().isEquipped == false)
                {
                    // Handle Mug
                    if (nearbyUsableItem.GetComponent<Mug>() != null)
                    {
                        GetComponent<EquipmentManager>().EquipRightHand(nearbyUsableItem);
                        StartCoroutine(ChangeStateInSeconds(InnCustomerState.ConsumingFood, 5));
                    }
                    //TODO: add additional items that should be handled here
                }

                NextCheckInSeconds(1);
            }

            if (_currentState == InnCustomerState.ConsumingFood && NextCheck())
            {
                //TODO: handle drinking vs. eating
                if (!_animator.GetBool("Drink"))
                {
                    // Drink animation loops at 90frames -> 3 seconds
                    _animator.SetBool("Drink", true);
                    
                    // First nextCheck is in 1.5s which should be when the Mug is at the mouth for the first time
                    NextCheckInSeconds(1.5f);
                }
                else
                {
                    var mug = GetComponent<EquipmentManager>().rightHand.GetComponentInChildren<Mug>();
                    mug.DrinkFromMug();
                    if (mug.IsEmpty())
                    {
                        _animator.SetBool("Drink", false);
                    }
                    NextCheckInSeconds(3);
                }
            }

            //Check if npc should do something new, can probably be moved into idle state later
            IdleCheck();
        }

        private IEnumerator ChangeStateInSeconds(InnCustomerState nextState, float seconds)
        {
            yield return new WaitForSeconds(seconds);
            _nextState = nextState;
        }

        private void IdleCheck()
        {
            if (NextCheck() && _currentState == InnCustomerState.Idle)
            {
                NextCheckInSeconds(30);
                //FollowPath("ToCity");
                FindPlaceToSit();
            }
        }

        private void FindPlaceToSit()
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

        private bool NextCheck()
        {
            return Time.time >= _nextCheck;
        }

        private void NextCheckInSeconds(float seconds)
        {
            _nextCheck = Time.time + seconds;
        }
    }
}