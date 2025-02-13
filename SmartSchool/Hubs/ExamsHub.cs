using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace SmartSchool.Hubs
{
    public class ExamsHub : Hub
    {
        public Task JoinExamGroup(string groupName)
        {
            return Groups.Add(Context.ConnectionId, groupName);
        }

        public Task SendMessageToGroup(string groupName, string message)
        {
            return Clients.Group(groupName).ReceiveMessage(message);
        }
    }
}