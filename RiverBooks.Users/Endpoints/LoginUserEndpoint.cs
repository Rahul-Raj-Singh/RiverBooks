using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using FastEndpoints.Security;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace RiverBooks.Users.Endpoints;

internal class LoginUserEndpoint(UserManager<ApplicationUser> userManager) : Endpoint<LoginUserRequest>
{
    public override void Configure()
    {
        Post("/users/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginUserRequest req, CancellationToken ct)
    {
        var existingUser = await userManager.FindByEmailAsync(req.Email);

        if (existingUser is null)
        {
            await SendUnauthorizedAsync();
            return;
        }
        
        var loginSucceeded = await userManager.CheckPasswordAsync(existingUser, req.Password);

        if (!loginSucceeded)
        {
            await SendUnauthorizedAsync();
            return;
        }

        var jwtSecret = Config["Auth:JwtSecret"]!;
        var token = JwtBearer.CreateToken(x =>
        {
            x.SigningKey = jwtSecret;
            x.User["Email"] = req.Email;
        });

        await SendAsync(token);
    }
}

internal class LoginUserRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}

internal class CreateUserRequestValidator : Validator<LoginUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.Email).EmailAddress().NotEmpty();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(5);
    }
}