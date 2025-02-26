﻿using System;
using Concordance.Helpers;
using Concordance.Model;
using Concordance.Model.Options;
using Concordance.Model.TextElements;
using Concordance.Services.Concordance;
using Concordance.Services.Concordance.Writer;
using Concordance.Services.Configurations;
using Concordance.Services.Parser;

namespace Concordance
{
    public class AppHost
    {

        private readonly IConfigurationParserService _configParser;
        private readonly ITextParserService _textParser;
        private readonly IConcordanceReportService _concordanceReportService;
        private readonly IConcordanceWriterService _concordanceWriter;

        public AppHost(IConfigurationParserService textInfoParser,
            ITextParserService textParser,
            IConcordanceReportService concordanceReportService,
            IConcordanceWriterService concordanceWriter)
        {
            _configParser = textInfoParser;
            _textParser = textParser;
            _concordanceReportService = concordanceReportService;
            _concordanceWriter = concordanceWriter;
        }

        public void Run()
        {
            var textOptions = _configParser.GetTextOptions();
            if (textOptions == null)
            {
                return;
            }

            var parserTextResult = _textParser.Parse(textOptions);
            if (!parserTextResult.IsSuccess)
            {
                return;
            }

            var reportResult = _concordanceReportService.Create(parserTextResult.Data);
            if (!reportResult.IsSuccess)
            {
                return;
            }
            _concordanceWriter.Write(reportResult.Data);

        }
    }
}
