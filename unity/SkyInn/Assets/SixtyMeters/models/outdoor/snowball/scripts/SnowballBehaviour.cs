using System;
using UnityEngine;

namespace SixtyMeters.models.outdoor.snowball.scripts
{
    public class SnowballBehaviour : MonoBehaviour
    {
        
        public GameObject snowballImpact;
        
        private float activationForce = 0.5f;

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.relativeVelocity.magnitude > activationForce)
            {
                var instantiate = Instantiate(snowballImpact, transform);
                instantiate.GetComponent<SnowballImpact>().OneShot();
                Destroy(gameObject, 0.1f);
            }
        }
    }
}