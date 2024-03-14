

namespace InStockWebAppBLL.Models.ProductVM
{
    public record GetProductsVM(
    int Id,
    string Name,
    string Description,
    decimal Price,
    int InStock,
    List<string> ImagePaths,
    DateTime CreatedAt,
    bool IsDeleted,
    string SubCategoryName,
    string DiscountName
)
        {
            public GetProductsVM()
                : this(0, "","", 0, 0, new List<string> { }, DateTime.MinValue,false, "", "") { }
        }

}
