using SixtyMeters.scripts.helpers.items;

namespace SixtyMeters.scripts.helpers.waypoints
{
    public class WaypointSeat : WayPoint
    {
        public bool occupied = false;

        public ItemLocation mugLocation;
        public ItemLocation coinLocation;

        public WayPoint exitWaypoint;

        public bool IsOccupied()
        {
            return occupied;
        }

        public void TakeSeat()
        {
            occupied = true;
        }

        public void LeaveSeat()
        {
            occupied = false;
        }

        public ItemLocation GetMugLocation()
        {
            return mugLocation;
        }

        public ItemLocation GetCoinLocation()
        {
            return coinLocation;
        }

        public WayPoint GetExitWaypoint()
        {
            return exitWaypoint;
        }
    }
}