using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPServer
{
    public class ApplicationHandler
    {
        public void Process()
        {
            Database db = new Database();
            db.Initialize();

            while(true)
            {
                Thread.Sleep(10 * 1000);

                var a = db.GetUserApplications();
                if (a.applications == null) continue;

                foreach(var app in a.applications)
                {
                    db.AcceptUserApplication(app.from_email, app.title);
                    db.SendNotification(new Notification(app.to_email, app.from_email, $"Ваше заявление /{app.title}/ принято!"));

                    switch (app.title)
                    {
                        case "Заявление: постановка на воинский учет":
                            Random random = new Random();
                            int first = random.Next(1000, 9999);
                            int second = random.Next(100, 999);

                            ArmyTicket ticket = new ArmyTicket(app.from_email, $"{second}-{first}");
                            db.SetArmyTicket(ticket);
                            db.SendNotification(new Notification(app.to_email, app.from_email, $"Вам присвоена ID-карта с номером {ticket.ticket_id}"));

                            break;
                    }

                    Thread.Sleep(1 * 1000);
                }
            }
        }
    }
}