using DigitalSalesPlatform.Base;

namespace DigitalSalesPlatform.Schema;

public class OrderDetailRequest : BaseRequest
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}