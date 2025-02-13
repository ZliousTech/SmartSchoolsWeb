using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace SmartSchool.Hubs
{
    public class HomeworkHub : Hub
    {
        public Task JoinHomeworkGroup(string groupName)
        {
            return Groups.Add(Context.ConnectionId, groupName);
        }

        public Task SendMessageToHomeworkGroup(string groupName, string message)
        {
            return Clients.Group(groupName).ReceiveHomeworkMessage(message);
        }
    }
}