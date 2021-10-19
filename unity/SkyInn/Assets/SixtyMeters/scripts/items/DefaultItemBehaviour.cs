using UnityEngine;

namespace SixtyMeters.scripts.items
{
    /**
     * Basic item settings and behaviour. This is a general script that should be applied to all non-static game objects.
     */
    public class DefaultItemBehaviour : MonoBehaviour
    {
        private const float LevelFloor = -30;

        [Tooltip("Whether the item should respawn when destroyed by the player")]
        public bool respawnWhenDestroyed;

        private Vector3 _startPosition;
        private Quaternion _startRotation;
        private Rigidbody _rigidbody;

        // Start is called before the first frame update
        void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            SaveStartPosition();
        }

        private void SaveStartPosition()
        {
            _startPosition = transform.position;
            _startRotation = transform.rotation;
        }

        // Update is called once per frame
        void Update()
        {
            CheckIfObjectShouldRespawn();
        }

        private void CheckIfObjectShouldRespawn()
        {
            if (transform.position.y < LevelFloor)
            {
                DestroyOrRespawn();
            }
        }

        public void DestroyOrRespawn()
        {
            if (respawnWhenDestroyed)
            {
                Respawn();
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private void Respawn()
        {
            transform.position = _startPosition;
            transform.rotation = _startRotation;
            
            //Zero out velocity, otherwise the object will continue flying at its start position
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }
    }
}