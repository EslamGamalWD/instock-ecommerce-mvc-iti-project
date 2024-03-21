using System.ComponentModel.DataAnnotations.Schema;
using InStockWebAppDAL.Entities.Enumerators;

namespace InStockWebAppDAL.Entities;

public class PaymentDetails
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public string Provider { get; set; } = string.Empty;
    public PaymentStatus PaymentStatus { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public bool IsDeleted { get; set; }
   
}