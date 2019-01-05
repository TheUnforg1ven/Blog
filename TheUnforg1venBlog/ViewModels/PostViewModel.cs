using Microsoft.AspNetCore.Http;

namespace TheUnforg1venBlog.ViewModels
{
	public class PostViewModel
	{
		public int PostID { get; set; }

		public string Title { get; set; }

		public string Body { get; set; }

		public IFormFile Image { get; set; }
	}
}
