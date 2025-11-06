public class Customer
{
    public int CustomerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"{CustomerId}: {Name} ({Email}, {PhoneNumber})";
    }
}