using AMONICAirlinesDesktopApp_Session3.Models.Entities;

namespace AMONICAirlinesDesktopApp_Session3.Models.DistanceFinderModels
{
    class ScheduleNode : Node<Schedules>
    {
        public ScheduleNode(Schedules schedule)
        {
            Vertex = schedule;
            TentativeDistance = int.MaxValue;
        }
    }
}
