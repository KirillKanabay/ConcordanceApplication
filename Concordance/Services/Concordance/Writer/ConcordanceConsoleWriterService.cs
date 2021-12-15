using System;
using Concordance.Model;

namespace Concordance.Services.Concordance.Writer
{
    public class ConcordanceConsoleWriterService : IConcordanceWriter
    {
        public void Write(ConcordanceReport report)
        {

            char prevFirstChar = ' ';

            foreach (var item in report.Items)
            {
                if (item.FirstChar != prevFirstChar)
                {
                    if (prevFirstChar != ' ')
                    {
                        Console.WriteLine();
                    }

                    prevFirstChar = item.FirstChar;
                    Console.WriteLine(item.FirstChar.ToString());
                }

                Console.WriteLine(item.ToString());
            }
        }
    }
}
