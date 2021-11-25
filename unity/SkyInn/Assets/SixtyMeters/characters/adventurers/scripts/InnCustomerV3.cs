using System.Collections;
using System.Collections.Generic;
using RootMotion.Dynamics;
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
        private Animator _animator;
        private InnLevelManager _innLevelManager;

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
            _navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
            _animator = gameObject.GetComponentInChildren<Animator>();  //TODO: chang me back
            SetupInnLevelManager();

            _nextCheck = Time.time;
            _currentState = InnCustomerState.Idle;
            _nextState = InnCustomerState.Idle;
        }

        // Update is called once per frame
        void Update()
        {
            // Keep the agent disabled while the puppet is unbalanced.
            _navMeshAgent.enabled = behaviourPuppet.state == BehaviourPuppet.State.Puppet;

            // Update agent destination and Animator
            if (_navMeshAgent.enabled)
            {
                //LOGIC for active AI bot here

                if (_currentState != _nextState)
                {
                    //State transition;
                    _currentState = _nextState;
                }

                if (_currentState == InnCustomerState.Idle)
                {
                    //TODO: idle logic
                }

                if (_currentState == InnCustomerState.Moving)
                {
                    if (WayPointReached(0.5f))
                    {
                        if (_nextWaypoints.Count == 0)
                        {
                           
                            _navMeshAgent.enabled = false; //TODO: remember to re-enable when standing up
                            transform.LookAt(_nextWaypoint.transform);
                            puppetMaster.SwitchToKinematicMode();
                            _animator.SetBool("SitOnBench", true);
                            _nextState = InnCustomerState.SittingInInn;
                            //TODO: maybe need to disable physics for sitting
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
                        if (nearbyUsableItem.GetComponent<Mug>() != null &&
                            !nearbyUsableItem.GetComponent<Mug>().IsEmpty())
                        {
                            //TODO: fixme
                            //_equipmentManager.EquipRightHand(nearbyUsableItem);
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
                        //TODO: fixme
                        /*var mug = _equipmentManager.rightHand.GetComponentInChildren<Mug>();
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
                        }*/
                    }
                }

                //Check if npc should do something new, can probably be moved into idle state later
                IdleCheck();


                _animator.SetFloat("Forward", _navMeshAgent.velocity.magnitude * 0.25f);
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
            _innLevelManager.QueuePathToInn(_nextWaypoints);
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
    }
}