using System;
using System.Collections.Generic;

namespace Quizo.Data.Models.Chat
{
	public class GroupChat
	{
		public string Id { get; init; } = Guid.NewGuid().ToString();

		public string GroupId { get; init; }

		public List<Message> Messages { get; set; }
	}
}
