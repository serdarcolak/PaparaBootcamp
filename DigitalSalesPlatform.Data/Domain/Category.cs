using System.ComponentModel.DataAnnotations.Schema;
using DigitalSalesPlatform.Base.Entity;

namespace DigitalSalesPlatform.Data;

[Table("Category", Schema = "dbo")]
public class Category : BaseEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Url { get; set; }
    public string Tag { get; set; }
    
    public ICollection<ProductCategory> ProductCategories { get; set; }
}