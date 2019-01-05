using Microsoft.AspNetCore.Mvc;
using TheUnforg1venBlog.Data.Interfaces;
using TheUnforg1venBlog.Services.FileManager;

namespace TheUnforg1venBlog.Controllers
{
	public class HomeController : Controller
	{
		private readonly IPostRepository _postRepository;

		private readonly IFileManager _fileManager;

		public HomeController(IPostRepository postRepository, IFileManager fileManager)
		{
			_postRepository = postRepository;
			_fileManager = fileManager;
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

		/// <summary>
		/// 
		/// Static image = drag from the folder and put in the needed place
		/// Dynamic image = stream it
		/// </summary>
		/// <param name="image"></param>
		/// <returns></returns>
		[HttpGet("/Image/{image}")]
		public IActionResult Image(string image)
		{
			var imageType = image.Substring(image.LastIndexOf('.') + 1);

			return new FileStreamResult(_fileManager.ImageStream(image), $"image/{imageType}");
		}
	}
}
