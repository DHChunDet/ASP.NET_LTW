using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DoAnLTW.Models
{
	public class ShopingCartItem
	{
		public int Id { get; set; }
		public String? Name { get; set; }
		public String? sdt { get; set; }
		public DateTime Datetime { get; set; }
		public int DonGia { get; set; }

		public int Quantity { get; set; }
		[ForeignKey("Product")]
		public int ProductId { get; set; }
		public virtual Product? Product { get; set; }
	}
}
