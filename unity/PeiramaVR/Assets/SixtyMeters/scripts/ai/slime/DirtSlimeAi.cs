using UnityEngine;
using UnityEngine.AI;

namespace SixtyMeters.scripts.ai.slime
{
    public class DirtSlimeAi : MonoBehaviour
    {
        // The game object from which the AI should flee from
        private GameObject _runAwayFrom;

        public float fearDistance = 2;
        
        private NavMeshAgent _navMeshAgent;

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
        }

        // Update is called once per frame
        void Update()
        {
            //TODO: rate limit check & runaway
            float dist = Vector3.Distance(transform.position, _runAwayFrom.transform.position);
            if(dist < fearDistance)
            {
                //TODO: run away
                RunFrom();
            }
            else
            {
                //TOOD: pick random destination
            }

        }
        
        public void RunFrom()
        {
 
            // store the starting transform
            var startTransform = transform;
         
            //temporarily point the object to look away from the player
            var groundedRunAwayFromPos = _runAwayFrom.transform.position;
            groundedRunAwayFromPos.y = transform.position.y;
            transform.rotation = Quaternion.LookRotation(transform.position - groundedRunAwayFromPos);
 
            //Then we'll get the position on that rotation that's multiplyBy down the path (you could set a Random.range
            // for this if you want variable results) and store it in a new Vector3 called runTo
            Vector3 runTo = transform.position + transform.forward * 2;

            //So now we've got a Vector3 to run to and we can transfer that to a location on the NavMesh with samplePosition.
            // 5 is the distance to check
            NavMesh.SamplePosition(runTo, out var hit, 5, 1 << NavMesh.GetNavMeshLayerFromName("Walkable"));

            // reset the transform back to our start transform
            transform.position = startTransform.position;
            transform.rotation = startTransform.rotation;
 
            // And get it to head towards the found NavMesh position
            _navMeshAgent.SetDestination(hit.position);
        }
    }
}
