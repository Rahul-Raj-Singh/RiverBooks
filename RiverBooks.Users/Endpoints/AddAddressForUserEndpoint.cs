using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Result;
using FastEndpoints;
using FluentValidation;
using MediatR;
using RiverBooks.Users.UseCases;

namespace RiverBooks.Users.Endpoints;

internal class AddAddressForUserEndpoint(IMediator mediator) : Endpoint<UserAddressDto>
{
    public override void Configure()
    {
        Post("/user/addresses");
        Claims("Email");
    }

    public override async Task HandleAsync(UserAddressDto req, CancellationToken ct)
    {
        var email = User.FindFirstValue("Email");

        var command = new AddAddressForUserCommand(
            email, req.Street1, req.Street2, req.City, req.State, req.Country, req.PostalCode);
        
        var result = await mediator.Send(command, ct);

        if (result.Status == ResultStatus.Unauthorized)
            await SendUnauthorizedAsync(ct);
        else
            await SendOkAsync(ct);
    }
}

internal record UserAddressDto(
    string Street1,
    string Street2,
    string City,
    string State,
    string Country,
    string PostalCode);

internal class UserAddressDtoValidator : Validator<UserAddressDto>
{
    public UserAddressDtoValidator()
    {
        RuleFor(x => x.Street1).NotEmpty();
        RuleFor(x => x.City).NotEmpty();
        RuleFor(x => x.State).NotEmpty();
        RuleFor(x => x.Country).NotEmpty();
        RuleFor(x => x.PostalCode).NotEmpty();
    }
}


