using System.ComponentModel.DataAnnotations.Schema;
using DigitalSalesPlatform.Base.Entity;

namespace DigitalSalesPlatform.Data;

[Table("Coupon", Schema = "dbo")]
public class Coupon : BaseEntity
{
    public int Id { get; set; }
    public string Code { get; set; }
    public decimal Amount { get; set; }
    public DateTime ExpiryDate { get; set; }
    public bool IsActive { get; set; }
}