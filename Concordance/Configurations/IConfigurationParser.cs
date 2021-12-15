using System.Collections.Generic;

namespace Concordance.Interfaces
{
    public interface IConfigurationParser
    {
        /// <summary>
        /// Получение путей к входным файлам
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetInputFilePaths();
        /// <summary>
        /// Получение пути к директории с результатами обработки
        /// </summary>
        /// <returns></returns>
        string GetOutputDirectory();
        /// <summary>
        /// Получения размера страницы в строках
        /// </summary>
        /// <returns></returns>
        int GetPageSize();
    }
}
