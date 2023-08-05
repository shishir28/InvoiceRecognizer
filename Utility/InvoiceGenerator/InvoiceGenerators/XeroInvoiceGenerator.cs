using iTextSharp.text.pdf;

namespace InvoiceGenerator.InvoiceGenerators
{
    public class XeroInvoiceGenerator
    {
        public static void GenerateDocuments(IList<InvoiceDTO> invoices, string toolPath)
        {
            for (int i = 0; i < invoices.Count; i++)
                GenerateDocument(invoices[i], i + 1, toolPath);
        }

        private static void GenerateDocument(InvoiceDTO invoice, int indexCount, string toolPath)
        {
            var templatePath = Path.Combine(toolPath, "PDF-Templates");
            var sourcePdfFileName = Path.Combine(templatePath, "xero-invoice-template-au.pdf");
            var outputPdfFileName = Path.Combine(toolPath, "GeneratedInvoices",$"xero-invoice-{indexCount}.pdf");
            using var templateStream = new FileStream(sourcePdfFileName, FileMode.Open, FileAccess.Read);
            using var outputStream = new FileStream(outputPdfFileName, FileMode.Create, FileAccess.Write);
            var pdfReader = new PdfReader(templateStream);
            var pdfStamper = new PdfStamper(pdfReader, outputStream);
            var formFields = pdfStamper.AcroFields;
            formFields.SetField("Your Company Name", invoice.CompanyName);
            formFields.SetField("Street Number and Name", string.Empty);
            formFields.SetField("State & Post Code", string.Empty);
            formFields.SetField("Country", string.Empty);
            formFields.SetField("Additional notes", string.Empty);
            formFields.SetField("Client's Company Name", invoice.ClientName);
            formFields.SetField("Attention: Client’s Name", invoice.ClientCustomerName);
            formFields.SetField("Client's Street Number & Name or PO Box", invoice.StreetAddress);
            formFields.SetField("Client's State & Post Code", invoice.PostCode.ToString());
            formFields.SetField("Client's Country", string.Empty);
            formFields.SetField("Invoice Date", invoice.InvoiceDate.ToString("yyyy-MM-dd"));
            formFields.SetField("Invoice Number", invoice.InvoiceNumber);
            formFields.SetField("Date Due", invoice.DueDate.ToString("yyyy-MM-dd"));

            decimal total = 0.0m;
            for (int i = 1; i <= 12; i++)
            {
                var quantity = (invoice.Quantity + i);
                var price = (invoice.Price + i);

                formFields.SetField($"Item {i}: Description", $"{invoice.ItemName}...{i}");
                formFields.SetField($"Item {i}: Quantity", quantity.ToString());
                formFields.SetField($"Item {i}: Unit Price", price.ToString());
                var amount = quantity * price;
                formFields.SetField($"Item {i}: Amount", amount.ToString());
                total += amount;
            }

            formFields.SetField($"Total", total.ToString());
            pdfStamper.FormFlattening = true;
            pdfStamper.Close();
            pdfReader.Close();
        }
    }
}