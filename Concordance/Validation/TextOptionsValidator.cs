using System.IO;
using Concordance.Model.Options;
using FluentValidation;

namespace Concordance.Validation
{
    public class TextOptionsValidator : AbstractValidator<TextOptions>
    {
        public TextOptionsValidator()
        {
            RuleFor(x => x.Path).Custom((path, context) =>
            {
                FileInfo fileInfo = new FileInfo(path);
                if (!fileInfo.Exists)
                {
                    context.AddFailure("Text path doesn't exists");
                }
            });
            RuleFor(x => x.PageSize).Must(pageSize => pageSize > 0).WithMessage("Page size must be more 0");
        }
    }
}
