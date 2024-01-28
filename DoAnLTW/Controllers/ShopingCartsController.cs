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
    public class ShopingCartsController : Controller
    {
        private readonly DoAnLTWContext _context;

        public ShopingCartsController(DoAnLTWContext context)
        {
            _context = context;
        }

        public ActionResult AddToCart(int productId, int quantity)
		{
			// Lấy sản phẩm từ cơ sở dữ liệu hoặc nguồn dữ liệu khác
			Product productToAdd = GetProductById(productId);

            if (productToAdd != null)
            {
                if (_context.ShopingCart.Count() == 0)
                {
                    _context.ShopingCart.Add(new ShopingCart());
                    _context.SaveChanges();
                }

                ShopingCart existingItem = _context.ShopingCart.FirstOrDefault(item => item.Product.Id == productId);

                if (existingItem != null)
                {
                    existingItem.Quantity += quantity;
                }
                else
                {
                    _context.ShopingCart.Add(new ShopingCart { Product = productToAdd, Quantity = quantity });
                }

                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
        private List<ShopingCart> GetShoppingCartWithTotal()
        {
            var shoppingCart = _context.ShopingCart.Include(c => c.Product.Id).FirstOrDefault();

            if (shoppingCart != null)
            {
                // Tính tổng giá trị giỏ hàng
                decimal total = _context.ShopingCart.Sum(item => item.Product.Price * item.Quantity);

                ViewBag.Total = total;
                return _context.ShopingCart.ToList();
            }

            ViewBag.Total = 0;
            return new List<ShopingCart>();
        }

        private Product GetProductById(int productId)
		{
				var product = _context.Product.FirstOrDefault(p => p.Id == productId);
				return product;
		}

        //------------------------------------------------------------------------------
        // GET: ShopingCarts
        public ActionResult Index()
        {
            return View(GetShoppingCartWithTotal());
        }

        // GET: ShopingCarts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shopingCart = await _context.ShopingCart
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (shopingCart == null)
            {
                return NotFound();
            }

            return View(shopingCart);
        }

        // GET: ShopingCarts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ShopingCarts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserID,OrderAt,Action,Address,Note")] ShopingCart shopingCart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shopingCart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(shopingCart);
        }

        // GET: ShopingCarts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shopingCart = await _context.ShopingCart.FindAsync(id);
            if (shopingCart == null)
            {
                return NotFound();
            }
            return View(shopingCart);
        }

        // POST: ShopingCarts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserID,OrderAt,Action,Address,Note")] ShopingCart shopingCart)
        {
            if (id != shopingCart.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shopingCart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShopingCartExists(shopingCart.OrderId))
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
            return View(shopingCart);
        }

        // GET: ShopingCarts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shopingCart = await _context.ShopingCart
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (shopingCart == null)
            {
                return NotFound();
            }

            return View(shopingCart);
        }

        // POST: ShopingCarts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shopingCart = await _context.ShopingCart.FindAsync(id);
            if (shopingCart != null)
            {
                _context.ShopingCart.Remove(shopingCart);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShopingCartExists(int id)
        {
            return _context.ShopingCart.Any(e => e.OrderId == id);
        }
    }
}
