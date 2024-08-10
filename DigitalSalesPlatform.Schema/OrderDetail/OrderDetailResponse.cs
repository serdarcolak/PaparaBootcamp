using DigitalSalesPlatform.Base;

namespace DigitalSalesPlatform.Schema;

public class OrderDetailResponse : BaseResponse
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}