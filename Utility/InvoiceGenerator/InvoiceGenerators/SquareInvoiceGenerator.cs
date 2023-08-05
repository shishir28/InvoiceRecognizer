using iTextSharp.text.pdf;

namespace InvoiceGenerator
{
    public class SquareInvoiceGenerator
    {
        public static void GenerateDocuments(IList<InvoiceDTO> invoices, string toolPath)
        {
            for (int i = 0; i < invoices.Count; i++)
                GenerateDocument(invoices[i], i + 1, toolPath);
        }

        private static void GenerateDocument(InvoiceDTO invoice, int indexCount, string toolPath)
        {
            var templatePath = Path.Combine(toolPath, "PDF-Templates");
            var sourcePdfFileName = Path.Combine(templatePath, "square-invoice-template-au.pdf");
            var outputPdfFileName = Path.Combine(toolPath, "GeneratedInvoices", $"square-invoice-{indexCount}.pdf");
            using var templateStream = new FileStream(sourcePdfFileName, FileMode.Open, FileAccess.Read);
            using var outputStream = new FileStream(outputPdfFileName, FileMode.Create, FileAccess.Write);
            var pdfReader = new PdfReader(templateStream);
            var pdfStamper = new PdfStamper(pdfReader, outputStream);
            var formFields = pdfStamper.AcroFields;
            formFields.SetField("business.name", invoice.CompanyName);
            formFields.SetField("business.address", string.Empty);
            formFields.SetField("business.zipcode", string.Empty);
            formFields.SetField("customer.business", invoice.ClientName);
            formFields.SetField("customer.name", invoice.ClientCustomerName);
            formFields.SetField("customer.address", invoice.StreetAddress);
            formFields.SetField("customer.zip", invoice.PostCode.ToString());
            formFields.SetField("invoice.date", invoice.InvoiceDate.ToString("yyyy-MM-dd"));
            formFields.SetField("invoice.number", invoice.InvoiceNumber);
            formFields.SetField("dueDate", invoice.DueDate.ToString("yyyy-MM-dd"));
            formFields.SetField("customer.phoneNumber", string.Empty);
            formFields.SetField("customer.email", string.Empty);

            decimal total = 0.0m;
            for (int i = 0; i < 3; i++)
            {
                var quantity = (invoice.Quantity + i);
                var price = (invoice.Price + i);

                formFields.SetField($"item.name.{i}", $"{invoice.ItemName}...{i}");
                if(i == 0)
                {
                    formFields.SetField("Quantity", quantity.ToString());
                    formFields.SetField("Price", price.ToString());
                    var amount = (quantity * price);
                    formFields.SetField("Amount", amount.ToString());
                    total += amount;
                } else
                {
                    formFields.SetField($"Quantity 2.{i-1}", quantity.ToString());
                    formFields.SetField($"Price 2.{i-1}", price.ToString());
                    var amount = (quantity * price);
                    formFields.SetField($"Amount 2.{i - 1}", amount.ToString());
                    total += amount;
                }
            }

            formFields.SetField($"Total Due", total.ToString());
            pdfStamper.FormFlattening = true;
            pdfStamper.Close();
            pdfReader.Close();
        }
    }
}