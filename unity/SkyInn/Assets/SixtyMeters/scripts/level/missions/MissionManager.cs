using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SixtyMeters.scripts.level.missions
{
    public class MissionManager : MonoBehaviour
    {
        private List<Mission> _missions;

        // Start is called before the first frame update
        void Start()
        {
            LoadMissions();
        }

        // Update is called once per frame
        void Update()
        {
        }

        private void LoadMissions()
        {
            _missions = new List<Mission>(GetComponentsInChildren<Mission>());
            Debug.Log(_missions.Count+"ddd");
        }

        public List<Mission> GetAllMissions()
        {
            return _missions;
        }
        
        public List<Mission> GetUnclaimedMissions()
        {
            return _missions.Where(mission => !mission.rewardWasClaimed).ToList();
        }
    }
}