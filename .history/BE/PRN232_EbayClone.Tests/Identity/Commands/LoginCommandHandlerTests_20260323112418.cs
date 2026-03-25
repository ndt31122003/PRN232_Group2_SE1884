using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using PRN232_EbayClone.Application.Abstractions.Authentication;
using PRN232_EbayClone.Application.Abstractions.Data;
using PRN232_EbayClone.Application.Identity.Commands;
using PRN232_EbayClone.Domain.Identity.Entities;
using PRN232_EbayClone.Domain.Identity.Errors;
using PRN232_EbayClone.Domain.Roles.Entities;
using PRN232_EbayClone.Domain.Users.Entities;
using PRN232_EbayClone.Domain.Users.Services;
using Xunit;

namespace PRN232_EbayClone.Tests.Identity.Commands;

public sealed class LoginCommandHandlerTests
{
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly IPasswordHasher _passwordHasher = Substitute.For<IPasswordHasher>();
    private readonly ITokenProvider _tokenProvider = Substitute.For<ITokenProvider>();
    private readonly IRefreshTokenRepository _refreshTokenRepository = Substitute.For<IRefreshTokenRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly LoginCommandHandler _handler;

    public LoginCommandHandlerTests()
    {
        _handler = new LoginCommandHandler(
            _userRepository,
            _passwordHasher,
            _tokenProvider,
            _refreshTokenRepository,
            _unitOfWork);
    }

    [Fact]
    public async Task Handle_ShouldLoginUsingEmailIdentifier()
    {
        var user = await CreateUserAsync("demo.seller1@example.com", "hashed-password");
        var command = new LoginCommand("demo.seller1@example.com", "123abc@A");

        _userRepository.GetByUsernameOrEmailAsync(command.Username, Arg.Any<CancellationToken>())
            .Returns(user);
        _passwordHasher.Verify(user.PasswordHash, command.Password).Returns(true);
        _tokenProvider.GenerateAccessToken(user).Returns("access-token");
        _tokenProvider.GenerateRefreshToken().Returns("refresh-token");

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.AccessToken.Should().Be("access-token");
        result.Value.RefreshToken.Should().Be("refresh-token");
        _refreshTokenRepository.Received(1).Add(Arg.Any<RefreshToken>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnInvalidCredentials_WhenPasswordHashIsInvalid()
    {
        var user = await CreateUserAsync("demo.seller1@example.com", "corrupted-hash");
        var command = new LoginCommand("demo.seller1@example.com", "123abc@A");

        _userRepository.GetByUsernameOrEmailAsync(command.Username, Arg.Any<CancellationToken>())
            .Returns(user);
        _passwordHasher
            .When(hasher => hasher.Verify(user.PasswordHash, command.Password))
            .Do(_ => throw new FormatException("Invalid password hash"));

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(IdentityErrors.InvalidCredentials);
        _refreshTokenRepository.DidNotReceive().Add(Arg.Any<RefreshToken>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    private static async Task<User> CreateUserAsync(string email, string passwordHash)
    {
        var emailChecker = Substitute.For<IEmailUniquenessChecker>();
        emailChecker.IsUniqueEmail(Arg.Any<Domain.Shared.ValueObjects.Email>()).Returns(true);

        var userResult = await User.CreateAsync(
            emailChecker,
            "Demo Seller",
            email,
            passwordHash,
            new List<Role>());

        return userResult.Value;
    }
}