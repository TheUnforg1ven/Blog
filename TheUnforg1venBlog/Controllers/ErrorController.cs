using Microsoft.AspNetCore.Mvc;

namespace TheUnforg1venBlog.Controllers
{
	public class ErrorController : Controller
	{
		/// <summary>
		/// Go to error page
		/// </summary>
		/// <returns>Error view</returns>
		public ViewResult Error() => View();
	}
}
