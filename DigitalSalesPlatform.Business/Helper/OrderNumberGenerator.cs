namespace DigitalSalesPlatform.Business.Helper;

public class OrderNumberGenerator
{
    private static readonly Random _random = new Random();

    public static int GenerateOrderNumber()
    {
        return _random.Next(100000000, 999999999);
    }
}