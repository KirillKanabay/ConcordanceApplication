using System.IO;
using Concordance.Constants;
using Concordance.Model.Options;
using FluentValidation;

namespace Concordance.Validation
{
    public class TextOptionsValidator : AbstractValidator<TextOptions>
    {
        public TextOptionsValidator()
        {
            RuleFor(x => x.Path)
                .Custom((path, context) =>
            {
                FileInfo fileInfo = new FileInfo(path);
                if (!fileInfo.Exists)
                {
                    context.AddFailure(ErrorConstants.TextPathNotExists);
                }
            });

            RuleFor(x => x.PageSize)
                .Must(pageSize => pageSize > 0)
                .WithMessage(ErrorConstants.PageSizeLessThanZero);
        }
    }
}
