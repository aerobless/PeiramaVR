namespace SixtyMeters.scripts.helpers.waypoints
{
    public class WaypointSeat : WayPoint
    {
        public bool occupied = false;

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
    }
}