using System.Collections;
using System.Collections.Generic;
using RootMotion.Dynamics;
using RootMotion.FinalIK;
using SixtyMeters.scripts.ai;
using SixtyMeters.scripts.helpers.waypoints;
using SixtyMeters.scripts.items;
using SixtyMeters.scripts.level;
using UnityEngine;
using UnityEngine.AI;

namespace SixtyMeters.characters.adventurers.scripts
{
    public class InnCustomerV3 : MonoBehaviour
    {
        public BehaviourPuppet behaviourPuppet;
        public PuppetMaster puppetMaster;

        public int healthPoints = 100;

        private NavMeshAgent _navMeshAgent;
        private bool _navMeshAuto = true;
        private Animator _animator;
        private InnLevelManager _innLevelManager;
        private DetectItems _itemDetector;
        private EquipmentManagerV2 _equipmentManager;
        private InnCustomerStats _innCustomerStats;

        private float _nextCheck;
        private WayPoint _currentWaypoint;

        public InnCustomerState currentState;
        private InnCustomerState _nextState;

        private readonly Queue<WayPoint> _nextWaypoints = new();

        private const int LevelDeathFloor = -50;

        // Start is called before the first frame update
        void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _equipmentManager = GetComponent<EquipmentManagerV2>();
            _animator = GetComponentInChildren<Animator>();
            _itemDetector = GetComponentInChildren<DetectItems>();
            _innCustomerStats = GetComponent<InnCustomerStats>();
            SetupInnLevelManager();

            _nextCheck = Time.time;
            currentState = InnCustomerState.Idle;
            ChangeState(InnCustomerState.Idle);
        }

        // Update is called once per frame
        void Update()
        {
            if (_navMeshAuto) // Auto can be disabled e.g for sitting at a table
            {
                // Keep the agent disabled while the puppet is unbalanced.
                _navMeshAgent.enabled = behaviourPuppet.state == BehaviourPuppet.State.Puppet;
            }

            // Update agent destination and Animator
            if (_navMeshAgent.enabled)
            {
                _animator.SetFloat("Forward", _navMeshAgent.velocity.magnitude * 0.25f);
            }


            currentState = _nextState;
            switch (currentState)
            {
                case InnCustomerState.Idle:
                    //Check if npc should do something new, can probably be moved into idle state later
                    IdleCheck();
                    break;
                case InnCustomerState.Moving:
                    Move();
                    break;
                case InnCustomerState.SittingInInn:
                    SittingInInn();
                    break;
                case InnCustomerState.ConsumingFood:
                    ConsumingFood();
                    break;
                case InnCustomerState.PayAndLeave:
                    PayAndLeave();
                    break;
                default:
                    Debug.Log("ERROR: No switch case for " + currentState);
                    break;
            }

            DestroyAfterFallingOutOfWorld();

            if (healthPoints <= 0 && puppetMaster.state != PuppetMaster.State.Dead)
            {
                Die();
            }
        }

        private void PayAndLeave()
        {
            if (NextCheck())
            {
                Debug.Log("Paying and leaving");
                if (_currentWaypoint.GetType() == typeof(WaypointSeat))
                {
                    var wayPointSeat = ((WaypointSeat) _currentWaypoint);
                    //TODO: fixme
                    //var coinInteractionObj = coinLocation.GetComponent<InteractionObject>();
                    //_equipmentManager.InteractWith(coinInteractionObj, EquipmentSlot.RightHand);

                    _innCustomerStats.PayCoins(3, wayPointSeat.GetCoinLocation().transform);

                    _animator.SetBool("SitOnBench", false);

                    var exitWaypointTransform = wayPointSeat.exitWaypoint.transform;
                    MoveTo(exitWaypointTransform.position, exitWaypointTransform.rotation);

                    _navMeshAgent.enabled = true;
                    _navMeshAuto = true;
                    puppetMaster.SwitchToActiveMode();

                    _innLevelManager.QueuePathToPortal(_nextWaypoints);

                    ChangeState(InnCustomerState.Moving);
                    NextCheckInSeconds(5);
                }
            }
        }

