using UnityEngine;

namespace SixtyMeters.scripts.ai
{
    public class DetectItems : MonoBehaviour
    {
        public LayerMask mLayerMask;

        public bool continuousScanning;

        void Start()
        {

        }

        void FixedUpdate()
        {
            if (continuousScanning)
            {
                GetCollisions(true);
            }
        }

        /**
     * Returns GameObject if found, otherwise null
     */
        public GameObject GetClosestItemOfType<T>()
        {
            var colliders = GetCollisions(false);

            GameObject closestGameObjectOfType = null;
            float distanceToClosestCollider = 1000;
        
            foreach (var coll in colliders)
            {
                //Verify type
                var component = coll.gameObject.GetComponent<T>();
                if (component != null)
                {
                    //Verify distance to existing closest collider of type
                    var distance = Vector3.Distance(transform.position, coll.transform.position);
                    if(distanceToClosestCollider > distance)
                    {
                        closestGameObjectOfType = coll.gameObject;
                        distanceToClosestCollider = distance;
                    }    
                }
            }

            return closestGameObjectOfType;
        }
    
        Collider[] GetCollisions(bool printToLog)
        {
            //Use the OverlapBox to detect if there are any other colliders within this box area.
            Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, gameObject.transform.rotation, mLayerMask);
        

            if (printToLog)
            {
                foreach (var hitCollider in hitColliders)
                {
                    Debug.Log("Hit : " +hitCollider.name);
                }
            }

            return hitColliders;
        }

        //Fyi: Actual range is slightly wider than drawn by gizmo
        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.matrix = transform.localToWorldMatrix;

            // convert from world position to local position 
            Vector3 boxPosition = transform.InverseTransformPoint(transform.position);
            Gizmos.DrawWireCube (boxPosition, transform.localScale);
        }
    }
}
