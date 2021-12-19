using UnityEngine;

namespace SixtyMeters.scripts.helpers.items
{
    public class ItemLocation : MonoBehaviour
    {
        
        void OnDrawGizmos()
        {
            // Location indicator
            Gizmos.color = Color.blue;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawCube(Vector3.zero, new Vector3(0.1f, 0.1f, 0.1f));
            
            // Direction indicator
            Gizmos.color = Color.red;
            Gizmos.DrawCube(Vector3.zero+new Vector3(0,0,0.05f), new Vector3(0.05f, 0.05f, 0.01f));   
        }
    }
    
}