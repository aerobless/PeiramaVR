using UnityEngine;
using UnityEngine.AI;

namespace SixtyMeters.scripts.ai.slime
{
    public class DirtSlimeAi : MonoBehaviour
    {
        // The game object from which the AI should flee from
        private GameObject _runAwayFrom;
        private NavMeshAgent _navMeshAgent;
        private const float RateLimit = 2;
        private float _nextMovementCheck = 0;
        private int _walkableNavMeshArea;

        public float fearMovementDistance = 2;
        public float randomMovementDistance = 3;


        // Start is called before the first frame update
        void Start()
        {
            //TODO: account for the possibility of having two brooms in the game at once..
            //TODO: e.g. use type to find all and then select closer one
            _runAwayFrom = GameObject.Find("Broom");
            if (_runAwayFrom == null)
            {
                //Die instantly if there is no broom in the game
                Destroy(this);
            }

            _navMeshAgent = GetComponent<NavMeshAgent>();
            _walkableNavMeshArea = NavMesh.GetAreaFromName("Walkable");
        }

        // Update is called once per frame
        void Update()
        {
            // Rate-limiting the next movement check to improve performance and to make the slime a bit easier to
            // catch for the player
            if (Time.time > _nextMovementCheck)
            {
                float dist = Vector3.Distance(transform.position, _runAwayFrom.transform.position);
                if (dist < fearMovementDistance)
                {
                    RunAwayFromBroom();
                }
                else
                {
                    RandomMovement();
                }

                _nextMovementCheck = Time.time + RateLimit;
            }
        }

        private void RunAwayFromBroom()
        {
            //temporarily point the object to look away from the player
            var groundedRunAwayFromPos = _runAwayFrom.transform.position;
            groundedRunAwayFromPos.y = transform.position.y;
            transform.rotation = Quaternion.LookRotation(transform.position - groundedRunAwayFromPos);

            //Then we'll get the position on that rotation that's multiplyBy down the path (you could set a Random.range
            // for this if you want variable results) and store it in a new Vector3 called runTo
            var runTo = transform.position + transform.forward * 2;

            //So now we've got a Vector3 to run to and we can transfer that to a location on the NavMesh with samplePosition.
            // 5 is the distance to check
            NavMesh.SamplePosition(runTo, out var hit, 5, 1 << _walkableNavMeshArea);

            // And get it to head towards the found NavMesh position
            _navMeshAgent.SetDestination(hit.position);
        }
        
        private void RandomMovement()
        {
            var randomDirection = Random.insideUnitSphere * randomMovementDistance;
            randomDirection += transform.position;

            //So now we've got a Vector3 to run to and we can transfer that to a location on the NavMesh with samplePosition.
            // 5 is the distance to check
            NavMesh.SamplePosition(randomDirection, out var hit, randomMovementDistance, 1 << _walkableNavMeshArea);
            
            // And get it to head towards the found NavMesh position
            _navMeshAgent.SetDestination(hit.position);
        }
    }
}