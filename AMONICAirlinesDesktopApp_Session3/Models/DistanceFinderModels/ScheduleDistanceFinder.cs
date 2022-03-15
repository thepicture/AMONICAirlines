using AMONICAirlinesDesktopApp_Session3.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace AMONICAirlinesDesktopApp_Session3.Models.DistanceFinderModels
{
    public class ScheduleDistanceFinder
        : IDistanceFinder<string>
    {
        public int GetNumberOfStops_(Schedules schedule1,
                                    Schedules schedule2)
        {
            int numberOfStops = 0;
            IList<Node<Schedules>> unvisitedSet =
                new List<Node<Schedules>>();
            using (SessionThreeEntities context =
                new SessionThreeEntities())
            {
                foreach (Schedules node
                    in context.Schedules
                    .Include(s => s.Routes)
                    .Include(s => s.Routes.Airports)
                    .Include(s => s.Routes.Airports1))
                {
                    unvisitedSet
                        .Add(new Node<Schedules>
                        {
                            Vertex = node,
                            TentativeDistance = schedule1.ID == node.ID
                            ? 0
                            : int.MaxValue,
                        });
                }
            }

            var currentNode = unvisitedSet
                .First(n => n.Vertex.ID == schedule1.ID);
            while (unvisitedSet.Count != 0)
            {
                var neighbors = unvisitedSet.Where(n =>
                {
                    return n.Vertex.Routes.DepartureAirportID
                           == currentNode.Vertex.Routes.ArrivalAirportID;
                });
                foreach (var neighbor in neighbors.ToList())
                {
                    var newDistance = currentNode.TentativeDistance
                                      + neighbor.Vertex.Routes.Distance;
                    if (newDistance < neighbor.TentativeDistance)
                    {
                        neighbor.TentativeDistance = newDistance;
                    }

                    if (!unvisitedSet.Remove(currentNode))
                    {
                        throw new Exception("Cannot remove a route");
                    }

                    numberOfStops++;

                    if (currentNode.Vertex.Routes.ArrivalAirportID
                        == schedule2.Routes.DepartureAirportID)
                    {
                        return numberOfStops;
                    }
                    else
                    {
                        currentNode = unvisitedSet.First(n =>
                        {
                            return unvisitedSet
                            .Min(_n => _n.TentativeDistance)
                            == n.TentativeDistance;
                        });
                    }
                }
            }
            return -1;
        }

        public int GetNumberOfStops(string IATACode1,
                                    string IATACode2,
                                    DateTime fromDate)
        {
            var nodes = new List<Node<Schedules>>();

            return 0;
        }

        private readonly IEnumerable<string> flightNumbers =
            new List<string>();

        public override string ToString()
        {
            if (flightNumbers.Count() == 0)
            {
                return "[]";
            }
            return string.Join(" - ", flightNumbers);
        }
    }
}
