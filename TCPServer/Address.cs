using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPServer
{
    public class Address
    {
        public string email;
        public string city;
        public string street;
        public string house_number;
        public int flat_number;

        public Address(string email, string city, string street, string house_number, int flat_number)
        {
            this.email = email;
            this.city = city;
            this.street = street;
            this.house_number = house_number;
            this.flat_number = flat_number;
        }
    }
}
