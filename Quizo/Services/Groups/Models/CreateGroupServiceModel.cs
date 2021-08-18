using System.ComponentModel.DataAnnotations;
using static Quizo.Data.DataConstants.Group;

namespace Quizo.Services.Groups.Models
{
	public class CreateGroupServiceModel
	{
		[Required]
		[StringLength(MaxLength, MinimumLength = MinLength, ErrorMessage = "The name should be at least {2} symbols and no longer than {1}.")]
		public string Name { get; set; }

		[Required]
		[StringLength(DescriptionMaxLength, MinimumLength = MinLength, ErrorMessage = "The description should be at least {2} symbols and no longer than {1}.")]
		public string Description { get; set; }

		[Required]
		[Url]
		public string ImageUrl { get; set; }
	}
}
