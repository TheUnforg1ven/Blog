using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace TheUnforg1venBlog.Services.FileManager
{
	public interface IFileManager
	{
		FileStream ImageStream(string fileImage);

		Task<string> SaveImage(IFormFile fileImage);

		bool RemoveImage(string image);
	}
}
