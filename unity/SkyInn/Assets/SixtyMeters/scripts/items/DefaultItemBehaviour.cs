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

        // Start is called before the first frame update
        void Start()
        {
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
            if (respawnWhenDestroyed && transform.position.y < LevelFloor)
            {
                transform.position = _startPosition;
                transform.rotation = _startRotation;
            }
        }
    }
}