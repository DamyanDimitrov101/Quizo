using AutoMapper;
using Quizo.Data.Models;
using Quizo.Data.Models.Identity;
using Quizo.Models.Identity;
using Quizo.Models.Questions;
using Quizo.Services.Groups.Models;

namespace Quizo.Infrastructure
{
	public class MappingProfile : Profile	
	{
		public MappingProfile()
		{
			this.CreateMap<CreateGroupServiceModel, Group>();
			this.CreateMap<User, UserViewModel>();

			this.CreateMap<AddQuestionFormModel, Question>();
			this.CreateMap<Answer, CorrectAnswers>();

		}
	}
}
