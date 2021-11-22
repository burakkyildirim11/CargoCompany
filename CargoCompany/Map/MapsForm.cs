using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;

namespace CargoCompany.Map
{

    public partial class MapsForm : Form
    {
        public System.Threading.Timer _timer = null;
        public City centerCity = new City("İzmit", 40.7686, 29.9199);
        public GMapOverlay markers = new GMapOverlay("markers");

        public MapsForm()
        {
            InitializeComponent();
            Refresh();
        }

        public async void GetCargo(City startCity)
        {
            markers.Clear();
            var client = new RestClient("https://localhost:44335/api/Cargos/GetAllCargo");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            var body = @"";
            request.AddParameter("text/plain", body, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            var obj = JsonConvert.DeserializeObject<List<Cargo>>(response.Content);
            
            List<City> CitiesList = new List<City>(); //kargo isimleri
            List<City> CitiesListCopy = new List<City>();
            CitiesList.Add(startCity);

            GMapMarker startCityMarker = new GMarkerGoogle(new PointLatLng(Convert.ToDouble(startCity.X), Convert.ToDouble(startCity.Y)), GMarkerGoogleType.red_dot);
            markers.Markers.Add(startCityMarker);

            GMapMarker FirstMarker = new GMarkerGoogle(new PointLatLng(40.7686, 29.9199), GMarkerGoogleType.green_big_go); //merkez noktası
            markers.Markers.Add(FirstMarker);
            foreach (Cargo item in obj)
            {
                if (item.deliveryStatus != true && item.isDeleted != true)
                {
                    CitiesList.Add(new City(item.CargoName, Convert.ToDouble(item.Latitude), Convert.ToDouble(item.Longitude)));
                    PointLatLng point = new PointLatLng(Convert.ToDouble(item.Latitude), Convert.ToDouble(item.Longitude));
                    GMapMarker marker = new GMarkerGoogle(point, GMarkerGoogleType.red_dot);
                    markers.Markers.Add(marker);    
                }
            }
            map.Overlays.Add(markers);
            CitiesListCopy.AddRange(CitiesList);
            string result = TSP.FindRoute(CitiesList);
            String[] resultSplit;
            resultSplit = result.Split('>'); //resultSplit içinde, algoritmadan donen rotanın elementleri yer alıyor. (Kargo İsimleri)
            City sendCity = null;
            City sendCity2 = null;

            for (int i = 0; i < resultSplit.Length - 1; i++)
            {
                for (int j = 0; j < resultSplit.Length; j++)
                {
                    string resultArray = resultSplit[i].ToString();
                    string cityArray = CitiesListCopy[j].CityName.ToString();
                    if (resultArray.Equals(cityArray))
                    {
                        sendCity = new City(CitiesListCopy[j].CityName, CitiesListCopy[j].X, CitiesListCopy[j].Y);
                    }

                    if (resultSplit[i + 1].Equals(CitiesListCopy[j].CityName))
                    {
                        sendCity2 = new City(CitiesListCopy[j].CityName, CitiesListCopy[j].X, CitiesListCopy[j].Y);
                    }

                }
                if (sendCity != null && sendCity2 != null)
                {
                    drawRoute(sendCity, sendCity2);


                    var client1 = new RestClient("https://localhost:44335/api/Carrier/UpdateCarrier");
                    client1.Timeout = -1;
                    var request1 = new RestRequest(Method.PUT);
                    request1.AddHeader("Content-Type", "application/json");
                    var result1 = new
                    {
                        id = 1,
                        newLatitude = sendCity2.X,
                        newLongitude = sendCity2.Y,
                        cargoName = sendCity2.CityName
                    };
                    string text1 = JsonConvert.SerializeObject(result1);
                    request1.AddParameter("application/json", text1, ParameterType.RequestBody);
                    IRestResponse response1 = client1.Execute(request1); //teslim ettiği lat-long db'e eklendi
                    Console.WriteLine(response1.Content);

                    //kurye tablosundan o anki lat lonu çekip startCity'e at
                    string cityNameDb = sendCity2.CityName;
                    var client2 = new RestClient($"https://localhost:44335/api/Cargos/UpdateCargo?cargoName={cityNameDb}&status=true");
                    client2.Timeout = -1;
                    var request2 = new RestRequest(Method.PUT);
                    request2.AddHeader("Content-Type", "application/json");

                    IRestResponse response2 = client2.Execute(request2);
                    
                    await Task.Delay(10000);
                   RefreshMap();
                }

            }


        }

        private void MapsForm_Load(object sender, EventArgs e)
        {
            GMapProviders.GoogleMap.ApiKey = "YourApiKey";

            map.DragButton = MouseButtons.Left;
            map.MapProvider = GMapProviders.GoogleMap;
            map.ShowCenter = false;
            map.SetPositionByKeywords("İzmit,Turkey");
            map.MinZoom = 5; //min zoom level
            map.MaxZoom = 100; //max zoom level
            map.Zoom = 10; //current zoom level


           // GetCargo(centerCity);
        }

       
        private void btnAddPoint_Click(object sender, EventArgs e)
        {
            // _points.Add(new PointLatLng(Convert.ToDouble(txtLat.Text), Convert.ToDouble(txtLong.Text)));
        }

        private void btnClearList_Click(object sender, EventArgs e)
        {
            markers.Clear();
            var client3 = new RestClient("https://localhost:44335/api/Carrier/GetAllCarrier");
            client3.Timeout = -1;
            var request3 = new RestRequest(Method.GET);
            request3.AddHeader("Content-Type", "application/json");
            var body = @"";
            request3.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response3 = client3.Execute(request3);

            var obj3 = JsonConvert.DeserializeObject<List<dynamic>>(response3.Content);
            foreach (var item in obj3)
            {
                MapsForm mapsForm = new MapsForm();
                City ct1 = new City(item.CargoName, Convert.ToDouble(item.Latitude), Convert.ToDouble(item.Longitude));
                mapsForm.GetCargo(ct1);
            }

        }

        public void drawRoute(City first, City other)
        {

            PointLatLng latLng = new PointLatLng(first.X, first.Y);
            PointLatLng latLng2 = new PointLatLng(other.X, other.Y);

            var route = GoogleMapProvider.Instance.GetRoute(
                    latLng, latLng2, false, false, 15);
            var r = new GMapRoute(route.Points, "My Route")
            {
                Stroke = new Pen(Color.Red, 5)
            };

            var routes = new GMapOverlay("routes");
            routes.Routes.Add(r);
            map.Overlays.Add(routes);
            
            //lblDistance.Text = route.Distance + "Km";
            map.Refresh();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void map_MouseClick(object sender, MouseEventArgs e)
        {

        }

        public void RefreshMap()
        {
            map.Zoom--;
            map.Zoom++;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            markers.Clear();
            GetCargo(centerCity);
        }
    }


    public class Cargo
    {
        public int Id { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public bool deliveryStatus { get; set; }
        public bool isDeleted { get; set; }
        public string CargoName { get; set; }
    }
}
