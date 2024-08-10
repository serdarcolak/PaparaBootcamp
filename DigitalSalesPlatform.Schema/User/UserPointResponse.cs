using DigitalSalesPlatform.Base;

namespace DigitalSalesPlatform.Schema;

public class UserPointResponse : BaseResponse
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public decimal PointsBalance { get; set; }
}