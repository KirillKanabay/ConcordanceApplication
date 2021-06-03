using Concordance.Model;

namespace Concordance.Interfaces
{
    public interface ITextReader
    {
        /// <summary>
        /// Метод читающий текст
        /// </summary>
        /// <returns></returns>
        Text Read();
    }
}
