namespace SixtyMeters.scripts.helpers.waypoints
{
    public class WaypointSeat : WayPoint
    {
        private bool _occupied;

        public bool IsOccupied()
        {
            return _occupied;
        }

        public void TakeSeat()
        {
            _occupied = true;
        }

        public void LeaveSeat()
        {
            _occupied = false;
        }
    }
}