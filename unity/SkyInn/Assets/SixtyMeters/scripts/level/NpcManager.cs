using System;
using System.Collections.Generic;
using SixtyMeters.characters.adventurers.scripts;
using SixtyMeters.characters.monsters.goblins.ai;
using SixtyMeters.models.portal.scripts;
using UnityEngine;

namespace SixtyMeters.scripts.level
{
    public class NpcManager : MonoBehaviour
    {
        public PortalBehaviour portal;
        
        private float _nextCheck;

        private List<GoblinAIv2> _goblins = new();
        private List<InnCustomerV3> _innCustomers = new();
        
        // Start is called before the first frame update
        void Start()
        {
            _nextCheck = Time.time;
        }

        // Update is called once per frame
        void Update()
        {
            if (NextCheck())
            {
                switch (portal.GetPortalLocation())
                {
                    case PortalLocation.GoblinCave:
                        if (_goblins.Count < 5)
                        {
                            portal.SpawnGoblin();
                            portal.SpawnGoblin();
                            portal.SpawnGoblin();
                        }
                        break;
                    case PortalLocation.HumanCity:
                        if (_innCustomers.Count < 1)
                        {
                            portal.SpawnInnCustomer();
                        }
                        break;
                    case PortalLocation.None:
                        break;
                }
                //TODO: check how many npcs are in world and spawn some if necessary
                
                NextCheckInSeconds(15);
            }
        }
        
        private bool NextCheck()
        {
            return Time.time >= _nextCheck;
        }

        private void NextCheckInSeconds(float seconds)
        {
            _nextCheck = Time.time + seconds;
        }

        public void RegisterGoblin(GoblinAIv2 goblin)
        {
            _goblins.Add(goblin);
        }
        
        public void DeregisterGoblin(GoblinAIv2 goblin)
        {
            _goblins.Remove(goblin);
        }
        
        public void RegisterInnCustomer(InnCustomerV3 customer)
        {
            _innCustomers.Add(customer);
        }
        
        public void DeregisterInnCustomer(InnCustomerV3 customer)
        {
            _innCustomers.Remove(customer);
        }
    }
}