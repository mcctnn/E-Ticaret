namespace Core.Entities.OrderAggregate;

public class PaymentSummary
{
    public int Last4 { get; set; } //kredi kartı son 4
    public required string Brand { get; set; }//markası
    public int ExpMonth { get; set; }//son kullanma tarihi ay
    public int ExpYear { get; set; }//son kullanma tarihi yıl
}