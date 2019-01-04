using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheUnforg1venBlog.Data.Interfaces;
using TheUnforg1venBlog.Models;

namespace TheUnforg1venBlog.Controllers
{
	[Authorize(Roles = "Admin")]
	public class AdminController : Controller
	{
		private readonly IPostRepository _postRepository;

		public AdminController(IPostRepository postRepository)
		{
			_postRepository = postRepository;
		}

		public IActionResult Index()
		{
			var posts = _postRepository.Posts;

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
				return View(new Post());
			// if post exists
			else
			{
				// find neeeded post by id
				var post = _postRepository.GetPost((int)postId);

				// return view with founded post
				return View(post);
			}

		}

		/// <summary>
		/// Update existing post OR add new post
		/// </summary>
		/// <param name="post">post to add or edit</param>
		[HttpPost]
		public async Task<IActionResult> Edit(Post post)
		{
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
