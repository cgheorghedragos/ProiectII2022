using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using BFFinderCoreApp.Enums;
using BFFinderCoreApp.Models;
using BFFinderCoreApp.Repository;

namespace BFFinderCoreApp.Services
{
    public class LoginService
    {
        public void LoginRequestFunct(string username, string password, TcpClient _client)
        {
            var connectPacket = new PacketBuilderService();
            connectPacket.WritePacketHeaderCode(PacketHeaders.LoginRequest);
            List<string> message = new List<string>();

            //messages to transmit
            message.Add(username);
            message.Add(password);

            connectPacket.WriteMessage(message);
            _client.Client.Send(connectPacket.GetPacketBytes());
        }

        public List<string> LoginClientResponse(PacketReaderService PacketReader)
        {
            List<string> msg = PacketReader.ReadMessage();
            return msg;
        }

        public void LoginServerResponse(string response, TcpClient ClientSocket, string userID = "")
        {
            var msgPacket = new PacketBuilderService();
            msgPacket.WritePacketHeaderCode(PacketHeaders.LoginResponse);
            List<string> msg = new List<string>();
            msg.Add(response);
            if (userID != "")
            {
                msg.Add(userID);
            }
            msgPacket.WriteMessage(msg);

            ClientSocket.Client.Send(msgPacket.GetPacketBytes());
        }

        public void HandleLoginRequest(PacketReaderService packetReader,TcpClient ClientSocket)
        {
            List<string> message = packetReader.ReadMessage();

            string username = message[1];
            string password = message[2];

            DataRepository<User> dataRepo = new DataRepository<User>();

            User searchUser = dataRepo.FindUserByUsername(username); ;
            

            if (searchUser != null)
            {
                Console.WriteLine(searchUser.FirstName + " " + password);
                LoginServerResponse("LogInSucces!", ClientSocket, searchUser.Id.ToString());


            }
            else
            {
                Console.WriteLine("HAHA NOT CONECTED");
                LoginServerResponse("LogInFailed", ClientSocket);
            }
        }

        
    }
}
