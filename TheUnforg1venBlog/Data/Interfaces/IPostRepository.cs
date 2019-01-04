using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TheUnforg1venBlog.Models;

namespace TheUnforg1venBlog.Data.Interfaces
{
	public interface IPostRepository
	{
		IEnumerable<Post> Posts { get; }

		Post GetPost(int postId);

		void RemovePost(int postId);

		void AddPost(Post post);

		void UpdatePost(Post post);

		Task<bool> SaveChangesAsync();
	}
}
