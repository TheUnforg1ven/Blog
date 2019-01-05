using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace TheUnforg1venBlog.Services.FileManager
{
	public class FileManager : IFileManager
	{
		private readonly string _imagePath;

		public FileManager(IConfiguration configuration)
		{
			_imagePath = configuration["Path:Images"];
		}

		public FileStream ImageStream(string fileImage)
		{
			return new FileStream(Path.Combine(_imagePath, fileImage), FileMode.Open, FileAccess.Read);
		}

		public async Task<string> SaveImage(IFormFile fileImage)
		{
			// fix taken path
			var savePath = Path.Combine(_imagePath);

			// create folder if needed
			if (!Directory.Exists(savePath))
			{
				Directory.CreateDirectory(savePath);
			}

			// get image type (jpeg/png/etc)
			var imageType = fileImage.FileName.Substring(fileImage.FileName.LastIndexOf('.'));

			// create filename 
			var fileName = $"img_{DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss")}{imageType}";

			using (var fileStream = new FileStream(Path.Combine(savePath, fileName), FileMode.Create))
			{
				await fileImage.CopyToAsync(fileStream);
			}

			return fileName;
		}
	}
}
