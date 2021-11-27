using UnityEngine;

namespace SixtyMeters.scripts.items
{
    public class UsableByNpc : MonoBehaviour
    {
        
        [Tooltip("The rotation that should be applied for the item to fit into the NPCs right hand")]
        public Vector3 rightHandRotation; //apply to local rotation of object

        public bool isEquipped = false;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
