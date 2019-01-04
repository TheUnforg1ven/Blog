using Microsoft.AspNetCore.Mvc;
using TheUnforg1venBlog.Data.Interfaces;

namespace TheUnforg1venBlog.Controllers
{
	public class HomeController : Controller
	{
		private readonly IPostRepository _postRepository;

		public HomeController(IPostRepository postRepository)
		{
			_postRepository = postRepository;
		}

		public IActionResult Index()
		{
			var posts = _postRepository.Posts;

			return View(posts);
		}

		public IActionResult Post(int postId)
		{
			var post = _postRepository.GetPost(postId);

			return View(post);
		}
	}
}
