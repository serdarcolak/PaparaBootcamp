using System.ComponentModel.DataAnnotations.Schema;
using DigitalSalesPlatform.Base.Entity;

namespace DigitalSalesPlatform.Data;

[Table("Product", Schema = "dbo")]
public class Product : BaseEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public bool IsActive { get; set; }
    public int Stock { get; set; }
    public decimal PointsPercentage { get; set; }
    public decimal MaxPoints { get; set; }
    
    public ICollection<ProductCategory> ProductCategories { get; set; }
    
}