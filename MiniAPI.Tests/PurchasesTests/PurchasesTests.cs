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
using MiniAPI.Services.Purchases.Commands;
using MiniAPI.Services.Purchases.Queries;

namespace MiniAPI.Tests.PurchasesTests;

public class PurchasesTests
{
    private static Mock<UserManager<User>> GetUserManagerMock()
    {
        var store = new Mock<IUserStore<User>>();
        return new Mock<UserManager<User>>(store.Object, null!, null!, null!, null!, null!, null!, null!, null!);
    }

    private MiniAPIContext GetInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<MiniAPIContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new MiniAPIContext(options);
    }

    [Fact]
    public async Task PurchaseGame_ShouldSucceed_WhenValid()
    {
        using var context = GetInMemoryContext();
        var userManagerMock = GetUserManagerMock();

        var owner = new User { Id = Guid.NewGuid(), Wallet = 100 };
        var buyer = new User { Id = Guid.NewGuid(), Wallet = 100 };
        var game = new Game { GameID = Guid.NewGuid(), OwnerID = owner.Id, Price = 50 };

        context.Users.Add(owner);
        context.Users.Add(buyer);
        context.Games.Add(game);
        await context.SaveChangesAsync();

        userManagerMock.Setup(m => m.UpdateAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success);

        var handler = new PurchaseGame.Handler(userManagerMock.Object, context);

        var command = new PurchaseGame.Command { GameID = game.GameID, User = buyer };

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(game.GameID, result.GameID);
        Assert.Equal(buyer.Id, result.UserID);
        Assert.Equal(game.Price, result.Amount);
        Assert.Equal(50, buyer.Wallet); // Wallet deducted
    }

    [Fact]
    public async Task PurchaseGame_ShouldThrow_WhenGameDoesNotExist()
    {
        using var context = GetInMemoryContext();
        var userManagerMock = GetUserManagerMock();
        var user = new User { Id = Guid.NewGuid(), Wallet = 100 };

        var handler = new PurchaseGame.Handler(userManagerMock.Object, context);

        var command = new PurchaseGame.Command { GameID = Guid.NewGuid(), User = user };

        await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task PurchaseGame_ShouldThrow_WhenUserIsOwner()
    {
        using var context = GetInMemoryContext();
        var userManagerMock = GetUserManagerMock();
        var user = new User { Id = Guid.NewGuid(), Wallet = 100 };
        var game = new Game { GameID = Guid.NewGuid(), OwnerID = user.Id, Price = 50 };

        context.Users.Add(user);
        context.Games.Add(game);
        await context.SaveChangesAsync();

        var handler = new PurchaseGame.Handler(userManagerMock.Object, context);

        var command = new PurchaseGame.Command { GameID = game.GameID, User = user };

        await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task PurchaseGame_ShouldThrow_WhenAlreadyPurchased()
    {
        using var context = GetInMemoryContext();
        var userManagerMock = GetUserManagerMock();
        var owner = new User { Id = Guid.NewGuid(), Wallet = 100 };
        var buyer = new User { Id = Guid.NewGuid(), Wallet = 100 };
        var game = new Game { GameID = Guid.NewGuid(), OwnerID = owner.Id, Price = 50 };
        var purchase = new Purchase { GameID = game.GameID, UserID = buyer.Id, Amount = 50 };

        context.Users.Add(owner);
        context.Users.Add(buyer);
        context.Games.Add(game);
        context.Purchases.Add(purchase);
        await context.SaveChangesAsync();

        var handler = new PurchaseGame.Handler(userManagerMock.Object, context);

        var command = new PurchaseGame.Command { GameID = game.GameID, User = buyer };

        await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task PurchaseGame_ShouldThrow_WhenInsufficientFunds()
    {
        using var context = GetInMemoryContext();
        var userManagerMock = GetUserManagerMock();
        var owner = new User { Id = Guid.NewGuid(), Wallet = 100 };
        var buyer = new User { Id = Guid.NewGuid(), Wallet = 10 };
        var game = new Game { GameID = Guid.NewGuid(), OwnerID = owner.Id, Price = 50 };

        context.Users.Add(owner);
        context.Users.Add(buyer);
        context.Games.Add(game);
        await context.SaveChangesAsync();

        var handler = new PurchaseGame.Handler(userManagerMock.Object, context);

        var command = new PurchaseGame.Command { GameID = game.GameID, User = buyer };

        await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task RefundGame_ShouldSucceed_WhenValid()
    {
        using var context = GetInMemoryContext();
        var userManagerMock = GetUserManagerMock();
        var user = new User { Id = Guid.NewGuid(), Wallet = 0 };
        var game = new Game { GameID = Guid.NewGuid(), OwnerID = Guid.NewGuid(), Price = 50 };
        var purchase = new Purchase { GameID = game.GameID, UserID = user.Id, Amount = 50 };

        context.Users.Add(user);
        context.Games.Add(game);
        context.Purchases.Add(purchase);
        await context.SaveChangesAsync();

        var handler = new RefundGame.Handler(userManagerMock.Object, context);

        var command = new RefundGame.Command { GameID = game.GameID, User = user };

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.Equal(purchase.PurchaseID, result.PurchaseID);
        Assert.Equal(50, user.Wallet); // Wallet refunded
        Assert.Empty(context.Purchases.ToList());
    }

    [Fact]
    public async Task RefundGame_ShouldThrow_WhenPurchaseDoesNotExist()
    {
        using var context = GetInMemoryContext();
        var userManagerMock = GetUserManagerMock();
        var user = new User { Id = Guid.NewGuid(), Wallet = 0 };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        var handler = new RefundGame.Handler(userManagerMock.Object, context);

        var command = new RefundGame.Command { GameID = Guid.NewGuid(), User = user };

        await Assert.ThrowsAsync<Exception>(() => handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task GetAllPurchases_ShouldReturnAllPurchases()
    {
        using var context = GetInMemoryContext();
        var purchases = new List<Purchase>
        {
            new Purchase { GameID = Guid.NewGuid(), UserID = Guid.NewGuid(), Amount = 10 },
            new Purchase { GameID = Guid.NewGuid(), UserID = Guid.NewGuid(), Amount = 20 }
        };
        context.Purchases.AddRange(purchases);
        await context.SaveChangesAsync();

        var handler = new GetAllPurchases.Handler(context);
        var result = await handler.Handle(new GetAllPurchases.Query(), CancellationToken.None);

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetAllUserPurchases_ShouldReturnUserPurchases()
    {
        using var context = GetInMemoryContext();
        var user = new User { Id = Guid.NewGuid() };
        var purchases = new List<Purchase>
        {
            new Purchase { GameID = Guid.NewGuid(), UserID = user.Id, Amount = 10 },
            new Purchase { GameID = Guid.NewGuid(), UserID = Guid.NewGuid(), Amount = 20 }
        };
        context.Users.Add(user);
        context.Purchases.AddRange(purchases);
        await context.SaveChangesAsync();

        var handler = new GetAllUserPurchases.Handler(GetUserManagerMock().Object, context);
        var result = await handler.Handle(new GetAllUserPurchases.Query { User = user }, CancellationToken.None);

        Assert.Single(result);
        Assert.Equal(user.Id, result[0].UserID);
    }

    [Fact]
    public async Task GetAllGamePurchases_ShouldReturnGamePurchases()
    {
        using var context = GetInMemoryContext();
        var gameId = Guid.NewGuid();
        var purchases = new List<Purchase>
        {
            new Purchase { GameID = gameId, UserID = Guid.NewGuid(), Amount = 10 },
            new Purchase { GameID = Guid.NewGuid(), UserID = Guid.NewGuid(), Amount = 20 }
        };
        context.Purchases.AddRange(purchases);
        await context.SaveChangesAsync();

        var handler = new GetAllGamePurchases.Handler(context);
        var result = await handler.Handle(new GetAllGamePurchases.Query { GameID = gameId }, CancellationToken.None);

        Assert.Single(result);
        Assert.Equal(gameId, result[0].GameID);
    }
}