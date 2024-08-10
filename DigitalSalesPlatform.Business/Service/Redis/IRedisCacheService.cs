namespace DigitalSalesPlatform.Business.Service;

public interface IRedisCacheService
{
    Task<bool> SetValueAsync(string key, string value);
    Task Clear(string key);
}