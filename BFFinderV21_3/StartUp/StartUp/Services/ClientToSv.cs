using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using BFFinderCoreApp.Services;
using BFFinderCoreApp.Enums;

namespace StartUp.Services
{
    internal class ClientToSv
    {
        private string ip = "127.0.0.1";
        int port = 5000;
        public List<string> returnMessage { get; set; }
        private TcpClient _client;
        public PacketReaderService PacketReader;
        private static ClientToSv instance = null;
        public static ClientToSv getInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ClientToSv();
                }
                return instance;
            }
        }
        private ClientToSv()
        {
            if (_client == null)
            {
                _client = new TcpClient();
            }

            ConnectToServer();

        }
        public bool isConnected()
        {
            //if (_client != null)              //later x1 probl
            //{
            //    if (_client.Connected)
            //        return true;
            //}

            if (_client.Connected)
                return true;
            return false;
        }

        public void clearAll()
        {
            if (this.returnMessage != null)
            {
                this.returnMessage.Clear();
            }
            return;
        }

       
        public void ConnectToServer()
        {
            try
            {
                if (!_client.Connected)
                {
                    string Hostname = null;
                    if (ip == "127.0.0.1")
                    {
                        string MyIP = "";
                        IPHostEntry Host = default(IPHostEntry);
                        Hostname = System.Environment.MachineName;
                        Host = Dns.GetHostEntry(Hostname);
                        foreach (IPAddress IP in Host.AddressList)
                        {
                            if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                            {
                                MyIP = Convert.ToString(IP);
                            }
                        }
                        ip = MyIP;
                    }
                    try
                    {
                        _client.Connect(ip, port);
                        PacketReader = new PacketReaderService(_client.GetStream());
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Unable to connect to Server! Server down \n" + e.Message);
                        instance = null;
                        _client = null;
                        return;
                    }
                    if (!string.IsNullOrEmpty(Hostname))
                    {
                        List<string> message = new List<string>();
                        var connectPacket = new PacketBuilderService();
                        connectPacket.WritePacketHeaderCode(PacketHeaders.ConectToServerRequest);
                        message.Add(Hostname);
                        connectPacket.WriteMessage(message);
                        _client.Client.Send(connectPacket.GetPacketBytes());
                    }

                    ReadPackets();

                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Unable to connect to Server! Server down \n" + e.Message);
                instance = null;
                _client = null;
                return;
            }
        }

        private void ReadPackets()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        var opcode = PacketReader.ReadByte();
                        switch (opcode)
                        {
                            case 1:
                                //
                                List<string> messaje = PacketReader.ReadMessage();
                                returnMessage = messaje;
                                break;
                            case (byte)PacketHeaders.ConnectToServerResponse:
                                ConnectToServerResponse();
                                break;
                            case (byte)PacketHeaders.LoginResponse:
                                //LoginApplicationResponse();
                                LoginService loginService = new LoginService();
                                returnMessage = loginService.LoginClientResponse(PacketReader);
                                break;
                            
                                break;

                            default:
                                Console.WriteLine("ah, yes .. ");
                                break;
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Server down!");
                        instance = null;
                        _client = null;
                        break;
                    }
                }
            });
        }

        public void LoginApplicationResponse()
        {
            List<string> msg = PacketReader.ReadMessage();
            returnMessage = msg;
        }

        public void ConnectToServerResponse()
        {
            List<string> msg = PacketReader.ReadMessage();
            while (msg == null)
            {
                //just wait
            }
            while (msg.Count == 0)
            {
                //just wait
            }
            string response = msg[1];
            if (response == "Connection failed!")
            {
                MessageBox.Show("Unable to connect to server!");
            }
        }

        public TcpClient getClient()
        {
            return _client;
        }


    }
}
