using InStockWebAppDAL.Entities;
using System.ComponentModel;

public class GetDiscountByIdVM
{
    public int Id { get; set; }

    [DisplayName("Discount Name")]
    public string Name { get; set; }

    [DisplayName("Description")]
    public string Description { get; set; }

    [DisplayName("Image Path")]
    public string ImagePath { get; set; }

    [DisplayName("Percentage")]
    public int Percentage { get; set; }

    [DisplayName("Is Active")]
    public bool IsActive { get; set; }

    [DisplayName("Created At")]
    public DateTime CreatedAt { get; set; }

    [DisplayName("Modified At")]
    public DateTime? ModifiedAt { get; set; }
    public bool IsDeleted { get; set; }

    [DisplayName("Deleted At")]
    public DateTime DeletedAt { get; set; }
    [DisplayName("Associated Products")]
    public List<Product>? Products { get; set; }
}
