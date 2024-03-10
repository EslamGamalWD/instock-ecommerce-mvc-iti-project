

namespace InStockWebAppBLL.Models.ProductVM
{
    public record GetProductsVM(
    int Id,
    string Name,
    string Description,
    decimal Price,
    int InStock,
    string ImagePath,
    DateTime CreatedAt,
    string SubCategoryName,
    string DiscountName
)
        {
            public GetProductsVM()
                : this(0, "","", 0, 0, "", DateTime.MinValue, "", "") { }
        }

}
