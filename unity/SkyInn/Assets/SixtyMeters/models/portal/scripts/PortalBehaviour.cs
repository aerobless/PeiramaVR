using System;
using System.Collections;
using System.Collections.Generic;
using SixtyMeters.scripts.helpers;
using SixtyMeters.scripts.items;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SixtyMeters.models.portal.scripts
{
    public class PortalBehaviour : MonoBehaviour
    {
        public AudioSource soundEffects;
        public AudioClip objectPassingThroughPortal;
        
        public AudioSource portalBackgroundNoise;

        public ParticleSystem portalMotes;
        public ParticleSystem blackBackground;

        public GameObject spawnPoint;
        
        private int _friendlyNpcVisitors = 0;
        private const int MaxFriendlyVisitors = 1;

        public List<GameObject> spawnableFriendlyVisitors;


        // Start is called before the first frame update
        void Start()
        {
            gameObject.SetActive(false);
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
                soundEffects.PlayOneShot(objectPassingThroughPortal);
                defaultItemBehaviour.DestroyOrRespawn();
            }
        }

        public void OpenPortal()
        {
            gameObject.SetActive(true);
            portalBackgroundNoise.Play();
            portalMotes.Play();
            blackBackground.Play();
            if (_friendlyNpcVisitors < MaxFriendlyVisitors)
            {
                StartCoroutine(SpawnVisitor());
            }
        }

        public void ClosePortal()
        {
            gameObject.SetActive(false);
            portalBackgroundNoise.Stop();
            portalMotes.Stop();
            blackBackground.Stop();
        }

        private IEnumerator SpawnVisitor()
        {
            yield return new WaitForSeconds(10);
            var randomVisitor = Helpers.GETRandomFromList(spawnableFriendlyVisitors);
            Instantiate(randomVisitor, spawnPoint.transform.position, spawnPoint.transform.rotation);
            ++_friendlyNpcVisitors;
        }
        
    }
}
