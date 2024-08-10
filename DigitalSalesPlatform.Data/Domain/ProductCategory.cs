using System.ComponentModel.DataAnnotations.Schema;
using DigitalSalesPlatform.Base.Entity;

namespace DigitalSalesPlatform.Data;

[Table("ProductCategory", Schema = "dbo")]
public class ProductCategory : BaseEntity
{
    public int ProductId { get; set; }
    public Product Product { get; set; }
        
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    
    public string InsertUser { get; set; }
    
}