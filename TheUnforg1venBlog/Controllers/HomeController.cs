using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using TheUnforg1venBlog.Data.Interfaces;
using TheUnforg1venBlog.Services.FileManager;
using TheUnforg1venBlog.ViewModels;

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
			var posts = _postRepository.Posts.Reverse();

			return View(posts);
		}

		public IActionResult Post(int postId)
		{
			var post = _postRepository.GetPost(postId);

			return View(post);
		}

		/// <summary>
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

		/// <summary>
		/// Search for post by name
		/// </summary>
		/// <param name="searchString">string to search</param>
		public ViewResult Search(string searchString)
		{
			// string needed to find
			var neededString = searchString ?? "  ";

			// find posts by needed string
			var posts = _postRepository.Posts.Where(p => p.Title.Contains(neededString, StringComparison.OrdinalIgnoreCase));

			// return view of finded posts
			return View(new SearchViewModel
			{
				Posts = posts,
				SearchString = searchString
			});
		}

		public ViewResult SearchTags(string searchString)
		{
			// string needed to find
			var neededString = searchString ?? "  ";

			// find posts by needed string
			var posts = _postRepository.Posts.Where(p => p.Tags.Contains(neededString, StringComparison.OrdinalIgnoreCase));

			// return view of finded posts
			return View("Search", new SearchViewModel
			{
				Posts = posts,
				SearchString = searchString
			});
		}
	}
}
