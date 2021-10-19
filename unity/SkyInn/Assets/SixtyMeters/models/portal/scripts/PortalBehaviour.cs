using System;
using SixtyMeters.scripts.items;
using UnityEngine;

namespace SixtyMeters.models.portal.scripts
{
    public class PortalBehaviour : MonoBehaviour
    {
        public AudioSource audioSource;
        public AudioClip objectPassingThroughPortal;
        
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnTriggerEnter(Collider other)
        {
            var defaultItemBehaviour = other.GetComponent<DefaultItemBehaviour>();
            if (defaultItemBehaviour != null)
            {
                audioSource.PlayOneShot(objectPassingThroughPortal);
                defaultItemBehaviour.DestroyOrRespawn();
            }
        }
    }
}
