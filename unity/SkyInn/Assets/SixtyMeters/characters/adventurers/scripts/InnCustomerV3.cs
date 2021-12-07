using System;
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

        private float _nextCheck;
        private WayPoint _nextWaypoint;

        private InnCustomerState _currentState;
        private InnCustomerState _nextState;

        private readonly Queue<WayPoint> _nextWaypoints = new Queue<WayPoint>();

        private const int LevelDeathFloor = -50;
        private const float RateLimit = 1;

        // Start is called before the first frame update
        void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _equipmentManager = GetComponent<EquipmentManagerV2>();
            _animator = GetComponentInChildren<Animator>();
            _itemDetector = GetComponentInChildren<DetectItems>();
            SetupInnLevelManager();

            _nextCheck = Time.time;
            _currentState = InnCustomerState.Idle;
            _nextState = InnCustomerState.Idle;
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


            // AI Logic

            _currentState = _nextState;

            if (_currentState == InnCustomerState.Idle)
            {
                //Check if npc should do something new, can probably be moved into idle state later
                IdleCheck();
            }

            if (_currentState == InnCustomerState.Moving)
            {
                if (WayPointReached(0.6f))
                {
                    if (_nextWaypoints.Count == 0)
                    {
                        _navMeshAuto = false;
                        _navMeshAgent.enabled = false; //TODO: remember to re-enable when standing up
                        transform.LookAt(_nextWaypoint.transform);
                        puppetMaster.SwitchToKinematicMode();
                        _animator.SetBool("SitOnBench", true);

                        // Lerp player into seat
                        var nextWaypointTransform = _nextWaypoint.transform;
                        StopAllCoroutines();
                        StartCoroutine(MoveTo(nextWaypointTransform.position, nextWaypointTransform.rotation));
                        
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
                var nearbyUsableItem = _itemDetector.GetClosestItemOfType<UsableByNpc>();
                if (nearbyUsableItem != null && nearbyUsableItem.GetComponent<UsableByNpc>().isEquipped == false)
                {
                    var isMug = nearbyUsableItem.GetComponent<Mug>() != null;
                    var mugIsFull = !nearbyUsableItem.GetComponent<Mug>().IsEmpty();
                    if (isMug && mugIsFull)
                    {
                        var interactionObject = nearbyUsableItem.transform.root.GetComponent<InteractionObject>();
                        nearbyUsableItem.GetComponent<UsableByNpc>().isEquipped = true; //TODO fixme
                        
                        _equipmentManager.Equip(interactionObject, EquipmentSlot.RightHand);
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
                    var mug = _equipmentManager.GetInteractionObject(EquipmentSlot.RightHand).GetComponentInChildren<Mug>();
                    mug.DrinkFromMug();
                    if (mug.IsEmpty())
                    {
                        _animator.SetBool("Drink", false);
                        _equipmentManager.Drop(EquipmentSlot.RightHand);
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

            DestroyAfterFallingOutOfWorld();

            if (healthPoints <= 0 && puppetMaster.state != PuppetMaster.State.Dead)
            {
                Die();
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
            //TODO: re-enable inn path
            // _innLevelManager.QueuePathToInn(_nextWaypoints);
            _nextWaypoints.Enqueue(_innLevelManager.GetEmptySeatInInn());
        }


        private void WalkToTarget(WayPoint wayPoint)
        {
            _nextWaypoint = wayPoint;
            _navMeshAgent.SetDestination(_nextWaypoint.transform.position);
        }


        private bool WayPointReached(float distance)
        {
            if (_nextWaypoint != null)
            {
                return Vector3.Distance(transform.position, _nextWaypoint.transform.position) <= distance;
            }

            return true;
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
            Debug.Log("seat lerp start");
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
            Debug.Log("seat lerp end");
        }
    }
}