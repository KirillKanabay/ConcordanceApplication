using System;
using Concordance.Helpers;
using Concordance.Model;
using Concordance.Model.Options;
using Concordance.Model.TextElements;
using Concordance.Services.Concordance;
using Concordance.Services.Concordance.Writer;
using Concordance.Services.Configurations;
using Concordance.Services.Parser;

namespace Concordance.View
{
    public class ConcordanceView : IView
    {
        private readonly IConfigurationParserService _configParser;
        private readonly ITextParserService _textParser;
        private readonly IConcordanceReportService _concordanceReportService;
        private readonly IConcordanceWriter _concordanceWriter;

        public ConcordanceView(IConfigurationParserService textInfoParser, 
            ITextParserService textParser,
            IConcordanceReportService concordanceReportService,
            IConcordanceWriter concordanceWriter)
        {
            _configParser = textInfoParser;
            _textParser = textParser;
            _concordanceReportService = concordanceReportService;
            _concordanceWriter = concordanceWriter;
        }

        public void Show()
        {
            var textOptions = GetTextOptions();
            if (textOptions == null)
            {
                return;
            }
            
            var text = ParseText(textOptions);
            if (text == null)
            {
                return;
            }

            var report = CreateConcordanceReport(text);
            WriteConcordance(report);
        }

        private void WriteConcordance(ConcordanceReport report)
        {
            ConsoleExtensions.WriteLineWithColor("Вывод отчета...", ConsoleColor.Green);
            _concordanceWriter.Write(report);
        }

        private ConcordanceReport CreateConcordanceReport(Text text)
        {
            ConsoleExtensions.WriteLineWithColor("Создание конкорданс отчета...", ConsoleColor.Green);
            var report = _concordanceReportService.Create(text);
            ConsoleExtensions.WriteLineWithColor("Отчет выполнен", ConsoleColor.Green);
            
            return report;
        }

        private Text ParseText(TextOptions options)
        {
            var parserResult = _textParser.Parse(options);

            if (parserResult.IsSuccess)
            {
                ConsoleExtensions.WriteLineWithColor("Текст распаршен.", ConsoleColor.Green);
            }
            else
            {
                ConsoleExtensions.WriteLineError(parserResult.Error);
            }

            Console.WriteLine();

            return parserResult.Text;
        }

        private TextOptions GetTextOptions()
        {
            ConsoleExtensions.WriteLineWithColor(
                "Настройки обрабатываемого текста находятся в appsettings.json, убедитесь в правильности заполнения!",
                ConsoleColor.DarkYellow);

            Console.WriteLine();

            TextOptions options = null;

            if (ConsoleExtensions.CheckContinue("Желаете продолжить? (y/n):"))
            {
                try
                {
                    options = _configParser.GetTextOptions();
                    ConsoleExtensions.WriteLineWithColor("Настройки текста загружены", ConsoleColor.Green);
                }
                catch
                {
                    ConsoleExtensions.WriteLineError("Не удалось прочитать файл конфигурации");
                }
            }

            return options;
        }
    }
}