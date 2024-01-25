using DoAnLTW.Data;
using Microsoft.AspNetCore.Mvc;

namespace DoAnLTW.Models
{
	public class Sidebar : ViewComponent
	{
		private readonly DoAnLTWContext _context;

		public Sidebar(DoAnLTWContext context)
		{
			_context = context;
		}
		public IViewComponentResult Invoke()
		{
			return View(_context.Category.ToList());
		}
	}
}
