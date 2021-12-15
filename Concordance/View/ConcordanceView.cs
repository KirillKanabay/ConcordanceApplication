using System;
using Concordance.Configurations;
using Concordance.Helpers;
using Concordance.Model;
using Concordance.Model.Options;
using Concordance.Parser;

namespace Concordance.View
{
    public class ConcordanceView : IView
    {
        private readonly IConfigurationParser _configParser;
        private readonly ITextParser _textParser;
        public ConcordanceView(IConfigurationParser textInfoParser, ITextParser textParser)
        {
            _configParser = textInfoParser;
            _textParser = textParser;
        }

        public void Show()
        {
            var textOptions = GetTextOptions();
            if (textOptions == null)
            {
                return;
            }

            var outputDir = GetOutputDirectory();
            if (outputDir == null)
            {
                return;
            }

            
            var text = ParseText(textOptions);

            if (text == null)
            {
                return;
            }
        }

        private Text ParseText(TextOptions options)
        {
            var parserResult = _textParser.Parse(options);

            if (parserResult.IsSuccess)
            {
                ConsoleExtensions.WriteLineWithColor("Текст распаршен. Приступается его обработка", ConsoleColor.Green);
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
                ConsoleColor.Red);

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

        private string GetOutputDirectory()
        {
            string outputDir = null;
            try
            {
                outputDir = _configParser.GetOutputDirectory();
                ConsoleExtensions.WriteLineWithColor($"Путь директории для результатов обработки извлечен", ConsoleColor.Green);
            }
            catch (Exception e)
            {
                ConsoleExtensions.WriteLineError($"Не удалось извлечь путь директории с результатами обработки: {e.Message}");
                Environment.Exit(0);
            }

            return outputDir;
        }
    }
}