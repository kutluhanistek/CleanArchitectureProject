using CleanArchitecture.Application.Services;
using CleanArchitecture.Domain.Dtos;
using MediatR;

namespace CleanArchitecture.Application.Features.CarFeatures.Commands.CreateCar;

public sealed class CreateCarCommandHandler : IRequestHandler<CreateCarCommand, MessageResponse>//request ve response'u verdik içine
{
    private ICarService carService;

    public CreateCarCommandHandler(ICarService carService)
    {
        this.carService = carService;
    }

    public async Task<MessageResponse> Handle(CreateCarCommand request, CancellationToken cancellationToken)
    {
        //işlemler
        await carService.CreateAsync(request, cancellationToken);
        return new("Araç başarıyla kaydedildi!");
    }
}
