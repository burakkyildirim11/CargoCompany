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
    public partial class ChangePassword : Form
    {
        public ChangePassword()
        {
            InitializeComponent();
        }

        private void ChangeBtn_Click(object sender, EventArgs e)
        {
        
            string name = UsernameBox.Text;
            string oldPassword = PasswordBox.Text;
            string newPassword = NewPassBox.Text;


            var client = new RestClient("https://localhost:44335/api/Users/UpdateUser");
            client.Timeout = -1;
            var request = new RestRequest(Method.PUT);
            request.AddHeader("Content-Type", "application/json");
            var result = new
            {
                username = name,
                password = oldPassword,
                newPassword = newPassword

            };
            string text = JsonConvert.SerializeObject(result);
            request.AddParameter("application/json", text, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            if (response.Content.Contains("Success"))
            {
                MessageBox.Show("Success Update");
            }
            else
            {
                MessageBox.Show("Error Update");
            }


        }
    }
}
