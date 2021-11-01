using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CargoCompany.Map
{
    public class City
    {
        public City(string cityName, double x, double y)
        {
            X = x;
            Y = y;
            CityName = cityName;
        }
        public double X { get; private set; }

        public double Y { get; private set; }

        public string CityName { get; private set; }
    }
}
