using System.Collections.Generic;
using System.Linq;
using Concordance.Constants;
using Concordance.Helpers.Logger;
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
            _logger.Information(InfoConstants.StartCreatingReport);

            if (text == null)
            {
                _logger.Error(ErrorConstants.TextForReportingIsNull);
                return new ServiceResult<ConcordanceReport>()
                {
                    IsSuccess = false,
                    Error = ErrorConstants.TextForReportingIsNull,
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
                        reportList.Add(word, new ConcordanceReportItem(word));
                    }

                    reportList[word].AddPage(page.Number);
                }
            }

            var report = new ConcordanceReport()
            {
                TextName = text.Name,
                Items = reportList.Values
            };

            _logger.Information(InfoConstants.EndCreatingReport);

            return new ServiceResult<ConcordanceReport>()
            {
                IsSuccess = true,
                Data = report,
            };
        }
    }
}