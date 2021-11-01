using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CargoCompany
{
    public partial class SingUp : Form
    {
        public SingUp()
        {
            InitializeComponent();
        }

        private void SingUp_Load(object sender, EventArgs e)
        {

        }

        private void SingUpBtn_Click(object sender, EventArgs e)
        {
            string name = txtUsername.Text;
            string password = txtPassword.Text;

            var client = new RestClient("https://localhost:44335/api/Users/CreateUser");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");


            var result = new
            {
                username = name,
                password = password
            };
            string text = JsonConvert.SerializeObject(result);

            request.AddParameter("application/json", text, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                MessageBox.Show("Success");
            }
            else
            {
                MessageBox.Show("Error");
            }
        }
    }
}
