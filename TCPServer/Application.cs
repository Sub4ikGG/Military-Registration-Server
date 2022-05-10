using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPServer
{
    public class Application
    {
        public string from_email;
        public string to_email;
        public int timestamp;
        public string title;
        public string description;

        public Application(string from_email, string to_email, int timestamp, string title, string description)
        {
            this.from_email = from_email;
            this.to_email = to_email;
            this.timestamp = timestamp;
            this.title = title;
            this.description = description;
        }
    }
}
