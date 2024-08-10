using DigitalSalesPlatform.Base;

namespace DigitalSalesPlatform.Schema;

public class CategoryRequest: BaseRequest
{
    public string Name { get; set; }
    public string Url { get; set; }
    public string Tag { get; set; }
}