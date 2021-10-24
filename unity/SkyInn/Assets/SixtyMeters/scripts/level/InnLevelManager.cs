using System.Collections.Generic;
using System.Linq;
using SixtyMeters.scripts.helpers;
using SixtyMeters.scripts.helpers.waypoints;
using UnityEngine;

namespace SixtyMeters.scripts.level
{
    public class InnLevelManager : MonoBehaviour
    {
        public List<WaypointSeat> seatsInInn;

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        public WaypointSeat GetEmptySeatInInn()
        {
            var emptySeats = seatsInInn.Where(seat => !seat.IsOccupied()).ToList();
            return Helpers.GETRandomFromList(emptySeats);
        }
    }
}