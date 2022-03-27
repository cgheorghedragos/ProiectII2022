using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using BFFinderCoreApp.Services;
using BFFinderCoreApp.Enums;

namespace BFFinderCoreApp
{
    internal class Program
    {
      
        static int port = 5000;
        static List<ClientProcess> _users;
        static TcpListener _listener;
        //static ServerInt currUser;
        static ClientProcess currUser;

        static void Main(string[] args)
        {

            Console.WriteLine($"[{DateTime.Now}]: Server has started! ");
            _users = new List<ClientProcess>();

            var taskWatcher = new ClientProcess();

            string MyIP = "";
            IPHostEntry Host = default(IPHostEntry);
            string Hostname = null;
            Hostname = System.Environment.MachineName;
            Host = Dns.GetHostEntry(Hostname);
            foreach (IPAddress IP in Host.AddressList)
            {
                if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    MyIP = Convert.ToString(IP);
                }
            }
            IPAddress localAddr = IPAddress.Parse(MyIP);
            _listener = new TcpListener(localAddr, port);


            _listener.Start();

            while (true)
            {
                try
                {
                    var client = new ClientProcess(_listener.AcceptTcpClient());
                    _users.Add(client);
                    currUser = client;
                    ConnectionToServerResponse("Connection successful!");

                }
                catch (Exception e)
                {
                    Console.WriteLine("Unable to establish a connection with a client!\n" + e.Message);
                    ConnectionToServerResponse("Connection failed!");
                }

                /*Broadcast the connection to everyone on the server */

            }
        }


        public static void ConnectionToServerResponse(string response)
        {
            var msgPacket = new PacketBuilderService();
            msgPacket.WritePacketHeaderCode(PacketHeaders.ConnectToServerResponse);
            List<string> msg = new List<string>();
            msg.Add(response);
            msgPacket.WriteMessage(msg);
            currUser.ClientSocket.Client.Send(msgPacket.GetPacketBytes());
            Console.WriteLine(msgPacket.ToString());
        }
    }
}
