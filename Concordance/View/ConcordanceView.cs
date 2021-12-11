using System;
using System.Collections.Generic;
using System.Linq;
using Concordance.Helpers;
using Concordance.Interfaces;
using Concordance.IO;
using Concordance.Model;
using Concordance.Report;

namespace Concordance.View
{
    public class ConcordanceView
    {
        private readonly IConfigurationParser _textInfoParser;

        private ICollection<Text> _textList;
        private int _pageSize;
        private string _outputDirectory;

        public ConcordanceView(IConfigurationParser textInfoParser)
        {
            _textInfoParser = textInfoParser;
        }

        public void Show()
        {
            LoadTextList();
            HandleTextList();
        }

        private void LoadTextList()
        {
            ConsoleExtensions.WriteLineWithColor(
                "!Пути к обрабатываемым файлам находятся в Config.json, убедитесь в правильности заполнения!",
                ConsoleColor.Red);
            Console.WriteLine();

            if (ConsoleExtensions.CheckContinue("Желаете продолжить? (y/n):"))
            {
                IEnumerable<string> filePaths = GetFilePaths();
                GetPageSize();
                _textList = new List<Text>();
                foreach (var filePath in filePaths)
                {
                    try
                    {
                        var reader = new TextFileReader(filePath, _pageSize);
                        var text = reader.Read();
                        _textList.Add(text);
                        ConsoleExtensions.WriteLineWithColor($"Текст {text.Name} добавлен в обработку.",
                            ConsoleColor.Green);
                    }
                    catch
                    {
                        ConsoleExtensions.WriteLineError($"Не удалось открыть файл: {filePath}");
                    }
                }
            }
            else
            {
                Environment.Exit(0);
            }
        }

        private void HandleTextList()
        {
            ConsoleExtensions.WriteLineWithColor($"Загружено {_textList.Count()} файлов.", ConsoleColor.Green);
            Console.WriteLine();

            if (ConsoleExtensions.CheckContinue("Начать обработку? (y/n):"))
            {
                GetOutputDirectory();
                foreach (var text in _textList)
                {
                    var report = new ConcordanceReport(text);
                    report.MakeReport();
                    try
                    {
                        var concordanceReportFileWriter = new ConcordanceReportFileWriter(_outputDirectory);
                        concordanceReportFileWriter.Write(report);
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

        private IEnumerable<string> GetFilePaths()
        {
            try
            {
                return _textInfoParser.GetInputFilePaths();
            }
            catch (Exception e)
            {
                ConsoleExtensions.WriteLineError($"Не удалось извлечь пути к входным файлам: {e.Message}");
                Environment.Exit(0);
            }

            return null;
        }

        private void GetPageSize()
        {
            try
            {
                _pageSize = _textInfoParser.GetPageSize();
            }
            catch (Exception e)
            {
                ConsoleExtensions.WriteLineError($"Не удалось извлечь размер страницы: {e.Message}");
                Environment.Exit(0);
            }
        }

        private void GetOutputDirectory()
        {
            try
            {
                _outputDirectory = _textInfoParser.GetOutputDirectory();
            }
            catch (Exception e)
            {
                ConsoleExtensions.WriteLineError($"Не удалось извлечь путь директории с результатами обработки: {e.Message}");
                Environment.Exit(0);
            }
        }
    }
}