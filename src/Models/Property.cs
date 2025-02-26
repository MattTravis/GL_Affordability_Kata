namespace AffordabilityServiceCore.Models;

public class Property
{
    public Property(long id, string address, decimal monthlyRent)
    {
        Id = id;
        Address = address;
        MonthlyRent = monthlyRent;
    }

    public Property()
    {
    }

    public long Id { get; set; }
    public string Address { get; set; }
    public decimal MonthlyRent { get; set; }
}