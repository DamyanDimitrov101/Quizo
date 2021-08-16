namespace Quizo.Data
{
	public class DataConstants
	{
		public class Question
		{
			public const int MaxLength = 30;
			public const int MinLength = 5;
			public const string LengthErrorMessage = "The {0} must be at least {2} and at max {1} characters long.";
		}
		public class Answer
		{
			public const int MaxLength = 20;
			public const int MinLength = 3;
			public const string LengthErrorMessage = "The {0} must be at least {2} and at max {1} characters long.";
		}

		public class Group
		{
			public const int MaxLength = 35;
			public const int MinLength = 4;
			public const int DescriptionMaxLength = 1400;
		}

		public class User
		{
			public const int MaxLength = 40;
			public const int PassMaxLength = 30;
			public const int PassMinLength = 6;
			public const int MinLength = 3;
			public const string LengthErrorMessage = "The {0} must be at least {2} and at max {1} characters long.";
			public const string PassMatchErrorMessage = "The password and confirmation password do not match.";
		}
	}
}
