namespace BankOfDotNet.API.Models
{
    /// <summary>
    /// This is the customer model
    /// </summary>
    public class Customer
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
