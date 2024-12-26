using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace RiverBooks.Users.Endpoints;

internal class RegisterUserEndpoint(UserManager<ApplicationUser> userManager) : Endpoint<RegisterUserRequest>
{
    public override void Configure()
    {
        Post("/users/register");
        AllowAnonymous();
    }

    public override async Task HandleAsync(RegisterUserRequest req, CancellationToken ct)
    {
        var existingUser = await userManager.FindByEmailAsync(req.Email);

        if (existingUser is not null)
        {
            AddError("Cannot register user. Email already exists!");
            await SendErrorsAsync();
            return;
        }

        await userManager.CreateAsync(new ApplicationUser {UserName = req.Email, Email = req.Email}, req.Password);

        await SendOkAsync();
    }
}

internal class RegisterUserRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}

internal class RegisterUserRequestValidator : Validator<RegisterUserRequest>
{
    public RegisterUserRequestValidator()
    {
        RuleFor(x => x.Email).EmailAddress().NotEmpty();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(5);
    }
}