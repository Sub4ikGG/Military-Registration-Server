using System;
using System.Net;
using System.Net.Sockets;

namespace TCPServer
{
    class Server
    {
        public static void Main()
        {
            TcpListener server = new TcpListener(IPAddress.Any, 25565);
            Console.WriteLine($"Server started: {server.LocalEndpoint}");
            server.Start();

            while(true) /*Слушатель подключений*/
            {
                TcpClient client = server.AcceptTcpClient();
                IPAddress ipAddress = ((IPEndPoint)client.Client.RemoteEndPoint).Address;
                ClientObject clientObject = new ClientObject(client);

                Thread thread = new Thread(clientObject.Process);
                thread.Start();

                Console.WriteLine($"Client {ipAddress} connected.");
            }
        }
    }
}