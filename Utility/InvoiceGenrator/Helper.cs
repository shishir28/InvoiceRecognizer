using Spectre.Console;
using System.Reflection;

namespace InvoiceGenrator
{
    public class Helper
    {
        public bool answerInvoice { get; set; }
        public string selectedFormat { get; set; }
        public string CwdPath { get; set; }
        public string TemplatePath { get; set; }

        public Helper()
        {
            CwdPath = Directory.GetCurrentDirectory();
            string toolRoot = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string baseToolPath = new Uri(toolRoot + "/../../../").LocalPath;
            TemplatePath = Path.Combine(baseToolPath, "PDF-Templates");
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

        public static void CreateFile(string path, string templatePath, string fileName)
        {
            var dest = Path.Combine(path, fileName);
            var src = Path.Combine(templatePath, fileName);
            AnsiConsole.MarkupLine(string.Format("LOG: Creating {0} ...", fileName));
            if (File.Exists(dest))
            {
                Warning(string.Format("IGNORE: File already exist"));
            }
            else
            {
                File.Copy(src, dest);
                Success(string.Format("CREATED: File '{0}' created", fileName));
            }
        }

        private void ProcessUserInput()
        {
            AnsiConsole.Status()
            .Start("Generating Invoice...", ctx =>
            {
                if (answerInvoice)
                {

                    CreateFile(CwdPath, TemplatePath, "README.md");
                    Thread.Sleep(1000);
                    ctx.Status("Next task");
                    ctx.Spinner(Spinner.Known.Star);
                    ctx.SpinnerStyle(Style.Parse("green"));
                }
            });
            AnsiConsole.MarkupLine("All Good now. Press any key to continue....");
        }

        private static void WriteHeader(string header) => AnsiConsole.Write(
            new FigletText(header)
                .Color(Color.Red));

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
