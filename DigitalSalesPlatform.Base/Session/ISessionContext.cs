using Microsoft.AspNetCore.Http;

namespace DigitalSalesPlatform.Base;

public interface ISessionContext
{
    public HttpContext HttpContext { get; set; }
    public Session Session { get; set; }
}