using System.ComponentModel.DataAnnotations.Schema;
using DigitalSalesPlatform.Base.Entity;

namespace DigitalSalesPlatform.Data;

[Table("OrderDetail", Schema = "dbo")]
public class OrderDetail : BaseEntity
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}