using System;
using Concordance.Concordance;
using Concordance.Configurations;
using Concordance.Helpers;
using Concordance.Model.Options;
using Concordance.Parser;

namespace Concordance.View
{
    public class ConcordanceView
    {
        private readonly IConfigurationParser _configParser;
        private readonly ITextParser _textParser;
        public ConcordanceView(IConfigurationParser textInfoParser)
        {
            _configParser = textInfoParser;
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


            HandleText(textOptions, outputDir);
        }

        private void HandleText(TextOptions options, string outputDir)
        {

            if (ConsoleExtensions.CheckContinue("Начать обработку? (y/n):"))
            {
                var parsedText = _textParser.Parse(options); 
                Console.WriteLine();
                ConsoleExtensions.WriteLineWithColor("Текст распаршен. Приступается его обработка", ConsoleColor.Green);
            }
            else
            {
                Environment.Exit(0);
            }
        }

        private TextOptions GetTextOptions()
        {
            ConsoleExtensions.WriteLineWithColor(
                "Настройки обрабатываемого текста находятся в Config.json, убедитесь в правильности заполнения!",
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
            try
            {
                var outputDirectory = _configParser.GetOutputDirectory();
            }
            catch (Exception e)
            {
                ConsoleExtensions.WriteLineError($"Не удалось извлечь путь директории с результатами обработки: {e.Message}");
                Environment.Exit(0);
            }

            return null;
        }
    }
}