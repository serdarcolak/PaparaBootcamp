using DigitalSalesPlatform.Base;

namespace DigitalSalesPlatform.Schema;

public class UserRequest : BaseRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public decimal WalletBalance { get; set; }
    public decimal PointsBalance { get; set; }
}