namespace InvoiceGenerator
{
    public class InvoiceDTO
    {
        public string CompanyName { get; set; }
        public string ClientName { get; set; }
        public string ClientCustomerName { get; set; }
        public string StreetAddress { get; set; }
        public int PostCode { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
    }
}
