using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using FileIO = System.IO.File;


namespace BankApp.Pages.User
{
    public class SuspiciousTransactionsModel : PageModel
    {

        public List<List<string>> FileContents { get; set; }

        public void OnGet()
        {
            string directoryPath = "../MoneyLaunderingGuard/SuspiciousTransactions";
            int linesToRead = 60;

            FileContents = ReadLastLinesFromTextFiles(directoryPath, linesToRead);
        }

        private List<List<string>> ReadLastLinesFromTextFiles(string directoryPath, int linesToRead)
        {
            List<List<string>> fileContents = new List<List<string>>();

            string[] textFiles = Directory.GetFiles(directoryPath, "*.txt");

            foreach (string filePath in textFiles)
            {
                List<string> lastLines = new List<string>();

                try
                { 
                    string countryName = Path.GetFileNameWithoutExtension(filePath);
                    lastLines.Add($"Country: {countryName}");

                    string[] allLines = FileIO.ReadAllLines(filePath);

                    int startIndex = Math.Max(0, allLines.Length - linesToRead);
                    int endIndex = allLines.Length;

                    for (int i = endIndex - 1; i >= startIndex; i--)
                    {
                        lastLines.Add(allLines[i]);
                    }
                }
                catch (IOException ex)
                {

                }

                fileContents.Add(lastLines);
            }

            return fileContents;
        }



        public IActionResult OnPostStartConsoleApp()
        {
            string consoleAppPath = "../MoneyLaunderingGuard/bin/Debug/net8.0/MoneyLaunderingGuard";

            Process.Start(consoleAppPath);

            return RedirectToPage("SuspiciousTransactions");
        }
    }
}
