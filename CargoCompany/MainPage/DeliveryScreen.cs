using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.Json;
using System.Text.Json.Serialization;
using CargoCompany.MainPage;
using RestSharp;
using Newtonsoft.Json;
using System.Net;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using System.Threading;
using CargoCompany.Map;

namespace CargoCompany
{
    
    public partial class DeliveryScreen : Form
    {
        decimal lat;
        decimal longitude;
        private List<PointLatLng> _points;
        private MapsForm _mapForms;

        public DeliveryScreen()
        {
            InitializeComponent();
            _points = new List<PointLatLng>();
            _mapForms = new MapsForm();
            CheckForIllegalCrossThreadCalls = false;
           
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void GetCargo()
        {
            var client = new RestClient("https://localhost:44335/api/Cargos/GetAllCargo");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            var body = @"";
            request.AddParameter("text/plain", body, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            var obj = JsonConvert.DeserializeObject<dynamic>(response.Content);


            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ColumnCount = 2;
            dataGridView1.Columns[0].Name = "Kargo Adı";
            dataGridView1.Columns[0].ReadOnly = true;
            dataGridView1.Columns[1].Name = "Kargo Durumu";
            dataGridView1.Columns[0].Width = 100;
            dataGridView1.Columns[1].Width = 100;




            foreach (var item in obj)
            {
                bool durum = (item.deliveryStatus);
                dataGridView1.Rows.Add(item.cargoName, durum.ToString());
            }


        }



        private void DeliveryScreen_Load(object sender, EventArgs e)
        {
            GMapProviders.GoogleMap.ApiKey = "AIzaSyAweG5zO2CCoQsMfSQOKbEeax480pnqSis";


            map.DragButton = MouseButtons.Left;
            map.MapProvider = GMapProviders.GoogleMap;
            map.ShowCenter = false;

            map.SetPositionByKeywords("İzmit,Turkey");

            map.MinZoom = 5; //min zoom level
            map.MaxZoom = 100; //max zoom level
            map.Zoom = 10; //current zoom level

            GetCargo();
            _mapForms.Show();
        }

        GMapOverlay markers = new GMapOverlay("markers");
        private void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteCargo deleteCargo = new DeleteCargo();
            deleteCargo.Show();
        }


        private void btnLoadMap_Click(object sender, EventArgs e)
        {
            double lat = Convert.ToDouble(txtLat.Text);
            double lng = Convert.ToDouble(txtLong.Text);
            map.Position = new GMap.NET.PointLatLng(lat, lng);


            PointLatLng point = new PointLatLng(lat, lng);
            GMapMarker marker = new GMarkerGoogle(point, GMarkerGoogleType.red_dot);


            markers.Markers.Add(marker);
            map.Overlays.Add(markers);
        }

        private void map_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                var pointt = map.FromLocalToLatLng(e.X, e.Y);



                //PointLatLng point = new PointLatLng(lng, lati);
                GMapMarker newmarker = new GMarkerGoogle(pointt, GMarkerGoogleType.red_dot);
                GMapOverlay newmarkers = new GMapOverlay("markers");
                newmarkers.Markers.Add(newmarker);
                map.Overlays.Add(newmarkers);

                MessageBox.Show(pointt.Lat + " " + pointt.Lng);
                decimal.TryParse(pointt.Lat.ToString(), out lat);
                decimal.TryParse(pointt.Lng.ToString(), out longitude);

                txtLat.Text = pointt.Lat.ToString();
                txtLong.Text = pointt.Lng.ToString();
                _mapForms.Refresh();

            }
        }

        private void AddDelivery_Click(object sender, EventArgs e)
        {
            if(txtLat.Text !="" && txtLong.Text !="")
            {
                decimal.TryParse(txtLat.Text, out lat);
                decimal.TryParse(txtLong.Text, out longitude);
            }

            string kargoAdı = txtKargoAdı.Text;



            var client = new RestClient("https://localhost:44335/api/Cargos/CreateCargo");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");


            var result = new
            {
                latitude = lat,
                longitude = longitude,
                cargoName = kargoAdı,
                userId = SingIn.Id
                
            };
            string text = JsonConvert.SerializeObject(result);

            request.AddParameter("application/json", text, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                MessageBox.Show(("basarılı"));
            }
            else
            {
                MessageBox.Show((response.Content));
            }

            txtLat.Clear();
            txtLong.Clear();

            Thread clearMap = new Thread(reloadMaps);
            clearMap.Start();



        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            
        }



        private void map_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            GetCargo();
        }

        private  void reloadMaps()
        {
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
                City ct1 = new City((item.cargoName).ToString(), Convert.ToDouble(item.latitude), Convert.ToDouble(item.longitude));
              
                _mapForms.GetCargo(ct1);
                
            }


        }


        private void BtnDraw_Click(object sender, EventArgs e)
        {
            _mapForms.GetCargo(_mapForms.centerCity);
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

            var current = dataGridView1.CurrentRow;
            var client4 = new RestClient($"https://localhost:44335/api/Cargos/UpdateCargo?cargoName={current.Cells[0].Value.ToString()}");
            client4.Timeout = -1;
            var request4 = new RestRequest(Method.PUT);
            request4.AddHeader("Content-Type", "application/json");
            var result4 = new
            {
                cargoName = current.Cells[0].Value.ToString(),
                status = Convert.ToBoolean( current.Cells[1].Value.ToString())
            };
            string text4 = JsonConvert.SerializeObject(result4);
            request4.AddParameter("application/json", text4, ParameterType.RequestBody);
            IRestResponse response4 = client4.Execute(request4);
            Console.WriteLine(response4.Content);

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
