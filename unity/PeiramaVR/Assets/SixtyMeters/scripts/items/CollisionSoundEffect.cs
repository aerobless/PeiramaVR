using System;
using UnityEngine;

namespace SixtyMeters.scripts.items
{
    public class CollisionSoundEffect : MonoBehaviour
    {

        public AudioSource audioSource;
        public AudioClip audioClip;
        
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
            if (collision.relativeVelocity.magnitude > 0.2)
            {
                float audioLevel = collision.relativeVelocity.magnitude / 10.0f;
                audioSource.PlayOneShot(audioClip, audioLevel);   
            }
        }
    }
}
