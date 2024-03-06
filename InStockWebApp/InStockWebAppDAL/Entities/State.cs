namespace InStockWebAppDAL.Entities;

public class State
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int CountryID { get; set; }
    public virtual IEnumerable<City> Cities { get; set; } = default!;
    public virtual Country Country { get; set; } = default!;
}