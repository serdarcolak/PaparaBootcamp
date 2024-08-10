using DigitalSalesPlatform.Base;

namespace DigitalSalesPlatform.Schema;

public class AuthorizationRequest : BaseRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}