using Microsoft.AspNetCore.SignalR;
using Quizo.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Connections;
using System.Collections.Generic;
using System;

namespace Quizo.Hubs
{
	public class ChatHub : Hub
	{
        private readonly QuizoDbContext db;
		private static Dictionary<string, string> connectionsNgroup = new Dictionary<string, string>();


		public ChatHub(QuizoDbContext db)
		{
			this.db = db;
		}

		public async Task Send(string message, string nameGroup)
		{
			string username = Context.User.Identity.Name;
			if (connectionsNgroup.ContainsKey(Context.ConnectionId))
			{
				await Clients.Group(connectionsNgroup[Context.ConnectionId]).SendAsync("broadcastMessage", username, message);
			}
			//await Clients.Group(nameGroup).SendAsync("broadcastMessage", username, message);
		}
		public async Task JoinGroup(string groupId)
		{
			var group = db.Groups.Find(groupId);

			if (connectionsNgroup.ContainsKey(Context.ConnectionId))
			{
				await Groups.RemoveFromGroupAsync(Context.ConnectionId, connectionsNgroup[Context.ConnectionId]);
				connectionsNgroup.Remove(Context.ConnectionId);
			}
			connectionsNgroup.Add(Context.ConnectionId, group.Name);
			await Groups.AddToGroupAsync(Context.ConnectionId, group.Name);
		}

		public async override Task OnConnectedAsync()
        {
			await base.OnConnectedAsync();
        }

		public override async Task OnDisconnectedAsync(Exception exception)
		{
			if (connectionsNgroup.ContainsKey(Context.ConnectionId))
			{
				await Groups.RemoveFromGroupAsync(Context.ConnectionId, connectionsNgroup[Context.ConnectionId]);
				connectionsNgroup.Remove(Context.ConnectionId);
			}
			await base.OnDisconnectedAsync(exception);
		}
	}
}
