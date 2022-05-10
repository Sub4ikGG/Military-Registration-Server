using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace TCPServer
{
    public class ClientObject
    {
        TcpClient client;
        NetworkStream stream;

        Database db;

        private const string CODE_AUTH = "/auth";
        private const string CODE_AUTH_SUCCESS = "100";
        private const string CODE_AUTH_FAILED = "101";

        private const string CODE_SIGNUP = "/signup";
        private const string CODE_SIGNUP_SUCCESS = "200";
        private const string CODE_SIGNUP_FAILED = "201";

        private const string CODE_GET_PASSPORT = "/getpassport";
        private const string CODE_GET_PASSPORT_SUCCESS = "300";
        private const string CODE_GET_PASSPORT_FAILED = "301";

        private const string CODE_SET_PASSPORT = "/setpassport";


        public ClientObject(TcpClient client)
        {
            this.client = client;
        }

        public void Process()
        {
            stream = null;

            db = new Database();
            db.Initialize();

            while (client != null)
            {
                try
                {
                    stream = client.GetStream();

                    WaitingRequest();
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine($"Client {client.Client.RemoteEndPoint} disconnected.");
                    client.Close();

                    client = null;
                    stream = null;
                }
            }
        }

        private void WaitingRequest()
        {
            byte[] buffer = new byte[256];
            int length = stream.Read(buffer, 0, buffer.Length);
            string request = Encoding.UTF8.GetString(buffer, 0, length).Trim();

            if (request.Length == 0) throw new Exception("Disconnected.");
            Console.WriteLine($"Server got request from {client.Client.RemoteEndPoint}: {request}");
            switch (request.Split('-')[0])
            {
                case CODE_AUTH:
                    request = request.Split("- ")[1];

                    var email = request.Split()[0];
                    var password = request.Split()[1];

                        if (email.Length > 6 && password.Length > 4)
                        {
                            var u = db.AuthorizateUser(email, password);
                            if (u.first_name != "-1") SendAnswer(JsonConvert.SerializeObject(u));
                            else SendAnswer(CODE_AUTH_FAILED);
                        }
                        else
                            SendAnswer(CODE_AUTH_FAILED);
                    break;
                case CODE_SIGNUP:
                    var json = request.Split('-')[1];
                    var user = JsonConvert.DeserializeObject<User>(json);

                    db.CreateUser(user);

                    SendAnswer(CODE_SIGNUP_SUCCESS);
                    break;

                case CODE_GET_PASSPORT:
                    var em = request.Split()[1];
                    var passport = db.GetPassport(em);

                    if(passport.email != "-1")
                    {
                        SendAnswer(JsonConvert.SerializeObject(passport));
                    }
                    else SendAnswer(CODE_GET_PASSPORT_FAILED);
                    break;

                case CODE_SET_PASSPORT:
                    Console.WriteLine(request.Split()[1]);
                    var pass = JsonConvert.DeserializeObject<Passport>(request.Split()[1]);

                    if (db.SetPassport(pass))
                    {
                        SendAnswer("OK");
                    }
                    else SendAnswer("NOT OK");


                    break;
            }
        }

        private void SendAnswer(string answer)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(answer);
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();

            Console.WriteLine($"Server sent request to {client.Client.RemoteEndPoint}: {answer}");
        }
    }
}
