using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Quizo.Hubs
{
	public class ChatHub : Hub
	{
		public async Task Send(string name, string message)
		{
			await Clients.All.SendAsync("broadcastMessage", name, message);
		}
	}
}
