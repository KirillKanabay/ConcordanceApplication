using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Concordance.Model;

namespace Concordance.Services
{
    public class ReportService : IReportService
    {
        public SortedList<char, List<Word>> ReportList { get; }
        public Text Text { get; }

        public ReportService(Text text)
        {
            Text = text;
            ReportList = new SortedList<char, List<Word>>();
        }

        public void MakeReport()
        {
            for (int i = 0; i < Text.Pages.Count; i++)
            {
                HandleWords(Text.Pages[i], i + 1);
            }
        }

        private void HandleWords(string page, int pageNumber)
        {
            //Разделяем страницу на слова
            char[] separators = new[] {',', ' ', '.', '(', ')', ':', ';', '-', '!', '?', '_', '\r', '\n'};
            string[] words = page.Split(separators,
                StringSplitOptions.RemoveEmptyEntries);

            foreach (var word in words)
            {
                char firstLetter = char.ToUpper(word[0]);
                //Если нет буквы в списке, то добавляем букву и слово
                if (!ReportList.ContainsKey(firstLetter))
                    ReportList.Add(firstLetter, new List<Word>() { new Word(word.ToLower(), pageNumber) });
                else
                {
                    //Если в списке есть уже слово, то добавляем страницу и увеличиваем количество встреч слова в тексте
                    if (ReportList[firstLetter].Any(w => w.Value == word.ToLower()))
                    {
                        ReportList[firstLetter]
                            .FirstOrDefault(w => w.Value == word.ToLower()) //Находим слово
                            .AddPage(pageNumber) //Добавляем страницу
                            .Count++; //Увеличиваем количество встреч
                    }
                    else
                    {
                        //Если слова нет, то добавляем новое слово
                        ReportList[firstLetter]
                            .Add(new Word(word.ToLower(), pageNumber));
                        ReportList[firstLetter].Sort();
                    }
                }
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var key in ReportList.Keys)
            {
                sb.AppendLine(char.ToUpper(key).ToString());
                foreach (var word in ReportList[key])
                {
                    sb.AppendLine(word.ToString());
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }
    }
}
