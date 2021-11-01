using System;
using System.Collections.Generic;
using System.Linq;

namespace CargoCompany.Map
{
    public class Tour
    {
        public Tour(IEnumerable<Stop> stops)
        {
            Anchor = stops.First();
        }

        public IEnumerable<Tour> GenerateMutations()
        {
            for (Stop stop = Anchor; stop.Next != Anchor; stop = stop.Next)
            {
                Stop current = stop.Next.Next;
                while (current != Anchor)
                {
                    yield return CloneWithSwap(stop.City, current.City);
                    current = current.Next;
                }
            }
        }


        public Stop Anchor { get; set; }


        public Tour CloneWithSwap(City firstCity, City secondCity)
        {
            Stop firstFrom = null, secondFrom = null;
            var stops = UnconnectedClones();
            stops.Connect(true);

            foreach (Stop stop in stops)
            {
                if (stop.City == firstCity) firstFrom = stop;

                if (stop.City == secondCity) secondFrom = stop;
            }

            var firstTo = firstFrom.Next;
            var secondTo = secondFrom.Next;

            firstTo.CanGetTo()
                   .TakeWhile(stop => stop != secondTo)
                   .Reverse()
                   .Connect(false);

            firstTo.Next = secondTo;
            firstFrom.Next = secondFrom;

            var tour = new Tour(stops);
            return tour;
        }


        public IList<Stop> UnconnectedClones()
        {
            return Cycle().Select(stop => stop.Clone()).ToList();
        }


        public double Cost()
        {
            return Cycle().Aggregate(
                0.0,
                (sum, stop) =>
                sum + Stop.Distance(stop, stop.Next));
        }


        private IEnumerable<Stop> Cycle()
        {
            return Anchor.CanGetTo();
        }


        public override string ToString()
        {
            string path = String.Join(
                ">",
                Cycle().Select(stop => stop.ToString()).ToArray());
            return String.Format("{0}", path);
        }

    }
}
