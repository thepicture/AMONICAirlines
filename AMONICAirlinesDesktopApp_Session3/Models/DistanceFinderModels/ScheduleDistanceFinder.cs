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
        public int GetNumberOfStops(string IATACode1,
                                    string IATACode2,
                                    DateTime fromDate)
        {
            flightNumbers.Clear();
            IList<ScheduleNode> nodes = new List<ScheduleNode>();
            using (SessionThreeEntities context = new SessionThreeEntities())
            {
                nodes = context
                    .Schedules
                    .Include(s => s.Routes)
                    .Include(s => s.Routes.Airports)
                    .Include(s => s.Routes.Airports1)
                    .Where(s => s.Date >= fromDate)
                    .OrderBy(s => s.Date)
                    .ToList()
                    .ConvertAll(s => new ScheduleNode(s));
            }
            if (nodes.Count == 0)
            {
                return -1;
            }
            var currentNode = nodes
                .FirstOrDefault(n =>
                {
                    return n.Vertex.Date >= fromDate
                    && n.Vertex.Routes.Airports.IATACode == IATACode1;
                });
            var visitedIATACodes = new List<string>();
            if (currentNode == null)
            {
                return -1;
            }
            string currentNodeCachedFlightNumber = 
                currentNode.Vertex.FlightNumber;
            if (currentNode.Vertex.Routes.Airports.IATACode == IATACode1
                && currentNode.Vertex.Routes.Airports1.IATACode == IATACode2)
            {
                flightNumbers.Add(currentNode.Vertex.FlightNumber);
                return 0;
            }
            currentNode.TentativeDistance = 0;
            while (nodes.Count != 0)
            {
                if (currentNode.Vertex.Routes.Airports1.IATACode
                 == IATACode2)
                {
                    int numberOfFlights = 1;
                    while (currentNode.Parent != null)
                    {
                        flightNumbers
                            .Add(currentNode.Vertex.FlightNumber);
                        numberOfFlights++;
                        currentNode.Parent = currentNode.Parent.Parent;
                    }
                    if (flightNumbers.Count == 0)
                    {
                        flightNumbers.Add(currentNodeCachedFlightNumber);
                        flightNumbers.Add(currentNode.Vertex.FlightNumber);
                    }
                    return numberOfFlights;
                }
                if (!nodes.Remove(currentNode))
                {
                    throw new Exception("Can't remove a node");
                }
                var neighbors = nodes.Where(n =>
                {
                    return n
                    .Vertex
                    .Routes
                    .DepartureAirportID
                           == currentNode
                           .Vertex
                           .Routes
                           .ArrivalAirportID;
                });
                foreach (var neighbor in neighbors)
                {
                    if (visitedIATACodes
                        .Contains(neighbor.Vertex.Routes.Airports.IATACode))
                    {
                        continue;
                    }
                    visitedIATACodes
                        .Add(neighbor.Vertex.Routes.Airports.IATACode);
                    var newDistance = currentNode.TentativeDistance
                                      + neighbor.Vertex.Routes.Distance;
                    if (newDistance < neighbor.TentativeDistance)
                    {
                        neighbor.TentativeDistance = newDistance;
                        neighbor.Parent = currentNode;
                    }
                }
                if (nodes.Count == 0)
                {
                    return -1;
                }
                currentNode = nodes.First(n1 =>
                {
                    return n1.TentativeDistance
                           == nodes
                           .Min(n2 => n2.TentativeDistance);
                });
            }
            return -1;
        }

        private readonly IList<string> flightNumbers =
            new List<string>();

        public override string ToString()
        {
            if (flightNumbers.Count() == 0)
            {
                return "[]";
            }
            return string.Join(" - ",
                               flightNumbers.Select(f => $"[{f}]"));
        }
    }
}
