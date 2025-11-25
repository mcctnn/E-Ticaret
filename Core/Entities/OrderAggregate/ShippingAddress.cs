namespace Core.Entities.OrderAggregate;

public class ShippingAddress
{
    public required string Name { get; set; }
    public required string Line1 { get; set; }//Adres satırı1
    public string? Line2 { get; set; }//Adres satırı2
    public required string City { get; set; }//Şehir
    public required string State { get; set; }//İlçe
    public required string PostalCode { get; set; }//Posta kodu 
    public required string Country { get; set; }//ülke
}
