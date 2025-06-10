using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using MiniAPI.Models;
using MiniAPI.Data;
using MiniAPI.Services.Users.Commands;
using MiniAPI.Services.Users.Queries;

namespace MiniAPI.Tests.UsersTests;

public class UsersTests
{
    private static Mock<UserManager<User>> GetUserManagerMock()
    {
        var store = new Mock<IUserStore<User>>();
        return new Mock<UserManager<User>>(store.Object, null!, null!, null!, null!, null!, null!, null!, null!);
    }

    [Fact]
    public async Task GetUser_ShouldReturnUser_WhenExists()
    {
        var userManagerMock = GetUserManagerMock();
        var user = new User { Id = Guid.NewGuid(), Email = "test@example.com" };
        userManagerMock.Setup(m => m.FindByIdAsync(user.Id.ToString())).ReturnsAsync(user);

        var handler = new GetUser.Handler(userManagerMock.Object);

        var result = await handler.Handle(new GetUser.Query { UserId = user.Id }, CancellationToken.None);

        Assert.Equal(user.Id, result.Id);
        Assert.Equal(user.Email, result.Email);
    }

    [Fact]
    public async Task GetUser_ShouldThrow_WhenNotExists()
    {
        var userManagerMock = GetUserManagerMock();
        userManagerMock.Setup(m => m.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((User)null!);

        var handler = new GetUser.Handler(userManagerMock.Object);

        await Assert.ThrowsAsync<Exception>(() =>
            handler.Handle(new GetUser.Query { UserId = Guid.NewGuid() }, CancellationToken.None));
    }

// [Fact]
// public async Task GetAllUsersAdmin_ShouldReturnAllUsers()
// {
//     var userManagerMock = GetUserManagerMock();
//     var users = new List<User>
//     {
//         new User { Id = Guid.NewGuid(), Email = "a@example.com" },
//         new User { Id = Guid.NewGuid(), Email = "b@example.com" }
//     };

//     userManagerMock.Setup(m => m.Users).Returns(users.AsQueryable());

//     var handler = new GetAllUsersAdmin.Handler(userManagerMock.Object);

//     var result = await handler.Handle(new GetAllUsersAdmin.Query(), CancellationToken.None);

//     Assert.Equal(2, result.Count);
// }

    [Fact]
    public async Task UpdateUser_ShouldUpdateFields()
    {
        var userManagerMock = GetUserManagerMock();
        var userId = Guid.NewGuid();
        var user = new User { Id = userId, Email = "old@example.com", FirstName = "Old", LastName = "Name" };
        var dto = new UserUpdateDTO { Email = "new@example.com", FirstName = "New", LastName = "Name", Password = null };

        userManagerMock.Setup(m => m.FindByIdAsync(userId.ToString())).ReturnsAsync(user);
        userManagerMock.Setup(m => m.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);

        var handler = new UpdateUser.Handler(userManagerMock.Object);

        var result = await handler.Handle(new UpdateUser.Command { UserId = userId, Dto = dto }, CancellationToken.None);

        Assert.Equal(dto.Email, result.Email);
        Assert.Equal(dto.FirstName, result.FirstName);
        Assert.Equal(dto.LastName, result.LastName);
    }

    [Fact]
    public async Task UpdateUser_ShouldThrow_WhenUserNotFound()
    {
        var userManagerMock = GetUserManagerMock();
        userManagerMock.Setup(m => m.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((User)null!);

        var handler = new UpdateUser.Handler(userManagerMock.Object);

        var dto = new UserUpdateDTO { Email = "new@example.com", FirstName = "New", LastName = "Name", Password = null };

        await Assert.ThrowsAsync<Exception>(() =>
            handler.Handle(new UpdateUser.Command { UserId = Guid.NewGuid(), Dto = dto }, CancellationToken.None));
    }

    [Fact]
    public async Task DeleteUser_ShouldDelete_WhenUserExists()
    {
        var userManagerMock = GetUserManagerMock();
        var user = new User { Id = Guid.NewGuid() };

        userManagerMock.Setup(m => m.FindByIdAsync(user.Id.ToString())).ReturnsAsync(user);
        userManagerMock.Setup(m => m.DeleteAsync(user)).ReturnsAsync(IdentityResult.Success);

        var handler = new DeleteUser.Handler(userManagerMock.Object);

        var result = await handler.Handle(new DeleteUser.Command { UserId = user.Id }, CancellationToken.None);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteUser_ShouldThrow_WhenUserNotFound()
    {
        var userManagerMock = GetUserManagerMock();
        userManagerMock.Setup(m => m.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((User)null!);

        var handler = new DeleteUser.Handler(userManagerMock.Object);

        await Assert.ThrowsAsync<Exception>(() =>
            handler.Handle(new DeleteUser.Command { UserId = Guid.NewGuid() }, CancellationToken.None));
    }

    [Fact]
    public async Task AddUserFunds_ShouldAddFunds_WhenUserExists()
    {
        var userManagerMock = GetUserManagerMock();
        var user = new User { Id = Guid.NewGuid(), Wallet = 10m };

        userManagerMock.Setup(m => m.FindByIdAsync(user.Id.ToString())).ReturnsAsync(user);
        userManagerMock.Setup(m => m.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);

        var handler = new AddUserFunds.Handler(userManagerMock.Object);

        var result = await handler.Handle(new AddUserFunds.Command { UserId = user.Id, Funds = 15m }, CancellationToken.None);

        Assert.Equal(25m, result);
    }

    [Fact]
    public async Task AddUserFunds_ShouldThrow_WhenUserNotFound()
    {
        var userManagerMock = GetUserManagerMock();
        userManagerMock.Setup(m => m.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((User)null!);

        var handler = new AddUserFunds.Handler(userManagerMock.Object);

        await Assert.ThrowsAsync<Exception>(() =>
            handler.Handle(new AddUserFunds.Command { UserId = Guid.NewGuid(), Funds = 10m }, CancellationToken.None));
    }
}