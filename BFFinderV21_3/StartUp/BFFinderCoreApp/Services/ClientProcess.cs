using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using BFFinderCoreApp.Enums;
using BFFinderCoreApp.Models;
using BFFinderCoreApp.Services;

namespace BFFinderCoreApp.Services
{
    public class ClientProcess : Client
    {
        PacketReaderService _packetReader;
        public ClientProcess()
        {

        }

        public ClientProcess(TcpClient client)
        {
            ClientSocket = client;
            UID = Guid.NewGuid();
            _packetReader = new PacketReaderService(ClientSocket.GetStream());

            var opcode = _packetReader.ReadByte();
            List<string> message = _packetReader.ReadMessage();
            Hostname = message[1];

            Console.WriteLine($"[{DateTime.Now}]: Client has connected with the username: {Hostname}");

            Task.Run(() => Process());
        }
        void Process()
        {
            while (true)
            {
                try
                {
                    var opCode = _packetReader.ReadByte();

                    switch (opCode)
                    {
                        case (byte)PacketHeaders.Dofirst:
                            List<string> messaj = _packetReader.ReadMessage();
                            string ceva = messaj[1];
                            Console.WriteLine($"[{DateTime.Now}] - {Hostname} : {ceva}");

                            //Program.SendBackToUserMessage();

                            break;
                        case (byte)PacketHeaders.DoSecond:
                            List<string> messaje = _packetReader.ReadMessage();
                            string altceva = messaje[1];
                            Console.WriteLine($"[{DateTime.Now}] - {Hostname} : {altceva}");
                            break;

                        case (byte)PacketHeaders.LoginRequest:
                            {
                                //probabil o sa las un try catch aici
                                //primim mesajul
                                LoginService loginService = new LoginService();
                                loginService.HandleLoginRequest(_packetReader, ClientSocket);
                               
                                //ver daca merge asa
                                //daca nu, dau variabila globala list<msg> si transmit de aici mesajul
                            }
                            break;


                        default:
                            break;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine($"[{Hostname}]:Disconnected!");
                    ClientSocket.Close();
                    break;
                }
            }
        }
    }
}
