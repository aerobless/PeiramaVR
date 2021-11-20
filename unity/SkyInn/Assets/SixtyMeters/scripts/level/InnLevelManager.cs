using System.Collections;
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
        public WayPointPath pathToInn;
        public WayPoint kitchen;

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
            var chosenSeat = Helpers.GETRandomFromList(emptySeats);
            chosenSeat.TakeSeat();
            
            //TODO: handle case if all seats are taken
            //TODO: add logic that releases seat again
            return chosenSeat;
        }

        public void QueuePathToInn(Queue<WayPoint> queue)
        {
            foreach (var wayPoint in pathToInn.wayPoints)
            {
                queue.Enqueue(wayPoint);
            }
        }
    }
}