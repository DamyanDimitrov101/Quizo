using System.ComponentModel.DataAnnotations;

namespace Quizo.Services.Groups.Models
{
	public class JoinGroupServiceModel
	{
		[Required]
		public string Id { get; set; }
		
		[Required]
		public bool IsAgreed { get; set; } = false;

		[Required]
		public bool IsJoined { get; set; } = false;
	}
}
