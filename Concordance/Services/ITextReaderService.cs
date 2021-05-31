using Concordance.Model;
using System.Collections.Generic;

namespace Concordance.Services
{
    public interface ITextReaderService
    {
        /// <summary>
        /// Метод возвращающий список текстов
        /// </summary>
        /// <returns></returns>
        List<Text> GetTextList();
    }
}
