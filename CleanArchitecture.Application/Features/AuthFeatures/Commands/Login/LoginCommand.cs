using MediatR;

namespace CleanArchitecture.Application.Features.AuthFeatures.Commands.Login;

public sealed record LoginCommand(
    string UserNameorEmail,
    string Password ) : IRequest<LoginCommandResponse>;



