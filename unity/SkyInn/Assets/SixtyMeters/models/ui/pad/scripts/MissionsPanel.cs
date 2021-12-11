using UnityEngine;

namespace SixtyMeters.models.ui.pad.scripts
{
    public class MissionsPanel : MonoBehaviour
    {
        public GameObject layout;
        public GameObject missionEntry;

        // Start is called before the first frame update
        void Start()
        {
            //TODO: move to level manager
            addMission("Cleaning the Inn", "Guests might spend more money if the inn is sparkling clean..", 20, 20);
            addMission("Open for business", "Find a way to open the portal so that guests can visit the inn", 0, 30);
            addMission("Not killing goblins", "Keep the goblins from eating your lunch without killing them", 0, 150);
            addMission("Short process", "Kill 10 goblins", 0, 5);
        }

        // Update is called once per frame
        void Update()
        {
        }

        private void addMission(string title, string desc, int percentageComplete, int coins)
        {
            GameObject entry = Instantiate(missionEntry, layout.transform);
            entry.GetComponent<MissionEntry>().SetDetails(title, desc, percentageComplete, coins);
        }
    }
}