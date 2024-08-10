

using DigitalSalesPlatform.Data;

namespace DigitalSalesPlatform.Business.Token;

public interface ITokenService
{
    Task<string> GetToken(User user);
}