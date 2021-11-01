using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CargoCompany.MainPage
{
    public partial class DeleteCargo : Form
    {
        //ArrayList cargoList = new ArrayList();
        //List cargoList = new List<KeyValuePair<string, int>>();
        public int deleteCargoId;
        public dynamic obj;


        public DeleteCargo()
        {
            InitializeComponent();
        }


        private void DeleteCargo_Load(object sender, EventArgs e)
        {
           
            var client = new RestClient("https://localhost:44335/api/Cargos/GetAllCargo");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            var body = @"";
            request.AddParameter("text/plain", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            obj = JsonConvert.DeserializeObject<dynamic>(response.Content);
            foreach (var item in obj)
            {

                comboBox.Items.Add(item.cargoName);
                //cargoList.Add(new KeyValuePair<string, int>(item.cargoName, item.id));
                
            }


        }

        private void btnDeleteCargo_Click(object sender, EventArgs e)
        {

            string selectedItem = comboBox.SelectedItem.ToString();
            foreach (var item in obj)
            {
                string name = item.cargoName;
                if (Equals(name, selectedItem))
                {
                    deleteCargoId = item.id;
                }
            }


            var client = new RestClient($"https://localhost:44335/api/Cargos/DeleteCargo?id={deleteCargoId}");
            client.Timeout = -1;
            var request = new RestRequest(Method.DELETE);
            request.AddHeader("Content-Type", "application/json");
 

            IRestResponse response = client.Execute(request);
            comboBox.Refresh(); //combobox refreshe bak
            MessageBox.Show("cargo deleted!");

            

        }
    }
}
