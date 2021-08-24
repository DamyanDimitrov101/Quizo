using Microsoft.Extensions.Configuration;
using Quizo;

namespace MusicStore.Test
{
	public class TestStartup : Startup
	{
		public TestStartup(IConfiguration configuration)
			: base(configuration)
		{
		}
	}
}