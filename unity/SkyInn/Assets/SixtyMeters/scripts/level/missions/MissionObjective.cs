using UnityEngine;

namespace SixtyMeters.scripts.level.missions
{
    public class MissionObjective : MonoBehaviour
    {
        public int percentageComplete = 0;

        public void SetPercentageComplete(int percentage)
        {
            percentageComplete = percentage;
        }
        
        public int GetPercentageCompleted()
        {
            return percentageComplete;
        }
    }
}