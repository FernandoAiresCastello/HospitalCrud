using Microsoft.AspNetCore.Mvc;

namespace HospitalCrud.Controllers
{
	/// <summary>
	/// The controller for the index (home) view
	/// </summary>
	public class IndexController : Controller
	{
		private readonly ILogger<IndexController> logger;

		public IndexController(ILogger<IndexController> logger)
		{
			this.logger = logger;
		}

		public IActionResult Index()
		{
			return View();
		}
	}
}
