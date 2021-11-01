using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using System;
using System.Collections.Generic;

namespace CargoCompany.Map
{
    public class Stop
    {
        public Stop(City city)
        {
            City = city;
        }

        public Stop Next { get; set; }
        public City City { get; set; }

        public Stop Clone()
        {
            return new Stop(City);
        }

        public static double Distance(Stop first, Stop other)
        {
            PointLatLng latLng = new PointLatLng(first.City.X, first.City.Y);
            PointLatLng latLng2 = new PointLatLng(other.City.X, other.City.Y);

            var route = GoogleMapProvider.Instance.GetRoute(latLng, latLng2, false, false, 15);

            return route.Distance;
        }

        public IEnumerable<Stop> CanGetTo()
        {
            var current = this;
            while (true)
            {
                yield return current;
                current = current.Next;
                if (current == this) break;
            }
        }

        public override bool Equals(object obj)
        {
            return City == ((Stop)obj).City;
        }


        public override int GetHashCode()
        {
            return City.GetHashCode();
        }


        public override string ToString()
        {
            return City.CityName.ToString();
        }
    }
}