        private void ConsumingFood()
        {
            if (NextCheck())
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
                    var mug = _equipmentManager.GetInteractionObject(EquipmentSlot.RightHand)
                        .GetComponentInChildren<Mug>();

                    mug.DrinkFromMug();
                    _innCustomerStats.Drink(10);

                    if (mug.IsEmpty() || !_innCustomerStats.IsThirsty())
                    {
                        _animator.SetBool("Drink", false);
                        if (_currentWaypoint.GetType() == typeof(WaypointSeat))
                        {
                            var mugDropLocation = ((WaypointSeat) _currentWaypoint).GetMugLocation();
                            _equipmentManager.Drop(EquipmentSlot.RightHand, mugDropLocation.transform);
                            Debug.Log("drop in spec. location");
                        }
                        else
                        {
                            _equipmentManager.Drop(EquipmentSlot.RightHand);
                        }

                        mug.GetComponent<UsableByNpc>().isEquipped = false; //TODO fixme
                        StartCoroutine(ChangeStateInSeconds(InnCustomerState.SittingInInn, 5));
                        NextCheckInSeconds(10);
                    }
                    else
                    {
                        NextCheckInSeconds(3);
                    }
                }
            }
        }

        private void SittingInInn()
        {
            if (NextCheck())
            {
                if (!_innCustomerStats.IsThirsty() && !_innCustomerStats.IsHungry())
                {
                    ChangeState(InnCustomerState.PayAndLeave);
                }

                var nearbyUsableItem = _itemDetector.GetClosestItemOfType<UsableByNpc>();
                if (nearbyUsableItem != null && nearbyUsableItem.GetComponent<UsableByNpc>().isEquipped == false)
                {
                    var isMug = nearbyUsableItem.GetComponent<Mug>() != null;
                    var mugIsFull = !nearbyUsableItem.GetComponent<Mug>().IsEmpty();
                    if (isMug && mugIsFull && _innCustomerStats.IsThirsty())
                    {
                        var interactionObject = nearbyUsableItem.transform.root.GetComponent<InteractionObject>();
                        nearbyUsableItem.GetComponent<UsableByNpc>().isEquipped = true; //TODO fixme

                        _equipmentManager.Equip(interactionObject, EquipmentSlot.RightHand);
                        StartCoroutine(ChangeStateInSeconds(InnCustomerState.ConsumingFood, 2));
                    }
                    //TODO: add additional items that should be handled here
                }

                NextCheckInSeconds(1);
            }
        }

        private void Move()
        {
            // Has not reached waypoint yet, walk there
            if (_currentWaypoint && !HasReachedCurrentWayPoint(0.6f))
            {
                WalkToTarget(_currentWaypoint);
                return;
            }

            // No current waypoint or has reached it. Dequeue new waypoint if available
            if ((!_currentWaypoint || HasReachedCurrentWayPoint(0.6f)) && _nextWaypoints.Count > 0)
            {
                WalkToTarget(_nextWaypoints.Dequeue());
                return;
            }


            // Has reached last waypoint in chain - Special actions
            if (HasReachedLastWayPoint(0.6f))
            {
                if (_currentWaypoint && _currentWaypoint.destination == WayPointDestination.Seat)
                {
                    _navMeshAuto = false;
                    _navMeshAgent.enabled = false; //TODO: remember to re-enable when standing up
                    transform.LookAt(_currentWaypoint.transform);
                    puppetMaster.SwitchToKinematicMode();
                    _animator.SetBool("SitOnBench", true);

                    // Lerp player into seat
                    var nextWaypointTransform = _currentWaypoint.transform;
                    StopAllCoroutines();
                    StartCoroutine(MoveTo(nextWaypointTransform.position, nextWaypointTransform.rotation));

                    ChangeState(InnCustomerState.SittingInInn);
                }
                else if (_currentWaypoint && _currentWaypoint.destination == WayPointDestination.Portal)
                {
                    //TODO: enable portal if it's deactivated
                    Destroy(transform.parent.gameObject, 1);
                }
                else
                {
                    ChangeState(InnCustomerState.Idle);
                }
            }

            // Revert to idle if there is no more movement to be done
            if (!_currentWaypoint && _nextWaypoints.Count == 0)
            {
                ChangeState(InnCustomerState.Idle);
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

        private IEnumerator ChangeStateInSeconds(InnCustomerState nextState, float seconds)
        {
            yield return new WaitForSeconds(seconds);
            ChangeState(nextState);
        }

        private void ChangeState(InnCustomerState nextState)
        {
            Debug.Log("Changing from state " + currentState + " to " + _nextState);
            _nextState = nextState;
        }

        private void IdleCheck()
        {
            if (NextCheck() && currentState == InnCustomerState.Idle)
            {
                NextCheckInSeconds(5);
                FindPlaceToSit();
            }
        }

        private void FindPlaceToSit()
        {
            ChangeState(InnCustomerState.Moving);

            _innLevelManager.QueuePathToInn(_nextWaypoints);
            _nextWaypoints.Enqueue(_innLevelManager.GetEmptySeatInInn());
        }


        private void WalkToTarget(WayPoint wayPoint)
        {
            _currentWaypoint = wayPoint;
            if (!_navMeshAgent.enabled)
            {
                return;
            }

            _navMeshAgent.SetDestination(_currentWaypoint.transform.position);
        }


        private bool HasReachedLastWayPoint(float distance)
        {
            return HasReachedCurrentWayPoint(distance) && _nextWaypoints.Count == 0;
        }

        private bool HasReachedCurrentWayPoint(float distance)
        {
            if (_currentWaypoint != null)
            {
                return Vector3.Distance(transform.position, _currentWaypoint.transform.position) <= distance;
            }

            return false;
        }

        private void SetupInnLevelManager()
        {
            _innLevelManager = FindObjectOfType<InnLevelManager>();
            if (_innLevelManager == null)
            {
                Debug.Log("InnCustomerAI needs an InnLevelManager to work, it wasn't found in the scene!");
            }
        }

        private void DestroyAfterFallingOutOfWorld()
        {
            if (transform.position.y < LevelDeathFloor)
            {
                Destroy(transform.parent.gameObject);
            }
        }

        private void Die()
        {
            //TODO: Should be implemented properly if death is needed
            Debug.Log("Inn Customer died but it doesn't support death... pls fix");
            puppetMaster.state = PuppetMaster.State.Dead;
        }

        IEnumerator MoveTo(Vector3 destPos, Quaternion destRot)
        {
            var startPos = transform.position;
            var startRot = transform.rotation;
            float fraction = 0;
            // While not there, move
            while (fraction < 1)
            {
                fraction += Time.deltaTime * 0.5f;
                transform.position = Vector3.Lerp(startPos, destPos, fraction);
                transform.rotation = Quaternion.Lerp(startRot, destRot, fraction);
                yield return null;
            }
        }
    }
}