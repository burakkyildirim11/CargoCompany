using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoCompany.Map
{
    public static class TSP
    {
        public static string FindRoute(List<City> cities)
        {

            var stops = Enumerable.Range(0, cities.Count)
                                  .Select(i => new Stop(cities[i]))
                                  .NearestNeighbors()
                                  .ToList();

            stops.Connect(true);

            Tour startingTour = new Tour(stops);

            while (true)
            {
                Console.WriteLine(startingTour);
                var newTour = startingTour.GenerateMutations()
                                          .MinBy(tour => tour.Cost());

                if (newTour != null && newTour.Cost() < startingTour.Cost())
                {
                    startingTour = newTour;
                } 
                else break;
            }
            return startingTour.ToString();


        }

        public static void Connect(this IEnumerable<Stop> stops, bool loop)
        {
            Stop prev = null, first = null;
            foreach (var stop in stops)
            {
                if (first == null) first = stop;
                if (prev != null) prev.Next = stop;
                prev = stop;
            }

            if (loop)
            {
                prev.Next = first;
            }
        }

        private static T MinBy<T, TComparable>(
        this IEnumerable<T> xs,
        Func<T, TComparable> func)
    where TComparable : IComparable<TComparable>
        {
            return xs.DefaultIfEmpty().Aggregate(
                (maxSoFar, elem) =>
                func(elem).CompareTo(func(maxSoFar)) > 0 ? maxSoFar : elem);
        }

        private static IEnumerable<Stop> NearestNeighbors(this IEnumerable<Stop> stops)
        {
            var stopsLeft = stops.ToList();
            for (var stop = stopsLeft.First();
                 stop != null;
                 stop = stopsLeft.MinBy(s => Stop.Distance(stop, s)))
            {
                stopsLeft.Remove(stop);
                yield return stop;
            }
        }

    }
}
