using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Concordance.Helpers;
using Concordance.Model;
using Concordance.Services;

namespace Concordance.View
{
    public class ConcordanceView
    {
        private readonly IReportWriterService _reportWriter;
        private readonly ITextReaderService _textReader;
        private List<Text> _textList;
        public ConcordanceView(ITextReaderService textReader, IReportWriterService reportWriterService)
        {
            _textReader = textReader;
            _reportWriter = reportWriterService;
        }

        public void Show()
        {
            do
            {
                LoadTextList();
                HandleTextList();
                Console.WriteLine();
            } while (true);

        }

        private void LoadTextList()
        {
            ConsoleExtensions.WriteLineWithColor("!Пути к обрабатываемым файлам находятся в Config.json, убедитесь в правильности заполнения!", ConsoleColor.Red);
            Console.WriteLine();
            
            if (ConsoleExtensions.CheckContinue("Желаете продолжить? (y/n):"))
            {
                Console.WriteLine();
                _textList = _textReader.GetTextList();
            }
            else
            {
                Environment.Exit(0);
            }
        }

        private void HandleTextList()
        {
            ConsoleExtensions.WriteLineWithColor($"Загружено {_textList.Count} файлов.", ConsoleColor.Green);
            Console.WriteLine();

            if (ConsoleExtensions.CheckContinue("Начать обработку? (y/n):"))
            {
                foreach (var text in _textList)
                {
                    var report = new ReportService(text);
                    report.MakeReport();
                    try
                    {
                        _reportWriter.Write(report);
                        ConsoleExtensions.WriteLineWithColor($"{text.Name} - обработан.", ConsoleColor.Green);
                    }
                    catch (Exception e)
                    {
                        ConsoleExtensions.WriteLineError($"{text.Name} - ошибка обработки. Ошибка: {e.Message}");
                    }
                }

                Console.WriteLine();
                ConsoleExtensions.WriteLineWithColor("Файлы обработаны.", ConsoleColor.Green);
            }
            else
            {
                Environment.Exit(0);
            }
        }
    }
}
