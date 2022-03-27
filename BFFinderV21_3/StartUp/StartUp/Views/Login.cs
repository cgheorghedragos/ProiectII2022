using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BFFinderCoreApp.Services;
using StartUp.Services;

namespace StartUp.Views
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        public Button GetLoginButton()
        {
            return LoginButton;
        }

        public TextBox GetUserNameTextBox()
        {
            return UserInput;
        }

        public TextBox GetPasswordTextBox()
        {
            return PasswordInput;
        }

        private void Login_Load(object sender, EventArgs e)
        {
           
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            //DataRepository<User> db = new DataRepository<User>();
            //db.FindUserByUsername("dragosbossu");
           
            ClientToSv _client = ClientToSv.getInstance;

            string username = UserInput.Text;
            string password = PasswordInput.Text;

            if (_client.isConnected())
            {
                // stergem orice urma de mesaje ramase cumva in interfata de client
                _client.clearAll();
                LoginService loginService = new LoginService();

                loginService.LoginRequestFunct(username, password, _client.getClient());

                MessageBox.Show("stai putin boss");
                while (_client.returnMessage == null)
                {
                    //wait a bit
                }
                if (_client.returnMessage.Count == 0)
                {
                    while (_client.returnMessage.Count == 0)
                    {

                    }
                }
                string noMsg = _client.returnMessage[0];
                if (_client.returnMessage.Count != Convert.ToInt32(noMsg) + 1)
                {
                    while (_client.returnMessage.Count != Convert.ToInt32(noMsg) + 1)
                    {

                    }
                }
                List<string> msg = _client.returnMessage;
                string response = msg[1];
                if (response == "LogInSucces!")
                {
                    
                    string userID = msg[2];
                    
                    
                }
                else
                {
                    MessageBox.Show("Wrong username or password!");
                   
                    return;
                }
            }
            else
            {
                MessageBox.Show("Server down! Please try again later!");
                
                return;
            }
        }
    }



}
