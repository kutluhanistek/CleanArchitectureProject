using FluentValidation;

namespace CleanArchitecture.Application.Features.CarFeatures.Commands.CreateCar;

public sealed class CreateCarCommandValidation : AbstractValidator<CreateCarCommand>
{
    public CreateCarCommandValidation()
    {
        RuleFor(p => p.Name).NotEmpty().WithMessage("Araç adı boş olamaz!!");
        RuleFor(p => p.Name).NotNull().WithMessage("Araç adı boş olamaz!!");
        RuleFor(p => p.Name).MinimumLength(3).WithMessage("Araç adı en az 3 karakterden oluşmalıdır!!");

        RuleFor(p => p.Model).NotEmpty().WithMessage("Araç modeli boş olamaz!!");
        RuleFor(p => p.Model).NotNull().WithMessage("Araç modeli boş olamaz!!");
        RuleFor(p => p.Model).MinimumLength(2).WithMessage("Araç modeli en az 2 karakterden oluşmalıdır!!");
        
        RuleFor(p => p.EnginePower).NotEmpty().WithMessage("Motor gücü boş olamaz!!");
        RuleFor(p => p.EnginePower).NotNull().WithMessage("Motor gücü boş olamaz!!");
        RuleFor(p => p.EnginePower).GreaterThan(0).WithMessage("Motor gücü 0'dan büyük olmalıdır!!");
    }
}
