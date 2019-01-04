using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheUnforg1venBlog.Data.Interfaces;
using TheUnforg1venBlog.Models;

namespace TheUnforg1venBlog.Data
{
	public class PostRepository : IPostRepository
	{
		private readonly ApplicationDbContext _applicationDbContext;

		public PostRepository(ApplicationDbContext applicationDbContext)
		{
			_applicationDbContext = applicationDbContext;
		}

		public IEnumerable<Post> Posts => _applicationDbContext.Posts;

		public Post GetPost(int postId)
			=> _applicationDbContext.Posts
			.FirstOrDefault(p => p.PostID == postId); 


		public void AddPost(Post post)
		{
			_applicationDbContext.Posts.Add(post);
		}

		public void RemovePost(int postId)
		{
			var dbEntry = _applicationDbContext.Posts.FirstOrDefault(p => p.PostID == postId);

			if (dbEntry != null)
				_applicationDbContext.Posts.Remove(dbEntry);
		}

		public void UpdatePost(Post post)
		{
			_applicationDbContext.Posts.Update(post);
		}

		public async Task<bool> SaveChangesAsync()
		{
			if (await _applicationDbContext.SaveChangesAsync() > 0)
				return true;

			return false;
		}
	}
}
