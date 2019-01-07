using System;
using System.ComponentModel.DataAnnotations;

namespace TheUnforg1venBlog.Models
{
	public class Post
	{
		public int PostID { get; set; }

		[Required(ErrorMessage = "Please enter a title")]
		public string Title { get; set; }

		[Required(ErrorMessage = "Please enter post body")]
		public string Body { get; set; }

		[Required(ErrorMessage = "Please enter tags")]
		public string Tags { get; set; }

		[Required(ErrorMessage = "Please enter description")]
		public string Description { get; set; }

		public string Image { get; set; }

		public DateTime Created { get; set; } = DateTime.Now;
	}
}
