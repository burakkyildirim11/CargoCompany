using CargoCompany.Map;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CargoCompany
{
    public partial class SingIn : Form
    {
        
        public static int Id;
        public SingIn()
        {
            InitializeComponent();
        }

        private void SingIn_Load(object sender, EventArgs e)
        {
            
        }

        private void singUpBtn_Click(object sender, EventArgs e)
        {
            SingUp frm2 = new SingUp();
            frm2.Show();

        }

        private void changePasswd_Click(object sender, EventArgs e)
        {
            ChangePassword frm3 = new ChangePassword();
            frm3.Show();
        }

        private void singInBtn_Click(object sender, EventArgs e)
        {
            //DeliveryScreen frmMain = new DeliveryScreen();
            this.SetVisibleCore(false);

            string name = txtUsername.Text;
            string password = txtPassword.Text;

            var client = new RestClient("https://localhost:44335/api/Users/GetUserLoginControl");
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
            var obj = JsonConvert.DeserializeObject<dynamic>(response.Content);
            Id = obj.id;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                //frmMain.Show();
                //Thread th2 = new Thread(new ThreadStart(MapsForm));
                //Thread th1 = new Thread(new ThreadStart(MainScreen));

                //th1.Start();
                //th2.Start();
                DeliveryScreen main = new DeliveryScreen();
                main.Show();
            }
            else
            {
                MessageBox.Show((response.Content));
            }

           

        }

        private static void MapsForm()
        {
            
        }

        private static void MainScreen()
        {
           

        }
    }
}
