using System.ComponentModel.DataAnnotations;

namespace Quizo.Models.Groups
{
	public class JoinGroupFormModel
	{
		[Required]
		public string Id { get; set; }
		
		[Required]
		public bool IsAgreed { get; set; } = false;
	}
}
