namespace AffordabilityServiceCore.Models;

public class RawTransaction
{
    public string Date { get; set; }
    public string PaymentType { get; set; }
    public string Details { get; set; }
    public string MoneyOut { get; set; }
    public string MoneyIn { get; set; }
    public string Balance { get; set; } 
}