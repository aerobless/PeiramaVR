using System.Collections.Generic;
using UnityEngine;

namespace SixtyMeters.scripts.helpers.waypoints
{
    public class WayPointPath : MonoBehaviour
    {
    
        public List<WayPoint> wayPoints;
        private List<WayPoint> _reversedWaitPoints;
        public string destination;
    
        // Start is called before the first frame update
        void Start()
        {
            _reversedWaitPoints = new List<WayPoint>(wayPoints);
            _reversedWaitPoints.Reverse();
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    
        public List<WayPoint> GetWaypoints(bool reversed)
        {
            return reversed ? _reversedWaitPoints : wayPoints;
        }
    }
}
