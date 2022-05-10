using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPServer
{
    public class Passport
    {
        public string email;
        public int series;
        public int number;
        public string date;
        public int division_code;

        public Passport(string email, int series, int number, string date, int division_code)
        {
            this.email = email;
            this.series = series;
            this.number = number;
            this.date = date;
            this.division_code = division_code;
        }
    }
}
