using DoAnLTW.Data;
using Microsoft.AspNetCore.Mvc;

namespace DoAnLTW.Models
{
	public class Search : ViewComponent
	{
		private readonly DoAnLTWContext _context;

		public Search(DoAnLTWContext context)
		{
			_context = context;
		}
		public IViewComponentResult Invoke()
		{
			return View(_context.Category.ToList());
		}
	}
}
