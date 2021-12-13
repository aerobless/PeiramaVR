using System;
using System.Collections;
using System.Collections.Generic;
using SixtyMeters.models.portal_rune.scripts;
using SixtyMeters.scripts.helpers;
using SixtyMeters.scripts.items;
using SixtyMeters.scripts.level.missions;
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
        private Renderer _renderer;

        public Material pinkPortalPlane;
        public Material greenPortalPlane;
        
        public List<GameObject> spawnableFriendlyVisitors;
        public List<GameObject> spawnableGoblins;

        public MissionObjective missionObjective;

        public GameObject spawnPoint;
        
        private int _friendlyNpcVisitors = 0;
        private int _goblinCount = 0;
        private const int MaxFriendlyVisitors = 1;
        private const int MaxGoblins = 10;
        private const float RateLimit = 1;
        private float _nextSpawnCreatureCheck;

        private PortalLocation _selectedPortalLocation;

        // Start is called before the first frame update
        void Start()
        {
            _renderer = GetComponent<Renderer>();
            gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (_selectedPortalLocation != PortalLocation.None && Time.time > _nextSpawnCreatureCheck)
            {
                switch (_selectedPortalLocation)
                {
                    case PortalLocation.GoblinCave:
                        CheckAndSpawnGoblin();
                        break;
                    case PortalLocation.HumanCity:
                        CheckAndSpawnFriendlyVisitor();
                        break;
                }
                
                _nextSpawnCreatureCheck = Time.time + RateLimit;
            }
        
        }

        private void CheckAndSpawnFriendlyVisitor()
        {
            if (_friendlyNpcVisitors < MaxFriendlyVisitors)
            {
                var visitor = Helpers.GETRandomFromList(spawnableFriendlyVisitors);
                Instantiate(visitor, spawnPoint.transform.position, spawnPoint.transform.rotation);
                ++_friendlyNpcVisitors;
       
            }
        }
        
        private void CheckAndSpawnGoblin()
        {
            if (_goblinCount < MaxGoblins)
            {
                var goblin = Helpers.GETRandomFromList(spawnableGoblins);
                Instantiate(goblin, spawnPoint.transform.position, spawnPoint.transform.rotation);
                ++_goblinCount;    //TODO: this should be counted elsewhere.. or be decremented when goblins die
            }
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

        public void OpenPortal(PortalLocation portalLocation)
        {
            _selectedPortalLocation = portalLocation;
            switch (portalLocation)
            {
                //TODO: maybe add sound effect for distored human voice or goblin murmer depending on location
                case PortalLocation.GoblinCave:
                    _renderer.material = greenPortalPlane;
                    break;
                case PortalLocation.HumanCity:
                    _renderer.material = pinkPortalPlane;
                    break;
            }
            gameObject.SetActive(true);
            portalBackgroundNoise.Play();
            portalMotes.Play();
            blackBackground.Play();

            if (missionObjective)
            {
                missionObjective.percentageComplete = 100;
            }
        }

        public void ClosePortal()
        {
            gameObject.SetActive(false);
            portalBackgroundNoise.Stop();
            portalMotes.Stop();
            blackBackground.Stop();
            _selectedPortalLocation = PortalLocation.None;
        }
        
        
    }
}
