using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DoAnLTW.Data;
using DoAnLTW.Models;

namespace DoAnLTW.Controllers
{
    public class ShopingCartItemsController : Controller
    {
        private readonly DoAnLTWContext _context;

        public ShopingCartItemsController(DoAnLTWContext context)
        {
            _context = context;
        }

        // GET: ShopingCartItems
        public async Task<IActionResult> Index()
        {
            var doAnLTWContext = _context.ShopingCartItem.Include(s => s.Product);
            return View(await doAnLTWContext.ToListAsync());
        }

        // GET: ShopingCartItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shopingCartItem = await _context.ShopingCartItem
                .Include(s => s.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shopingCartItem == null)
            {
                return NotFound();
            }

            return View(shopingCartItem);
        }

        // GET: ShopingCartItems/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Id");
            return View();
        }

		public IActionResult Count()
		{
			var DonDHS = _context.ShopingCartItem;
			var productCount = from product in DonDHS
							   group product by new { product.Id } into g
							   select new { Name = g.Key.Id, Totalquantity = g.Sum(d => d.Quantity) };

			return View(productCount);
		}
		public IActionResult CalculateRevenueByDay()
		{
			var orders = _context.ShopingCartItem;

			var revenueByDay = from order in orders
							   group order by new { order.Datetime.Date } into g
							   select new { Date = g.Key, TotalRevenue = g.Sum(order => order.DonGia * order.Quantity) };

			return View(revenueByDay);
		}
        public IActionResult CalculateRevenueByMonth()
        {
            var orders = _context.ShopingCartItem;

            var revenueByMonth = from order in orders
                                 group order by new { order.Datetime.Month } into g
                                 select new { Month = g.Key, TotalRevenueMonth = g.Sum(order => order.DonGia * order.Quantity) };

            return View(revenueByMonth);
        }

			// POST: ShopingCartItems/Create
			// To protect from overposting attacks, enable the specific properties you want to bind to.
			// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
			[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,sdt,Datetime,DonGia,Quantity,ProductId")] ShopingCartItem shopingCartItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shopingCartItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Id", shopingCartItem.ProductId);
            return View(shopingCartItem);
        }

        // GET: ShopingCartItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shopingCartItem = await _context.ShopingCartItem.FindAsync(id);
            if (shopingCartItem == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Id", shopingCartItem.ProductId);
            return View(shopingCartItem);
        }

        // POST: ShopingCartItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,sdt,Datetime,DonGia,Quantity,ProductId")] ShopingCartItem shopingCartItem)
        {
            if (id != shopingCartItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shopingCartItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShopingCartItemExists(shopingCartItem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.Product, "Id", "Id", shopingCartItem.ProductId);
            return View(shopingCartItem);
        }

        // GET: ShopingCartItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shopingCartItem = await _context.ShopingCartItem
                .Include(s => s.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shopingCartItem == null)
            {
                return NotFound();
            }

            return View(shopingCartItem);
        }

        // POST: ShopingCartItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shopingCartItem = await _context.ShopingCartItem.FindAsync(id);
            if (shopingCartItem != null)
            {
                _context.ShopingCartItem.Remove(shopingCartItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShopingCartItemExists(int id)
        {
            return _context.ShopingCartItem.Any(e => e.Id == id);
        }
    }
}
