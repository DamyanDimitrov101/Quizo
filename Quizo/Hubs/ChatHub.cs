using Microsoft.AspNetCore.SignalR;
using Quizo.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Quizo.Data.Models.Chat;
using System.Linq;

namespace Quizo.Hubs
{
	public class ChatHub : Hub
	{
        private readonly QuizoDbContext data;
		private static Dictionary<string, string> connectionsNgroup = new Dictionary<string, string>();

		public ChatHub(QuizoDbContext data)
		{
			this.data = data;
		}

		public async Task Send(string message, string nameGroup, string groupId)
		{
			string username = Context.User.Identity.Name;
			if (connectionsNgroup.ContainsKey(Context.ConnectionId))
			{
				await Clients
					.Group(connectionsNgroup[Context.ConnectionId])
					.SendAsync("broadcastMessage", username, message);

				var groupChat = this.data.GroupChats.FirstOrDefault(gc => gc.GroupId == groupId);

				if(groupChat is null)
				{
					var newGroupChat = new GroupChat
					{
						GroupId = groupId
					};
					groupChat = newGroupChat;
					this.data.GroupChats.Add(newGroupChat);
					await this.data.SaveChangesAsync();
				}

				var newMessage = new Message
				{
					GroupChatId = groupChat.Id,
					OwnerName = username,
					Text = message,
					Time = DateTime.Now.ToLongTimeString()
				};

				groupChat.Messages.Add(newMessage);
				await this.data.SaveChangesAsync();
			}
		}
		public async Task JoinGroup(string groupId)
		{
			var group = data.Groups.Find(groupId);

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
