using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;
using MiniAPI.Models;
using MiniAPI.Services.Auth.Commands;
using MiniAPI.Services.Auth.Queries;
using MiniAPI.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;


namespace MiniAPI.Tests;

public class AuthTests
{
    private readonly Mock<UserManager<User>> _userManagerMock;

    private readonly DbContextOptions<MiniAPIContext> _dbOptions;

    private static Mock<UserManager<User>> GetUserManagerMock()
    {
        var store = new Mock<IUserStore<User>>();
        return new Mock<UserManager<User>>(
            store.Object, null!, null!, null!, null!, null!, null!, null!, null!
        );
    }

    public AuthTests()
    {
        _userManagerMock = GetUserManagerMock();
        _dbOptions = new DbContextOptionsBuilder<MiniAPIContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        // _dbMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));
        // _dbMock.Setup(x => x.Add(It.IsAny<Game>()));
    }

    RegisterDTO validRegisterDTO = new RegisterDTO
    {
        FirstName = "Edmund",
        LastName = "Ng",
        Password = "Edmund1234",
        Email = "EdmundNg@gmail.com"
    };

    LoginDTO validLoginDTO = new LoginDTO
    {
        Email = "EdmundNg@gmail.com",
        Password = "Edmund1234"
    };

    private IConfiguration GetTestConfig()
    {
        var inMemorySettings = new Dictionary<string, string> {
            {"Jwt:Key", "THIS_IS_A_SUPER_SECRET_KEY_1234567890!!"}, // 32+ chars for HS256
            {"Jwt:Issuer", "testissuer"},
            {"Jwt:Audience", "testaudience"},
            {"Jwt:ExpireDays", "1"}
        };
#pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        return new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();
#pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
    }


    [Fact]
    public async Task RegisterCommand_ShouldRegisterUser()
    {
        // Arrange
        MiniAPIContext context = new MiniAPIContext(_dbOptions);

        var command = new Register.Command
        {
            Dto = validRegisterDTO
        };

        var mapperMock = new Mock<IMapper>();
        mapperMock
        .Setup(m => m.Map<RegisterDTO, User>(It.IsAny<RegisterDTO>(), It.IsAny<User>()))
        .Callback<RegisterDTO, User>((src, dest) =>
        {
            dest.Email = src.Email!;
            dest.FirstName = src.FirstName!;
            dest.LastName = src.LastName!;
        });
        var mapper = mapperMock.Object;

        _userManagerMock.Setup(u => u.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

        var handler = new Register.Handler(_userManagerMock.Object, mapper);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal("Registration Successful", result);
    }

    [Fact]
    public async Task LoginQuery_ShouldLoginUser()
    {
        // Arrange
        User user = new User
        {
            FirstName = "Edmund",
            LastName = "Ng",
            Email = "EdmundNg@gmail.com",
        };

        var query = new Login.Query
        {
            Dto = validLoginDTO
        };

        _userManagerMock.Setup(u => u.CreateAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
        _userManagerMock.Setup(u => u.FindByEmailAsync("EdmundNg@gmail.com")).ReturnsAsync(user);
        _userManagerMock.Setup(u => u.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>())).ReturnsAsync(true);
        _userManagerMock
        .Setup(u => u.GetRolesAsync(It.IsAny<User>()))
        .ReturnsAsync(new List<string>()); // or new List<string> { "User" }

        // filepath: [AuthorizationTests.cs](http://_vscodecontentref_/3)
        var config = GetTestConfig();

        var handler = new Login.Handler(_userManagerMock.Object, config);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.IsType<string>(result);
    }

    [Fact]
    public async Task Handle_ShouldReturnToken_WhenLoginIsValid()
    {
        // Arrange
        var userManagerMock = GetUserManagerMock();
        var config = GetTestConfig();

        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = "Test",
            LastName = "User",
            Email = "test@example.com",
            Wallet = 100
        };

        var loginDto = new LoginDTO { Email = user.Email, Password = "password" };
        var query = new Login.Query { Dto = loginDto };

        userManagerMock.Setup(m => m.FindByEmailAsync(user.Email)).ReturnsAsync(user);
        userManagerMock.Setup(m => m.CheckPasswordAsync(user, loginDto.Password)).ReturnsAsync(true);
        userManagerMock.Setup(m => m.GetRolesAsync(user)).ReturnsAsync(new List<string> { "User" });

        var handler = new Login.Handler(userManagerMock.Object, config);

        // Act
        var token = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(string.IsNullOrWhiteSpace(token));
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenEmailNotFound()
    {
        // Arrange
        var userManagerMock = GetUserManagerMock();
        var config = GetTestConfig();

        var loginDto = new LoginDTO { Email = "notfound@example.com", Password = "password" };
        var query = new Login.Query { Dto = loginDto };

        userManagerMock.Setup(m => m.FindByEmailAsync(loginDto.Email)).ReturnsAsync((User)null!);

        var handler = new Login.Handler(userManagerMock.Object, config);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => handler.Handle(query, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenPasswordIsIncorrect()
    {
        // Arrange
        var userManagerMock = GetUserManagerMock();
        var config = GetTestConfig();

        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = "Test",
            LastName = "User",
            Email = "test@example.com",
            Wallet = 100
        };

        var loginDto = new LoginDTO { Email = user.Email, Password = "wrongpassword" };
        var query = new Login.Query { Dto = loginDto };

        userManagerMock.Setup(m => m.FindByEmailAsync(user.Email)).ReturnsAsync(user);
        userManagerMock.Setup(m => m.CheckPasswordAsync(user, loginDto.Password)).ReturnsAsync(false);

        var handler = new Login.Handler(userManagerMock.Object, config);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => handler.Handle(query, CancellationToken.None));
    }
}