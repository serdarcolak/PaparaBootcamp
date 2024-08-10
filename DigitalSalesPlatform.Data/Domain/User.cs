using System.ComponentModel.DataAnnotations.Schema;
using DigitalSalesPlatform.Base.Entity;

namespace DigitalSalesPlatform.Data;

[Table("User", Schema = "dbo")]
public class User : BaseEntity
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public string Password { get; set; }
    public bool IsActive { get; set; }
    public decimal WalletBalance { get; set; }
    public decimal PointsBalance { get; set; }
    public ICollection<Order> Orders { get; set; }
}