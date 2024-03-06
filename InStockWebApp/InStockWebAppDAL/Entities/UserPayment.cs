using System.ComponentModel.DataAnnotations;
using InStockWebAppDAL.Entities.Enumerators;

namespace InStockWebAppDAL.Entities;

public class UserPayment
{
    public int Id { get; set; }

    public PaymentType PaymentType { get; set; }

    public string Provider { get; set; } = string.Empty;

    public string AccountNumber { get; set; } = string.Empty;

    public DateOnly ExpiryDate { get; set; }
    public PaymentStatus Status { get; set; }
    public bool IsDeleted { get; set; } = false;

    //Navigation Property
    public string UserId { get; set; } = string.Empty;
    public virtual User User { get; set; } = default!;
}