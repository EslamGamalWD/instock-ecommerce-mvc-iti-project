using System.ComponentModel;

public class GetAllDiscountsVM
{
    public int Id { get; set; }

    [DisplayName("Discount Name")]
    public string Name { get; set; }

    [DisplayName("Percentage")]
    public int Percentage { get; set; }

    [DisplayName("Active")]
    public bool IsActive { get; set; }

    [DisplayName("Creation Date")]
    public DateTime CreatedAt { get; set; }

    [DisplayName("Modification Date")]
    public DateTime? ModifiedAt { get; set; }

    public bool IsDeleted { get; set; } = false;
}
