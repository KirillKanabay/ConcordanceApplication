using System.Collections.Generic;
using System.Linq;
using Concordance.Constants;
using Concordance.Infrastructure.Logger;
using Concordance.Model;
using Concordance.Model.TextElements;

namespace Concordance.Services.Concordance
{
    public class ConcordanceReportService : IConcordanceReportService
    {
        private readonly ILogger _logger;
        public ConcordanceReportService(ILogger logger)
        {
            _logger = logger;
        }

        public ServiceResult<ConcordanceReport> Create(Text text)
        {
            _logger.Information(LogConstants.StartCreatingReport);

            if (text == null)
            {
                _logger.Warning(LogConstants.TextForReportingIsNull);
                return new ServiceResult<ConcordanceReport>()
                {
                    IsSuccess = false,
                    Error = LogConstants.TextForReportingIsNull,
                };
            }

            var reportList = new SortedList<Word, ConcordanceReportItem>();

            foreach (var page in text.Pages)
            {
                var words = page.Sentences
                    .SelectMany(p => p.SentenceElements.Where(se => se is Word))
                    .Select(se => se as Word);

                foreach (var word in words)
                {
                    if (!reportList.ContainsKey(word))
                    {
                        reportList.Add(word, new ConcordanceReportItem()
                        {
                            PageNumbers = new List<int>(),
                            Word = word
                        });
                    }

                    AddPageToConcordanceReportItem(reportList[word], page.Number);
                }
            }

            var report = new ConcordanceReport()
            {
                TextName = text.Name,
                Items = reportList.Values
            };

            _logger.Information(LogConstants.CreatedReport);

            return new ServiceResult<ConcordanceReport>()
            {
                IsSuccess = true,
                Data = report,
            };
        }

        private void AddPageToConcordanceReportItem(ConcordanceReportItem item, int pageNumber)
        {
            if (item.PageNumbers.FirstOrDefault(p => p == pageNumber) == default)
            {
                item.PageNumbers.Add(pageNumber);
            }

            item.Count++;
        }
    }
}