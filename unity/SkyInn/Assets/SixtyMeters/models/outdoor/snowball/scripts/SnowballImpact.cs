using System.Collections.Generic;
using SixtyMeters.scripts.helpers;
using UnityEngine;

namespace SixtyMeters.models.outdoor.snowball.scripts
{
    public class SnowballImpact : MonoBehaviour
    {
        
        public AudioSource audioSource;
        public List<AudioClip> snowballHit;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void OneShot()
        {
            var randomHit = Helpers.GETRandomFromList(snowballHit);
            audioSource.PlayOneShot(randomHit);
            Destroy(gameObject, 5);
        }
    }
}
