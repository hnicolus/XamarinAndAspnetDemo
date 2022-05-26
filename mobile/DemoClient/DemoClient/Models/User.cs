using System;
namespace DemoClient.Models
{
	public class User
	{
		public User()
		{
		}


		public Guid Id { get; set; }
		public string FirstName { get; set; } = string.Empty;
		public string LastName { get; set; } = string.Empty;

	}
}

