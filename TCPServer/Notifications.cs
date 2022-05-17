using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPServer
{
    public class Notifications
    {
        public Notification[] notifications;

        public Notifications(Notification[] notifications)
        {
            this.notifications = notifications;
        }
    }
}
