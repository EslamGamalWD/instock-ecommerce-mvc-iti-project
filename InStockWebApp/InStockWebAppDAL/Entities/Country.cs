namespace InStockWebAppDAL.Entities;

public class Country
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public virtual IEnumerable<State> States { get; set; } = default!;
}