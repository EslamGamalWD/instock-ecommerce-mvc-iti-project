namespace InStockWebAppDAL.Entities;

public class City
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int StateId { get; set; }
    public virtual State State { get; set; } = default!;
    public IEnumerable<User> Users { get; set; } = default!;
}