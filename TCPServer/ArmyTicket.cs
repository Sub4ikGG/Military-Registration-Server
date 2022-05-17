using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPServer
{
    public class ArmyTicket
    {
        public string email;
        public string ticket_id;

        public ArmyTicket(string email, string ticket_id)
        {
            this.email = email;
            this.ticket_id = ticket_id;
        }
    }
}
