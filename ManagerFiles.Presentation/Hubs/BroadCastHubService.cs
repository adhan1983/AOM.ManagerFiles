using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using ManagerFiles.Presentation.Contants;

namespace ManagerFiles.Presentation.Hubs
{
    public class BroadCastHubService : Hub
    {
        public Task Feedback(string message) => Clients.Caller.SendAsync(ManagerFilesConstants.FEEDBACK, message);        
    }
}
