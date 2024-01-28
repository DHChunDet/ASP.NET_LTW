using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DoAnLTW.Models
{
	public class ShopingCart
	{
		public Product Product { get; set; }
		[Key]
		public int OrderId { get; set; }
		public int UserID { get; set; }
		public int Quantity { get; set; }
		public DateTime OrderAt { get; set; }
		public bool Action { get; set; }
		public string Address { get; set; }
		public string Note { get; set; }
	}
	public class ApplicationDbContext : DbContext
	{
		public DbSet<ShopingCart> ShopingCarts { get; set; }
	}
}
