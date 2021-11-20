using System.Collections;
using System.Collections.Generic;
using Oculus.Platform.Models;
using SixtyMeters.scripts.helpers.waypoints;
using SixtyMeters.scripts.items;
using SixtyMeters.scripts.level;
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
        
        private readonly Queue<WayPoint> _nextWaypoints = new Queue<WayPoint>();
        
        private WayPoint _nextWaypoint;

        private InnCustomerState _currentState;
        private InnCustomerState _nextState;
        private int _pathIndex;
        private string _destination;

        private InnLevelManager _innLevelManager;

        private EquipmentManager _equipmentManager;
        

        // Start is called before the first frame update
        void Start()
        {
            SetupInnLevelManager();

            _equipmentManager = GetComponent<EquipmentManager>();
            _animator = GetComponent<Animator>();
            _navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
            _ikControl = gameObject.GetComponent<IkControl>();

            _nextCheck = Time.time;
            _navMeshAgent.updatePosition = false;
            _navMeshAgent.updateRotation = true;

            // Initial state
            _currentState = InnCustomerState.Idle;
            _nextState = InnCustomerState.Idle;
        }

        private void SetupInnLevelManager()
        {
            _innLevelManager = FindObjectOfType<InnLevelManager>();
            if (_innLevelManager == null)
            {
                Debug.Log("InnCustomerAI needs an InnLevelManager to work, it wasn't found in the scene!");
            }
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

            if (_currentState == InnCustomerState.Moving)
            {
                if (WayPointReached(0.5f))
                {
                    if (_nextWaypoints.Count == 0)
                    {
                        _animator.SetBool("Walk", false);
                        _navMeshAgent.enabled = false; //TODO: remember to re-enable when standing up
                        _animator.SetBool("SitOnBench", true);
                        _nextState = InnCustomerState.SittingInInn;
                    }
                    else
                    {
                        WalkToTarget(_nextWaypoints.Dequeue());   
                    }
                }
            }

            if (_currentState == InnCustomerState.SittingInInn && NextCheck())
            {
                var nearbyUsableItem =
                    gameObject.GetComponentInChildren<DetectItems>().GetClosestItemOfType<UsableByNpc>();
                if (nearbyUsableItem != null && nearbyUsableItem.GetComponent<UsableByNpc>().isEquipped == false)
                {
                    // Handle Mug & check that it isn't empty
                    if (nearbyUsableItem.GetComponent<Mug>() != null && !nearbyUsableItem.GetComponent<Mug>().IsEmpty())
                    {
                        _equipmentManager.EquipRightHand(nearbyUsableItem);
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
                    var mug = _equipmentManager.rightHand.GetComponentInChildren<Mug>();
                    mug.DrinkFromMug();
                    if (mug.IsEmpty())
                    {
                        _animator.SetBool("Drink", false);
                        _equipmentManager.DropRightHand();
                        StartCoroutine(ChangeStateInSeconds(InnCustomerState.SittingInInn, 5));
                        NextCheckInSeconds(10);
                    }
                    else
                    {
                        NextCheckInSeconds(3);
                    }
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
                FindPlaceToSit();
            }
        }

        private void FindPlaceToSit()
        {
            _nextState = InnCustomerState.Moving;

            //TODO: logic to choose a seat
            _innLevelManager.QueuePathToInn(_nextWaypoints);
            _nextWaypoints.Enqueue(_innLevelManager.GetEmptySeatInInn());
        }

        private void WalkToTarget(WayPoint wayPoint)
        {
            _nextWaypoint = wayPoint;
            _navMeshAgent.SetDestination(_nextWaypoint.transform.position);
            _animator.SetBool("Walk", true);
        }


        private bool WayPointReached(float distance)
        {
            if (_nextWaypoint != null)
            {
                return Vector3.Distance(transform.position, _nextWaypoint.transform.position) <= distance;
            }

            return true;
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