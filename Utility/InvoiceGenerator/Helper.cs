using InvoiceGenerator.InvoiceGenerators;
using Spectre.Console;
using System.Reflection;

namespace InvoiceGenerator
{
    public class Helper
    {
        public bool AnswerInvoice { get; set; }
        public string SelectedFormat { get; set; }
        public string CwdPath { get; set; }
        public string TemplatePath { get; set; }
        public string BaseToolPath { get; set; }

        public Helper()
        {
            CwdPath = Directory.GetCurrentDirectory();
            var toolRoot = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            BaseToolPath = new Uri(toolRoot + "/../../../").LocalPath;           
        }

        public void RunProgram()
        {
            WriteHeader("Invoice Generator");
            ReadUserInput();
            ProcessUserInput();
            AnsiConsole.Markup("[red]Press any key to exit...[/]");
            Console.Read();
        }

        private void ReadUserInput()
        {
            Console.WriteLine("");
            AnswerInvoice = AnsiConsole.Confirm("Generate a [green] Invoice[/]?");

            SelectedFormat = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Select [green]Invoice template[/] to use")
                .PageSize(10)
                .AddChoices(new[] {
                 "Xero", "Square"
                }));
        }

        private void ProcessUserInput()
        {
            AnsiConsole.Status()
            .Start("Generating Invoice...", ctx =>
            {
                if (AnswerInvoice)
                {
                    if (SelectedFormat == "Xero")
                        ProcessXeroData();

                    if (SelectedFormat == "Square")
                        ProcessSquareData();

                    Thread.Sleep(1000);
                    ctx.Status("Generating Invoice");
                    ctx.Spinner(Spinner.Known.Star);
                    ctx.SpinnerStyle(Style.Parse("green"));
                }
            });
            AnsiConsole.MarkupLine("All Good now. Press any key to continue...");
        }

        private void ProcessXeroData()
        {
            var invoices = ExcelDataReader.ReadInvoicesFromExcel(Path.Combine(BaseToolPath, "Data", "Xero-Data.xlsx"));
            XeroInvoiceGenerator.GenerateDocuments(invoices, BaseToolPath);
        }

        private void ProcessSquareData()
        {
            var invoices = ExcelDataReader.ReadInvoicesFromExcel(Path.Combine(BaseToolPath, "Data", "Square-Data.xlsx"));
            SquareInvoiceGenerator.GenerateDocuments(invoices, BaseToolPath);
        }

        private static void WriteHeader(string header) => AnsiConsole.Write(
            new FigletText(header)
                .Color(Color.Green));
        
        private static void Write(string message, string color) =>
            AnsiConsole.MarkupLine(string.Format("[{0}]{1}[/]", color, message));
    }
}
