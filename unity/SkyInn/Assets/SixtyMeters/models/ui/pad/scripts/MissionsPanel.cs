using SixtyMeters.scripts.level.missions;
using UnityEngine;

namespace SixtyMeters.models.ui.pad.scripts
{
    public class MissionsPanel : MonoBehaviour
    {
        public GameObject layout;
        public GameObject missionEntry;
        public MissionManager missionManager;

        private int _totalMissionCount = 0;

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            if (_totalMissionCount != missionManager.GetAllMissions().Count)
            {
                _totalMissionCount = missionManager.GetAllMissions().Count;
                //TODO: this will stop working if we add new missions at runtime..
                //maybe a better solution can be found..
                AddMissionEntries();
            }
        }
        
        private void AddMissionEntries()
        {
            foreach (var mission in missionManager.GetUnclaimedMissions())
            {
                GameObject entry = Instantiate(missionEntry, layout.transform);
                entry.GetComponent<MissionEntry>().SetMission(mission);
            }
        }
    }
}