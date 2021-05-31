using System;
using Concordance.Model;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Concordance.Helpers;

namespace Concordance.Services
{
    class TextReaderFromConfigService : ITextReaderService
    {
        private readonly IConfiguration _configuration;
        public TextReaderFromConfigService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<Text> GetTextList()
        {
            List<Text> textList = new List<Text>();

            var inputFileSection = _configuration.GetSection("inputFiles");
            foreach(var section in inputFileSection.GetChildren())
            {
                try
                {
                    string path = section["Path"];
                    int pageSize = int.Parse(section["PageSize"]);
                    var text = ReadTextFromFile(path, pageSize);

                    if (text != null)
                    {
                        textList.Add(text);
                        ConsoleExtensions.WriteLineWithColor($"Текст {text.Name} добавлен в обработку.", ConsoleColor.Green);
                    }
                }
                catch(Exception e)
                {
                    ConsoleExtensions.WriteLineError($"Ошибка обработки файла конфигурации: {e.Message}");
                }
                
                
            }

            return textList;
        }

        private Text ReadTextFromFile(string path, int pageSize)
        {
            try
            {
                using var fs = new FileStream(path, FileMode.Open);
                using var reader = new StreamReader(fs);

                Text text = new Text { Name = Path.GetFileNameWithoutExtension(path), PageSize = pageSize };
                while (!reader.EndOfStream)
                {
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < text.PageSize && !reader.EndOfStream; i++)
                    {
                        sb.AppendLine(reader.ReadLine());
                    }
                    text.Pages.Add(sb.ToString());
                }

                return text;
            }
            catch
            {
                ConsoleExtensions.WriteLineError($"Не удалось открыть файл: {path}");
            }

            return null;
        }
    }
}
