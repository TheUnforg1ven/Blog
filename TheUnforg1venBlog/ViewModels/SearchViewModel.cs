using System.Collections.Generic;
using TheUnforg1venBlog.Models;

namespace TheUnforg1venBlog.ViewModels
{
	public class SearchViewModel
	{
		public IEnumerable<Post> Posts { get; set; }

		public string SearchString { get; set; }
	}
}
