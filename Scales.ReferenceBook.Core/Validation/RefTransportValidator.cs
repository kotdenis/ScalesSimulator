using FluentValidation;
using SharedLibrary.DTO.ReferenceBook;

namespace Scales.ReferenceBook.Core.Validation
{
    public class RefTransportValidator : AbstractValidator<RefTransportDto>
    {
        public RefTransportValidator()
        {
            RuleFor(x => x.NumberOfAxles)
                .GreaterThanOrEqualTo(2)
                .LessThanOrEqualTo(5);
            RuleFor(x => x.Brand)
                .NotEmpty();
            RuleFor(x => x.CarPlate)
                .NotEmpty();
        }
    }
}
