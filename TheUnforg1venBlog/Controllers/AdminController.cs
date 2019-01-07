using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using TheUnforg1venBlog.Data.Interfaces;
using TheUnforg1venBlog.Models;
using TheUnforg1venBlog.Services.FileManager;
using TheUnforg1venBlog.ViewModels;

namespace TheUnforg1venBlog.Controllers
{
	[Authorize(Roles = "Admin")]
	public class AdminController : Controller
	{
		private readonly IPostRepository _postRepository;

		private readonly IFileManager _fileManager;

		public AdminController(IPostRepository postRepository, IFileManager fileManager)
		{
			_postRepository = postRepository;
			_fileManager = fileManager;
		}

		public IActionResult Index()
		{
			var posts = _postRepository.Posts.Reverse();

			return View(posts);
		}

		/// <summary>
		/// Return view with existing post to edit OR return view for add new post
		/// </summary>
		/// <param name="postId">nullable id of current post</param>
		public IActionResult Edit(int? postId)
		{
			// if there are no such post
			if (postId == null)
				// view for new post creation
				return View(new PostViewModel());
			// if post exists
			else
			{
				// find neeeded post by id
				var post = _postRepository.GetPost((int)postId);

				// return view with founded post
				return View(new PostViewModel
				{
					PostID = post.PostID,
					Title = post.Title,
					Body = post.Body,
					CurrentImage = post.Image,
					Description = post.Description,
					Tags = post.Tags
				});
			}

		}

		/// <summary>
		/// Update existing post OR add new post
		/// </summary>
		/// <param name="post">post to add or edit</param>
		[HttpPost]
		public async Task<IActionResult> Edit(PostViewModel postViewModel)
		{
			if (ModelState.IsValid)
			{
				var post = new Post
				{
					PostID = postViewModel.PostID,
					Title = postViewModel.Title,
					Body = postViewModel.Body,
					Description = postViewModel.Description,
					Tags = postViewModel.Tags
				};

				if (postViewModel.Image == null)
				{
					post.Image = postViewModel.CurrentImage;
				}
				else
				{
					if (!string.IsNullOrEmpty(postViewModel.CurrentImage))
						_fileManager.RemoveImage(postViewModel.CurrentImage);

					post.Image = await _fileManager.SaveImage(postViewModel.Image);
				}
					
				// if post exists
				if (post.PostID > 0)
					_postRepository.UpdatePost(post);
				// if there no such post
				else
					_postRepository.AddPost(post);

				// all successfully saved in DB context
				if (await _postRepository.SaveChangesAsync())
					return RedirectToAction("Index");
				else
					return View(post);
			}
			else
			{
				return View(postViewModel);
			}
		}

		/// <summary>
		/// Removes existing post
		/// </summary>
		/// <param name="postId">id of post to remove</param>
		public async Task<IActionResult> Remove(int postId)
		{
			_postRepository.RemovePost(postId);

			await _postRepository.SaveChangesAsync();

			return RedirectToAction("Index");
		}
	}
}
