using Spectre.Console;
using System.Reflection;

namespace InvoiceGenerator
{
    public class Helper
    {
        public bool answerInvoice { get; set; }
        public string selectedFormat { get; set; }
        public string CwdPath { get; set; }
        public string TemplatePath { get; set; }
        public string BaseToolPath { get; set; }


        public Helper()
        {
            CwdPath = Directory.GetCurrentDirectory();
            var toolRoot = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            BaseToolPath = new Uri(toolRoot + "/../../../").LocalPath;
            TemplatePath = Path.Combine(BaseToolPath, "PDF-Templates");
        }


        private void ReadUserInput()
        {
            Console.WriteLine("");
            answerInvoice = AnsiConsole.Confirm("Generate a [green] Invoice[/]?");

            selectedFormat = AnsiConsole.Prompt(
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
                if (answerInvoice)
                {
                    if (selectedFormat == "Xero")
                        ProcessXeroData();
                    
                    Thread.Sleep(1000);
                    ctx.Status("Generating Xero Invoice");
                    ctx.Spinner(Spinner.Known.Star);
                    ctx.SpinnerStyle(Style.Parse("green"));
                }
            });
            AnsiConsole.MarkupLine("All Good now. Press any key to continue....");
        }

        private void ProcessXeroData()
        {
            var invoices = ExcelDataReader.ReadInvoicesFromExcel(Path.Combine(BaseToolPath, "Data", "Xero-Data.xlsx"));
            XeroInvoiceGenerator.GenerateDocuments(invoices,TemplatePath);

        }

        private static void WriteHeader(string header) => AnsiConsole.Write(
            new FigletText(header)
                .Color(Color.Green));

        private static void Success(string success) => Write(success, "green");
        private static void Warning(string warning) => Write(warning, "yellow");
        private static void Write(string message, string color) =>
            AnsiConsole.MarkupLine(string.Format("[{0}]{1}[/]", color, message));

        public void RunProgram()
        {
            WriteHeader("Invoice Generator");
            ReadUserInput();
            ProcessUserInput();
            AnsiConsole.Markup("[red]Press any key to exit...[/]");
            Console.Read();
        }

    }
}
