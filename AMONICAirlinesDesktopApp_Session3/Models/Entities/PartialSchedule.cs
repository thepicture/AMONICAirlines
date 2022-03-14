using AMONICAirlinesDesktopApp_Session3.Models.DistanceFinderModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;

namespace AMONICAirlinesDesktopApp_Session3.Models.Entities
{
    public partial class Schedules : INotifyPropertyChanged
    {
        private string flightNumbers;

        public decimal BusinessPrice => EconomyPrice * (decimal)1.35;
        public decimal FirstClassPrice => BusinessPrice * (decimal)1.30;
        public string FlightNumbers
        {
            get => flightNumbers;
            set
            {
                flightNumbers = value;
                PropertyChanged?
                    .Invoke(this,
                    new PropertyChangedEventArgs(nameof(FlightNumbers)));
            }
        }
        public int NumberOfStops
        {
            get
            {
                IList<string> flightNumbers = new List<string>();
                IList<Node<Routes>> unvisitedSet =
                    new List<Node<Routes>>();
                using (SessionThreeEntities context =
                    new SessionThreeEntities())
                {
                    foreach (Routes node
                        in context.Routes
                        .Include(r => r.Airports1)
                        .Include(r => r.Airports))
                    {
                        unvisitedSet
                            .Add(new Node<Routes>
                            {
                                Vertex = node,
                                TentativeDistance = Routes.ID == node.ID
                                ? 0
                                : int.MaxValue,
                            });
                    }
                }

                var currentNode = unvisitedSet
                    .First(n => n.Vertex.ID == Routes.ID);
                while (unvisitedSet.Count != 0)
                {
                    var neighbors = unvisitedSet.Where(n =>
                    {
                        return n.Vertex.DepartureAirportID
                               == currentNode.Vertex.ArrivalAirportID;
                    });
                    foreach (var neighbor in neighbors.ToList())
                    {
                        var newDistance = currentNode.TentativeDistance
                                          + neighbor.Vertex.Distance;
                        if (newDistance < neighbor.TentativeDistance)
                        {
                            neighbor.TentativeDistance = newDistance;
                        }

                        if (!unvisitedSet.Remove(currentNode))
                        {
                            throw new Exception("Cannot remove a route");
                        }

                        flightNumbers.Add(currentNode.Vertex.Airports.IATACode);
                        FlightNumbers = string.Join(" - ", flightNumbers);
                        if (currentNode.Vertex.DepartureAirportID
                            == Routes.ArrivalAirportID)
                        {
                            return flightNumbers.Count - 2;
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
                throw new Exception("Route does not exist");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
