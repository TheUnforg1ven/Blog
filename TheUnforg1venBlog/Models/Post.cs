using System;

namespace TheUnforg1venBlog.Models
{
	public class Post
	{
		public int PostID { get; set; }

		public string Title { get; set; }

		public string Body { get; set; }

		public DateTime Created { get; set; } = DateTime.Now;
	}
}
