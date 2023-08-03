using OfficeOpenXml;

namespace InvoiceGenerator
{
    
    public static class  ExcelDataReader
    {
        public static IList<InvoiceDTO> ReadInvoicesFromExcel(string fileName)
        {
            ExcelPackage.LicenseContext = LicenseContext.Commercial;

            var invoices = new List<InvoiceDTO>();
            var fileInfo = new FileInfo(fileName);
            using (var package = new ExcelPackage(fileInfo))
            {
                var worksheet = package.Workbook.Worksheets[0];
                var rowCount = worksheet.Dimension.Rows;
                for (int row = 2; row <= rowCount; row++)
                {
                    var invoice = new InvoiceDTO
                    {
                        CompanyName = worksheet.Cells[row, 1].Value?.ToString(),
                        ClientName = worksheet.Cells[row, 2].Value?.ToString(),
                        ClientCustomerName = worksheet.Cells[row, 3].Value?.ToString(),
                        StreetAddress = worksheet.Cells[row, 4].Value?.ToString(),
                        PostCode = Convert.ToInt32(worksheet.Cells[row, 5].Value),
                        InvoiceNumber = worksheet.Cells[row, 6].Value?.ToString(),
                        InvoiceDate = Convert.ToDateTime(worksheet.Cells[row, 7].Value),
                        DueDate = Convert.ToDateTime(worksheet.Cells[row, 8].Value),
                        ItemName = worksheet.Cells[row, 9].Value?.ToString(),
                        Quantity = Convert.ToInt32(worksheet.Cells[row, 10].Value),
                        Price = Convert.ToDecimal(worksheet.Cells[row, 11].Value),
                        Amount = Convert.ToDecimal(worksheet.Cells[row, 12].Value)
                    };

                    invoices.Add(invoice);
                }
            }
            return invoices;
        }
    }
}
