using UnityEngine;

namespace SixtyMeters.scripts.level.missions
{
    public class MissionObjective : MonoBehaviour
    {
        private int _percentageComplete = 0;

        public void SetPercentageComplete(int percentage)
        {
            _percentageComplete = percentage;
        }
        
        public int GetPercentageCompleted()
        {
            return _percentageComplete;
        }
    }
}