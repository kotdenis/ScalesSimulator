using FluentValidation;
using SharedLibrary.DTO.Journal;

namespace Scales.Journal.Core.Validation
{
    public class TransportValidator : AbstractValidator<TransportDto>
    {
        public TransportValidator()
        {
            RuleFor(x => x.Weight)
                .GreaterThan(5000);
            RuleFor(x => x.AxlesDtos.Count)
                .GreaterThanOrEqualTo(2);
            RuleFor(x => x.Cargo)
                .NotEmpty();
            RuleFor(x => x.Brand)
                .NotEmpty();
            RuleFor(x => x.CarPlate)
                .NotEmpty();
        }
    }
}
