namespace InStockWebAppDAL.Entities.Enumerators;

public enum PaymentStatus
{
    UnPaid,
    Pending,
    Completed,
    Failed,
    Declined,
    Canceled,
    Abandoned,
    Settling,
    Settled,
    Refunded
}