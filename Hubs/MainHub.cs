using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace SistemaDeGestionDeFilas.Hubs
{
    public class MainHub : Hub
    {
        private static readonly String AGENTEACTIVO = "estage1";
        private static readonly String AGENTEINACTIVO = "estage2";
        public static IHubContext HubContext
        {
            get 
            {
                return GlobalHost.ConnectionManager.GetHubContext<MainHub>();
            }
        }

        public void CallTicket(String groupName, dynamic ticket)
        {
            Clients.Group(groupName).CallTicket(ticket);
        }

        public void AddTicketCalled(String groupName, dynamic ticket)
        {
            Clients.Group(groupName).AddTicketCalled(ticket);
        }

        public void RemoveTicket(String groupName, dynamic ticket)
        {
            Clients.Group(groupName).RemoveTicket(ticket);
        }

        public Task JoinGroup(String groupName)
        {
            return Groups.Add(Context.ConnectionId, groupName); 
        }

        public Task LeaveGroup(String groupName)
        {
            return Groups.Remove(Context.ConnectionId, groupName);
        }

        public void Length(Int32 length)
        {
            Clients.All.length(length);
        }

        public void RefreshAll()
        {
            Clients.All.RefreshAll();
        }

        internal static void SendAtencionesLength(Int32 length)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<MainHub>();
            context.Clients.All.GetAtencionesLength(length);
        }

        public void Exportada(dynamic exportada)
        {
            Clients.All.exportada(exportada);
        }
    }
}