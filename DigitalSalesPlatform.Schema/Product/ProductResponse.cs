using DigitalSalesPlatform.Base;
using DigitalSalesPlatform.Data;

namespace DigitalSalesPlatform.Schema;

public class ProductResponse : BaseResponse
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public bool IsActive { get; set; }
    public int Stock { get; set; }
    public decimal PointsPercentage { get; set; }
    public decimal MaxPoints { get; set; }
    public List<CategoryResponse> Categories { get; set; } 
}