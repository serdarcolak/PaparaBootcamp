using System.ComponentModel.DataAnnotations.Schema;
using DigitalSalesPlatform.Base.Entity;

namespace DigitalSalesPlatform.Data;

[Table("Order", Schema = "dbo")]
public class Order : BaseEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    
    public int OrderNumber { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal WalletUsed { get; set; }
    public string CouponCode { get; set; }
    public decimal CouponAmount { get; set; }
    public decimal PointsUsed { get; set; }
    
    public decimal PointsEarned { get; set; }
    public DateTime OrderDate { get; set; }
    public ICollection<OrderDetail> OrderDetails { get; set; }
    
}